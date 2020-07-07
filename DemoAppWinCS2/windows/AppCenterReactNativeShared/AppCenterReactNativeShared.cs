using System;

namespace AppCenterReactNativeShared
{
	public sealed class AppCenterReactNativeShared
    {
        public static async void configureAppCenter() {
            Microsoft.AppCenter.AppCenter.SetWrapperSdk(new Microsoft.AppCenter.WrapperSdk("appcenter.rn-windows", Microsoft.AppCenter.AppCenter.SdkVersion));
            var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(
                new Uri("ms-appx:///Assets/app-center-config.json"));
            var content = await Windows.Storage.FileIO.ReadTextAsync(file);
            var secretContainer = Windows.Data.Json.JsonObject.Parse(content);
            var appSecret = secretContainer.GetNamedString("app-secret");
            if(appSecret.Length > 0) {
                Microsoft.AppCenter.AppCenter.Start(appSecret);
            }
            else {
                Microsoft.AppCenter.AppCenter.Start();
            }
        }
    }
}