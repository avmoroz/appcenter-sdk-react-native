#pragma once

#include <NativeModules.h>
#include <winrt/Windows.Foundation.h>
#include <winrt/Windows.Web.Http.Headers.h>
#include <winrt/Windows.Data.Json.h>
#include <winrt/Windows.Storage.h>

#include "debugapi.h"

using namespace winrt::Windows::Web::Http;
using namespace winrt::Windows::Data::Json;

namespace AppCenterAnalytics {

	REACT_MODULE(AppCenterReactNativeAnalytics)
	struct AppCenterReactNativeAnalytics {
        LPWSTR m_body{
			L"{\"logs\":[{\"type\":\"event\",\"timestamp\":\"2020-06-16T04:13:15.301Z\",\"sid\":\"11f46dcc-9b22-463a-8ed9-c82e57f347af\",\"device\":{\"wrapperSdkVersion\":\"3.0.3\",\"wrapperSdkName\":\"appcenter.react-native\",\"sdkName\":\"appcenter.android\",\"sdkVersion\":\"3.2.0\",\"model\":\"5.1-inch Marshmallow (6.0.0) XXHDPI Phone\",\"oemName\":\"VS Emulator\",\"osName\":\"Android\",\"osVersion\":\"6.0\",\"osBuild\":\"MRA58K\",\"osApiLevel\":23,\"locale\":\"en_US\",\"timeZoneOffset\":-420,\"screenSize\":\"1440x2387\",\"appVersion\":\"1.0\",\"appBuild\":\"1\",\"appNamespace\":\"com.demoapp\"},\"name\":\"EventWithProperties\",\"id\":\"dcb25d80-d7ea-4503-9ce5-82ef56bf82b5\",\"typedProperties\":[{\"type\":\"string\",\"name\":\"property1\",\"value\":\"100\"},{\"type\":\"string\",\"name\":\"property2\",\"value\":\"200\"}]}]}"
		};

        bool m_enabled{false};

		REACT_METHOD(SetEnabled, L"setEnabled")
		void SetEnabled(bool&& enabled, 
						winrt::Microsoft::ReactNative::ReactPromise<bool> promise) noexcept {
            OutputDebugStringA("Calling SetEnabled()\n");
			promise.Resolve(m_enabled = !m_enabled);
		}

		REACT_METHOD(IsEnabled, L"isEnabled")
		void IsEnabled(winrt::Microsoft::ReactNative::ReactPromise<bool> promise) noexcept {
            OutputDebugStringA("Calling IsEnabled()\n");
			promise.Resolve(m_enabled);
		}

		REACT_METHOD(TrackEvent, L"trackEvent")
		void TrackEvent(std::wstring&& eventName, 
						winrt::Microsoft::ReactNative::JSValueObject&& properties, 
						winrt::Microsoft::ReactNative::ReactPromise<void> promise) noexcept {
            
			//auto json = buildJSON(eventName, properties);
			//promise.Resolve();
            sendHttpRequest(buildJSON(eventName, properties), promise);
		}

		winrt::hstring buildJSON(std::wstring& eventName, winrt::Microsoft::ReactNative::JSValueObject& properties) {
			JsonObject device;
			device.Insert(L"wrapperSdkVersion", JsonValue::CreateStringValue(L"3.0.3"));
            device.Insert(L"wrapperSdkName", JsonValue::CreateStringValue(L"appcenter.react-native"));
            device.Insert(L"sdkName", JsonValue::CreateStringValue(L"appcenter.android"));
            device.Insert(L"sdkVersion", JsonValue::CreateStringValue(L"3.2.0"));
            device.Insert(L"model", JsonValue::CreateStringValue(L"5.1-inch Marshmallow (6.0.0) XXHDPI Phone"));
            device.Insert(L"oemName", JsonValue::CreateStringValue(L"VS Emulator"));
            device.Insert(L"osName", JsonValue::CreateStringValue(L"Android"));
            device.Insert(L"osVersion", JsonValue::CreateStringValue(L"6.0"));
            device.Insert(L"osBuild", JsonValue::CreateStringValue(L"MRA58K"));
            device.Insert(L"osApiLevel", JsonValue::CreateNumberValue(23));
            device.Insert(L"locale", JsonValue::CreateStringValue(L"en_US"));
            device.Insert(L"timeZoneOffset", JsonValue::CreateNumberValue(-420));
            device.Insert(L"screenSize", JsonValue::CreateStringValue(L"1440x2387"));
            device.Insert(L"appVersion", JsonValue::CreateStringValue(L"1.0"));
            device.Insert(L"appBuild", JsonValue::CreateStringValue(L"1"));
            device.Insert(L"appNamespace", JsonValue::CreateStringValue(L"com.demoapp"));

			JsonArray typedProperties;
            for (auto& pair : properties) {
                JsonObject property;
				property.Insert(L"type", JsonValue::CreateStringValue(L"string"));
                property.Insert(L"name", JsonValue::CreateStringValue(winrt::to_hstring(pair.first)));
				property.Insert(L"value", JsonValue::CreateStringValue(winrt::to_hstring(pair.second.AsString())));
                typedProperties.Append(property);
			}

			JsonObject log;
            log.Insert(L"type", JsonValue::CreateStringValue(L"event"));
            log.Insert(L"timestamp", JsonValue::CreateStringValue(L"2020-06-16T04:13:15.301Z"));
            log.Insert(L"sid", JsonValue::CreateStringValue(L"11f46dcc-9b22-463a-8ed9-c82e57f347af"));
            log.Insert(L"name", JsonValue::CreateStringValue(eventName));
            log.Insert(L"id", JsonValue::CreateStringValue(L"dcb25d80-d7ea-4503-9ce5-82ef56bf82b5"));
            log.Insert(L"typedProperties", typedProperties);
            log.Insert(L"device", device);
			
			JsonArray logs;
            logs.Append(log);
            
			JsonObject outputWrapper;
            outputWrapper.Insert(L"logs", logs);
			
			OutputDebugStringW(outputWrapper.Stringify().c_str());

			return outputWrapper.Stringify();
		}

		void sendHttpRequest(winrt::param::hstring const& json, winrt::Microsoft::ReactNative::ReactPromise<void>& promise) {

			auto file = winrt::Windows::Storage::StorageFile::GetFileFromApplicationUriAsync(
                      winrt::Windows::Foundation::Uri{L"ms-appx:///Assets/app-center-config.json"});
            auto content = winrt::Windows::Storage::FileIO::ReadTextAsync(file.get()).get();
            auto secretContainer = JsonObject::Parse(content);
            auto appSecret = secretContainer.Lookup(L"app-secret").GetString();

			HttpClient httpClient;
			auto headers{httpClient.DefaultRequestHeaders()};
			//headers.Append(L"App-Secret", L"b924f8cc-a99e-466d-9dc5-47bccccb726a");
            headers.Append(L"App-Secret", appSecret);
			headers.Append(L"Install-ID", L"728c5cf5-6124-4aef-8ac6-524992147b55");

			HttpStringContent stringContent{json};
            //OutputDebugStringW();
			stringContent.Headers().Insert(L"Content-Type", L"application/json");

			HttpResponseMessage httpResponseMessage;
			std::wstring httpResponseBody;

			try {
				// Send the POST request.
				winrt::Windows::Foundation::Uri requestUri{L"https://in.appcenter.ms/logs?api-version=1.0.0"};
				httpResponseMessage = httpClient.PostAsync(requestUri, stringContent).get();
				httpResponseMessage.EnsureSuccessStatusCode();
				httpResponseBody = httpResponseMessage.Content().ReadAsStringAsync().get();
				promise.Resolve();
			} catch (winrt::hresult_error const &ex) {
				httpResponseBody = ex.message();
				promise.Reject(ex.message().c_str());
			}
			OutputDebugStringW(httpResponseBody.c_str());
		}

		// I have no idea what these do; the MS docs don't mention them at all
		/*
		REACT_METHOD(TrackTransmissionTargetEvent, L"trackTransmissionTargetEvent")
		void TrackTransmissionTargetEvent(std::string &&eventName, winrt::Microsoft::ReactNative::JSValueObject &&properties,
                                                  std::string &&targetToken) noexcept {}
		
		REACT_METHOD(GetTransmissionTarget, L"getTransmissionTarget")
		void GetTransmissionTarget(std::string&& targetToken) noexcept {}

		REACT_METHOD(IsTransmissionTargetEnabled, L"isTransmissionTargetEnabled")
		void IsTransmissionTargetEnabled(std::string&& targetToken) noexcept {}

		REACT_METHOD(SetTransmissionTargetEnabled, L"setTransmissionTargetEnabled")
		void SetTransmissionTargetEnabled(bool&& enabled, std::string&& targetToken) noexcept {}

		REACT_METHOD(SetTransmissionTargetEventProperty, L"setTransmissionTargetEventProperty")
		void SetTransmissionTargetEventProperty(std::string&& propertyKey, std::string&& propertyValue, std::string&& targetToken) noexcept {}
		
		REACT_METHOD(RemoveTransmissionEventProperty, L"removeTransmissionEventProperty")
		void RemoveTransmissionEventProperty(std::string&& propertyKey, std::string&& propertyToken) noexcept {}

		REACT_METHOD(CollectTransmissionTargetDeviceId, L"collectTransmissionTargetDeviceId")
		void CollectTransmissionTargetDeviceId(std::string&& targetToken) noexcept {}

		REACT_METHOD(GetChildTransmissionTarget, L"getChildTransmissionTarget")
		void GetChildTransmissionTarget(std::string&& childToken, std::string&& parentToken) noexcept {}

		REACT_METHOD(SetTransmissionTargetAppName, L"setTransmissionTargetAppName")
		void SetTransmissionTargetAppName(std::string&& appName, std::string targetToken) noexcept {}

		REACT_METHOD(SetTransmissionTargetAppVersion, L"setTransmissionTargetAppVersion")
		void SetTransmissionTargetAppVersion(std::string&& appVersion, std::string&& targetToken) noexcept {}

		REACT_METHOD(SetTransmissionTargetAppLocale, L"setTransmissionTargetAppLocale")
		void SetTransmissionTargetAppLocale(std::string&& appLocale, std::string&& targetToken) noexcept {}
		*/
	};

}; // namespace AppCenter
