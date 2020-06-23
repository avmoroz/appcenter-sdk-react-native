using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;

namespace DemoAppWinCS
{
	class temp {
		public temp() {
			AppCenter.Start("{8cb33970-8b2a-486c-ad41-58006048d74a}", typeof(Analytics));
		}
	}

	[ReactModule]
	class AppCenterReactNativeAnalytics
	{
		//private bool m_enabled = false;
		private temp toStart = new temp();
		//private var mTransmissionTargets = new Dictionary<string, AnalyticsTransmissionTarget>();

		//AppCenter.Start("{8cb33970-8b2a-486c-ad41-58006048d74a}", typeof(Analytics));
		//AppCenterReactNativeAnalytics() {
			//	
			//	m_enabled = false;
		//}

		[ReactMethod("setEnabled")]
		public void SetEnabled(bool enabled, ReactPromise<JSValue> promise) {
			//Analytics
			Debug.WriteLine("Calling SetEnabled()");
			Analytics.SetEnabledAsync(enabled);
			promise.Resolve(JSValue.Null);
		}

		[ReactMethod("isEnabled")]
		public void IsEnabled(ReactPromise<bool> promise) {
			Debug.WriteLine("Calling IsEnabled()\n");
			promise.Resolve(Analytics.IsEnabledAsync().Result);
		}

		[ReactMethod("trackEvent")]
		public void TrackEvent(string eventName,
						Dictionary<string, string> properties,
						ReactPromise<JSValue> promise) {
			Analytics.TrackEvent(eventName, properties); // Java implementation puts this in a try-catch block. Not sure why
			promise.Resolve(JSValue.Null);
		}

		[ReactMethod("trackTargetTransmissionEvent")]
		public void TrackTransmissionTargetEvent(string eventName,
						Dictionary<string, string> properties,
						string targetToken,
						ReactPromise<JSValue> promise) {
			promise.Resolve(JSValue.Null);
		}

		[ReactMethod("getTransmissionTarget")]
		public void GetTransmissionTarget(string targetToken, ReactPromise<JSValue> promise) {
			promise.Resolve(JSValue.Null);
		}

		[ReactMethod("isTransmissionTargetEnabled")]
		public void IsTransmissionTargetEnabled(string targetToken, ReactPromise<JSValue> promise) {
			promise.Resolve(JSValue.Null);
		}

		[ReactMethod("setTransmissionTargetEnabled")]
		public void SetTransmissionTargetEnabled(bool enabled, string targetToken, ReactPromise<JSValue> promise) {
			promise.Resolve(JSValue.Null);
		}

		[ReactMethod("setTransmissionTargetEventProperty")]
		public void SetTransmissionTargetEventProperty(string propertyKey, 
						string propertyValue, 
						string targetToken, 
						ReactPromise<JSValue> promise) {
			promise.Resolve(JSValue.Null);
		}

		[ReactMethod("removeTransmissionTargetEventProperty")]
		public void RemoveTransmissionTargetEventProperty(string propertyKey, string targetToken, ReactPromise<JSValue> promise) {
			promise.Resolve(JSValue.Null);
		}

		[ReactMethod("collectTransmissionTargetDeviceId")]
		public void CollectTransmissionTargetDeviceId(string targetToken, ReactPromise<JSValue> promise) {
			promise.Resolve(JSValue.Null);
		}

		[ReactMethod("getChildTransmissionTarget")]
		public void GetChildTransmissionTarget(string childToken, string parentToken, ReactPromise<JSValue> promise) {
			promise.Resolve(JSValue.Null);
		}

		[ReactMethod("setTransmissionTargetAppName")]
		public void SetTransmissionTargetAppName(string appName, string targetToken, ReactPromise<JSValue> promise) {
			promise.Resolve(JSValue.Null);
		}

		[ReactMethod("setTransmissionTargetAppVersion")]
		public void SetTransmissionTargetAppVersion(string appVersion, string targetToken, ReactPromise<JSValue> promise) {
			promise.Resolve(JSValue.Null);
		}

		[ReactMethod("setTransmissionTargetAppLocale")]
		public void SetTransmissionTargetAppLocale(string appLocale, string targetToken, ReactPromise<JSValue> promise) {
			promise.Resolve(JSValue.Null);
		}

	}

}
