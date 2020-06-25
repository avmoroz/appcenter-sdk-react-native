using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Windows.System;
using Microsoft.AppCenter.Ingestion.Models;

namespace DemoAppWinCS
{
	[ReactModule]
	class AppCenterReactNativeCrashes
	{
		private const int DONT_SEND = 0;
		private const int SEND = 1;
		private const int ALWAYS_SEND = 2;

		public AppCenterReactNativeCrashes() {
			Crashes.SendingErrorReport += Crashes_SendingErrorReport;
			StartAppCenter();
		}

		private static async void StartAppCenter() {
			var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(
				new Uri("ms-appx:///Assets/app-center-config.json"));
			var content = await Windows.Storage.FileIO.ReadTextAsync(file);
			var secretContainer = Windows.Data.Json.JsonObject.Parse(content);
			var appSecret = secretContainer.GetNamedString("app-secret");

			AppCenter.Start(appSecret, typeof(Crashes));
		}

		[ReactMethod("setEnabled")]
		public void SetEnabled(bool enabled, ReactPromise<JSValue> promise) {
			Crashes.SetEnabledAsync(enabled);
			promise.Resolve(JSValue.Null);
		}

		[ReactMethod("isEnabled")]
		public void IsEnabled(ReactPromise<bool> promise) {
			promise.Resolve(Crashes.IsEnabledAsync().Result);
		}

		[ReactMethod("hasCrashedInLastSession")]
		public void HasCrashedInLastSession(ReactPromise<bool> promise) {
			promise.Resolve(Crashes.HasCrashedInLastSessionAsync().Result);
		}

		[ReactMethod("hasReceivedMemoryWarningInLastSession")]
		public void HasReceivedMemoryWarningInLastSession(ReactPromise<bool> promise) {
			promise.Resolve(Crashes.HasReceivedMemoryWarningInLastSessionAsync().Result);
		}

		[ReactMethod("generateTestCrash")]
		public void GenerateTestCrash(ReactPromise<JSValue> promise) {
			Crashes.GenerateTestCrash();
			promise.Resolve(JSValue.Null);
		}

		[ReactMethod("notifyWithUserConfirmation")]
		public void NotifyWithUserConfirmation(int userConfirmation) {
			switch (userConfirmation) {
				case DONT_SEND:
					Crashes.NotifyUserConfirmation(UserConfirmation.DontSend);
					break;
				case SEND:
					Crashes.NotifyUserConfirmation(UserConfirmation.Send);
					break;
				case ALWAYS_SEND:
					Crashes.NotifyUserConfirmation(UserConfirmation.AlwaysSend);
					break;
			}
		}

		private void Crashes_SendingErrorReport(object sender, SendingErrorReportEventArgs e) {
			onBeforeSending?.Invoke(e.Report);
		}

		[ReactEvent]
		public Action<ErrorReport> onBeforeSending { get; set; }

	}
	static class ErrorReportWriter
	{
		public static void WriteValue(this IJSValueWriter writer, ErrorReport errorReport) {
			writer.WriteObjectBegin();
			if (errorReport != null) {

				writer.WriteObjectProperty("id", errorReport.Id);
				//writer.WriteObjectProperty("threadName", errorReport.ThreadName); Should list thread, but it is not a property of ErrorReport. A UWP issue
				writer.WriteObjectProperty("appErrorTime", errorReport.AppErrorTime.ToUnixTimeMilliseconds());

				writer.WriteObjectProperty("appStartTime", errorReport.AppStartTime.ToUnixTimeMilliseconds());

				var stackTrace = errorReport.StackTrace;
				if (stackTrace != null) {
					writer.WriteObjectProperty("exception", stackTrace);
				}

				/* Convert device info. */
				var deviceInfo = errorReport.Device;
				if (deviceInfo != null) {
					writer.WriteObjectProperty("device", deviceInfo);
				}
	
			}
			writer.WriteObjectEnd();
		}

		public static void WriteValue(this IJSValueWriter writer, Microsoft.AppCenter.Device deviceInfo) {
			writer.WriteObjectBegin();
			writer.WriteObjectProperty("appBuild", deviceInfo.AppBuild);
			writer.WriteObjectProperty("appNamespace", deviceInfo.AppNamespace);
			writer.WriteObjectProperty("appVersion", deviceInfo.AppVersion);
			writer.WriteObjectProperty("carrierCountry", deviceInfo.CarrierCountry);
			writer.WriteObjectProperty("carrierName", deviceInfo.CarrierName);
			writer.WriteObjectProperty("locale", deviceInfo.Locale);
			writer.WriteObjectProperty("model", deviceInfo.Model);
			writer.WriteObjectProperty("oemName", deviceInfo.OemName);
			writer.WriteObjectProperty("osAPILevel", deviceInfo.OsApiLevel);
			writer.WriteObjectProperty("osBuild", deviceInfo.OsBuild);
			writer.WriteObjectProperty("osName", deviceInfo.OsName);
			writer.WriteObjectProperty("osVersion", deviceInfo.OsVersion);
			writer.WriteObjectProperty("screenSize", deviceInfo.ScreenSize);
			writer.WriteObjectProperty("sdkName", deviceInfo.SdkName);
			writer.WriteObjectProperty("sdkVersion", deviceInfo.SdkVersion);
			writer.WriteObjectProperty("timeZoneOffset", deviceInfo.TimeZoneOffset);
			writer.WriteObjectEnd();
		}

	}

		class AppCenterReactNativeCrashesListener { 
		
	}
}
