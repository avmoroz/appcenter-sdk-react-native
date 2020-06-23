using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.ReactNative.Managed;
using System.Collections.Generic;

namespace DemoAppWinCS
{

	[ReactModule]
	class AppCenterReactNativeAnalytics
	{
		public AppCenterReactNativeAnalytics() {
			AppCenter.Start("@app-secret", typeof(Analytics));
		}

		[ReactMethod("setEnabled")]
		public void SetEnabled(bool enabled, ReactPromise<JSValue> promise) {
			Analytics.SetEnabledAsync(enabled);
			promise.Resolve(JSValue.Null);
		}

		[ReactMethod("isEnabled")]
		public void IsEnabled(ReactPromise<bool> promise) {
			promise.Resolve(Analytics.IsEnabledAsync().Result);
		}

		[ReactMethod("trackEvent")]
		public void TrackEvent(string eventName,
						Dictionary<string, string> properties,
						ReactPromise<JSValue> promise) {
			Analytics.TrackEvent(eventName, properties);
			promise.Resolve(JSValue.Null);
		}
	}
}
