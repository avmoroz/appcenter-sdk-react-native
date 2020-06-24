using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;

namespace DemoAppWinCS
{
	[ReactModule]
	class AppCenterReactNativeCrashes
	{
		public AppCenterReactNativeCrashes() {
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
	}
}
