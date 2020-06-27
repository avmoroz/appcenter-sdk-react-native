using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.ReactNative;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace DemoAppWinCS
{
	sealed partial class App : ReactApplication
	{
		public const string LogTag = "AppCenterDemo";
		public App() {
			MainComponentName = "DemoAppWinCS";

#if BUNDLE
            JavaScriptBundleFile = "index.windows";
            InstanceSettings.UseWebDebugger = false;
            InstanceSettings.UseFastRefresh = false;
#else
			JavaScriptMainModuleName = "index";
			InstanceSettings.UseWebDebugger = true;
			InstanceSettings.UseFastRefresh = true;
#endif

#if DEBUG
			InstanceSettings.EnableDeveloperMenu = true;
#else
            InstanceSettings.EnableDeveloperMenu = false;
#endif

			Microsoft.ReactNative.Managed.AutolinkedNativeModules.RegisterAutolinkedNativeModulePackages(PackageProviders); // Includes any autolinked modules

			PackageProviders.Add(new Microsoft.ReactNative.Managed.ReactPackageProvider()); // Includes any modules in this project
																							//PackageProviders.Add(new DemoAppWinCS.ReactPackageProvider());

			InitializeComponent();
		}

		protected override void OnLaunched(LaunchActivatedEventArgs e) {
			base.OnLaunched(e);
			StartAppCenter();
		}

		private async void StartAppCenter() {


			var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(
				new Uri("ms-appx:///Assets/app-center-config.json"));
			var content = await Windows.Storage.FileIO.ReadTextAsync(file);
			var secretContainer = Windows.Data.Json.JsonObject.Parse(content);
			var appSecret = secretContainer.GetNamedString("app-secret");

			if (!AppCenter.Configured) {
				AppCenterLog.Assert(LogTag, "AppCenter.LogLevel=" + AppCenter.LogLevel);
				AppCenter.LogLevel = LogLevel.Verbose;
				AppCenterLog.Info(LogTag, "AppCenter.LogLevel=" + AppCenter.LogLevel);
				AppCenterLog.Info(LogTag, "AppCenter.Configured=" + AppCenter.Configured);

				Crashes.ShouldProcessErrorReport = ShouldProcess;
				Crashes.ShouldAwaitUserConfirmation = ConfirmationHandler;

				AppCenterLog.Assert(LogTag, "AppCenter.Configured=" + AppCenter.Configured);

				AppCenter.Start(appSecret, typeof(Analytics), typeof(Crashes));
				Console.WriteLine(AppCenter.Configured);

				_ = AppCenter.IsEnabledAsync().ContinueWith(enabled =>
				  {
					  AppCenterLog.Info(LogTag, "AppCenter.Enabled=" + enabled.Result);
				  });
				_ = AppCenter.GetInstallIdAsync().ContinueWith(installId =>
				  {
					  AppCenterLog.Info(LogTag, "AppCenter.InstallId=" + installId.Result);
				  });
				AppCenterLog.Info(LogTag, "AppCenter.SdkVersion=" + AppCenter.SdkVersion);
				_ = Crashes.HasCrashedInLastSessionAsync().ContinueWith(hasCrashed =>
				  {
					  AppCenterLog.Info(LogTag, "Crashes.HasCrashedInLastSession=" + hasCrashed.Result);
				  });
				_ = Crashes.GetLastSessionCrashReportAsync().ContinueWith(task =>
				  {
					  AppCenterLog.Info(LogTag, "Crashes.LastSessionCrashReport.StackTrace=" + task.Result?.StackTrace);
				  });
			}

			var dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Tick += (object sender, object e) => {

#pragma warning disable CS0219
				Debugger.Log(0, "aaa", (42 / int.Parse("0")).ToString());
#pragma warning restore CS0219
			};
			dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
			//dispatcherTimer.Start();
		}

		bool ShouldProcess(ErrorReport report) {
			AppCenterLog.Info(LogTag, "Determining whether to process error report");
			return true;
		}

		bool ConfirmationHandler() {
			_ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
			() =>
			{
				//Current. MainPage.DisplayActionSheet("Crash detected. Send anonymous crash report?", null, null, "Send", "Always Send", "Don't Send").ContinueWith((arg) =>
				//{
				//	var answer = arg.Result;
				//	UserConfirmation userConfirmationSelection;
				//	if (answer == "Send") {
				//		userConfirmationSelection = UserConfirmation.Send;
				//	}
				//	else if (answer == "Always Send") {
				//		userConfirmationSelection = UserConfirmation.AlwaysSend;
				//	}
				//	else {
				//		userConfirmationSelection = UserConfirmation.DontSend;
				//	}
				//	AppCenterLog.Debug(LogTag, "User selected confirmation option: \"" + answer + "\"");
				//	Crashes.NotifyUserConfirmation(userConfirmationSelection);
				//});
			});
			return true;
		}
	}
}
