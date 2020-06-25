using Microsoft.ReactNative.Managed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;

using System.Reflection.Metadata;
using System.Security.Cryptography;
using Windows.Networking.Vpn;
using System.Diagnostics;
using System.ServiceModel.Description;

namespace DemoAppWinCS
{
    [ReactModule]
    class AppCenterReactNative
    {
        public AppCenterReactNative()
        {
            StartAppCenter();
        }

        private static async void StartAppCenter()
        {
            var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(
                new Uri("ms-appx:///Assets/app-center-config.json"));
            var content = await Windows.Storage.FileIO.ReadTextAsync(file);
            var secretContainer = Windows.Data.Json.JsonObject.Parse(content);
            var appSecret = secretContainer.GetNamedString("app-secret");

            AppCenter.Start(appSecret, typeof(Analytics));
        }

        [ReactMethod("startFromLibrary")]
        //public void StartFromLibrary(JSValueObject service)
        public void StartFromLibrary(Dictionary<string, string> service)
        {
            Debug.WriteLine("Number of keys: " + service.Keys.Count);
            Debug.WriteLine(service["Foo"]);
            var properties = new CustomProperties();
            //properties.PlatformSet();
            //AppCenter.
        }

        [ReactMethod("setEnabled")]
        public async void SetEnabled(bool enabled, ReactPromise<bool> promise)
        {
            await AppCenter.SetEnabledAsync(enabled);
            promise.Resolve(enabled);
        }

        [ReactMethod("isEnabled")]
        public async void IsEnabled(ReactPromise<bool> promise)
        {
            var test = await AppCenter.IsEnabledAsync();
            //promise.Resolve(await AppCenter.IsEnabledAsync());
            promise.Resolve(test);
        }

        [ReactMethod("setLogLevel")]
        public void SetLogLevel(int logLevel)
        {
            AppCenter.LogLevel = (Microsoft.AppCenter.LogLevel)logLevel;
        }

        [ReactMethod("getLogLevel")]
        public void GetLogLevel(ReactPromise<int> promise)
        {
            promise.Resolve((int)AppCenter.LogLevel);
        }

        [ReactMethod("getInstallId")]
        public async void GetInstallId(ReactPromise<string> promise)
        {
            promise.Resolve((await AppCenter.GetInstallIdAsync()).ToString());
        }

        [ReactMethod("setUserId")]
        public void SetUserId(String userId)
        {
            AppCenter.SetUserId(userId);
        }

        [ReactMethod("setCustomProperties")]
        public void SetCustomProperties(JSValue properties)
        {
            CustomProperties customProperties = new CustomProperties();
            foreach(var key in properties.AsObject().Keys)
            {
                var valueObject = properties[key];
                var type = valueObject["type"];
                var value = valueObject["value"];
                switch (type.AsString())
                {
                    case "string":
                        customProperties.Set(key, value.AsString());
                        break;
                    case "number":
                        customProperties.Set(key, value.AsDouble());
                        break;
                    case "boolean":
                        customProperties.Set(key, value.AsBoolean());
                        break;
                    case "date-time":
                        customProperties.Set(key, DateTimeOffset.FromUnixTimeMilliseconds(value.AsInt64()).UtcDateTime);
                        break;
                    case "clear":
                        customProperties.Clear(key);
                        break;
                }
            }
            AppCenter.SetCustomProperties(customProperties);
        }

        /*
         * 
         * @SuppressWarnings("unchecked")
    @ReactMethod
    public void startFromLibrary(ReadableMap service) {
        String type = service.getString("bindingType");
        try {
            AppCenter.startFromLibrary(mApplication, new Class[]{ Class.forName(type) });
        } catch (ClassNotFoundException e) {
            AppCenterLog.error(LOG_TAG, "Unable to resolve App Center module", e);
        }
    }

    @ReactMethod
    public void setEnabled(boolean enabled, final Promise promise) {
        AppCenter.setEnabled(enabled).thenAccept(new AppCenterConsumer<Void>() {

            @Override
            public void accept(Void result) {
                promise.resolve(result);
            }
        });
    }

    @ReactMethod
    public void isEnabled(final Promise promise) {
        AppCenter.isEnabled().thenAccept(new AppCenterConsumer<Boolean>() {

            @Override
            public void accept(Boolean enabled) {
                promise.resolve(enabled);
            }
        });
    }

    @ReactMethod
    public void setLogLevel(int logLevel) {
        AppCenter.setLogLevel(logLevel);
    }

    @ReactMethod
    public void getLogLevel(final Promise promise) {
        int logLevel = AppCenter.getLogLevel();
        promise.resolve(logLevel);
    }

    @ReactMethod
    public void getInstallId(final Promise promise) {
        AppCenter.getInstallId().thenAccept(new AppCenterConsumer<UUID>() {

            @Override
            public void accept(UUID installId) {
                promise.resolve(installId == null ? null : installId.toString());
            }
        });
    }

    @ReactMethod
    public void setUserId(String userId) {
        AppCenter.setUserId(userId);
    }

    @ReactMethod
    public void setCustomProperties(ReadableMap properties) {
        AppCenter.setCustomProperties(ReactNativeUtils.toCustomProperties(properties));
    }
         */
    }
}
