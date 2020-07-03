using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;
using System;
using Windows.ApplicationModel.Activation;


namespace DemoAppWinCS
{
    sealed partial class App : ReactApplication
    {
        public App()
        {
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
            PackageProviders.Add(new AppCenter.ReactPackageProvider());
            PackageProviders.Add(new AppCenterReactNativeAnalytics.ReactPackageProvider());
            PackageProviders.Add(new AppCenterReactNativeCrashes.ReactPackageProvider());

            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            base.OnLaunched(e);
            StartAppCenter();
        }

        private static async void StartAppCenter()
        {
            var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(
                new Uri("ms-appx:///Assets/app-center-config.json"));
            var content = await Windows.Storage.FileIO.ReadTextAsync(file);
            var secretContainer = Windows.Data.Json.JsonObject.Parse(content);
            var appSecret = secretContainer.GetNamedString("app-secret");
            Microsoft.AppCenter.AppCenter.Start(appSecret, typeof(Analytics), typeof(Crashes));
        }
    }
}
