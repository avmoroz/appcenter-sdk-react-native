#pragma once
#include <NativeModules.h>
#include "debugapi.h"
#include <winrt/Windows.Foundation.h>
#include <winrt/Windows.Web.Http.Headers.h>



//using namespace winrt;
//using namespace Windows::Foundation;

namespace AppCenter {

REACT_MODULE(AppCenterReactNativeAnalytics)
struct AppCenterReactNativeAnalytics {

	// We need to include an MSTargetDictionary of some sort
	// Seems like we need to call some methods within AppCenter and AppCenterShared. This changes the scope of things
  bool enabled{false};

  const std::wstring App_Secret{L"@app-secret"};
  const std::wstring Install_ID{L"@install-id"};
  const std::string Host{"in.appcenter.ms"};

	REACT_METHOD(TrackEvent, L"trackEvent")
	void TrackEvent(std::string&& eventName, 
		winrt::Microsoft::ReactNative::JSValueObject&& properties,
		winrt::Microsoft::ReactNative::ReactPromise<void> &&promise) noexcept {
		std::string output = eventName + "\n";
		makeGetRequest();
		makePostRequest();
		OutputDebugStringA(output.c_str());
		promise.Resolve();
	}

	void makeGetRequest() {
		// Create an HttpClient object.
		winrt::Windows::Web::Http::HttpClient httpClient;
		auto headers{httpClient.DefaultRequestHeaders()};
		
		std::wstring header{L"ie"};
		
		if (!headers.UserAgent().TryParseAdd(header)) {
			throw L"Invalid header value: " + header;
		}
		
		header = L"Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
		if (!headers.UserAgent().TryParseAdd(header)) {
			throw L"Invalid header value: " + header;
		}
		
		winrt::Windows::Foundation::Uri requestUri{L"http://www.contoso.com"};
		
		// Send the GET request asynchronously, and retrieve the response as a string.
		winrt::Windows::Web::Http::HttpResponseMessage httpResponseMessage;
		std::wstring httpResponseBody;
		
		try {
			// Send the GET request.
			httpResponseMessage = httpClient.GetAsync(requestUri).get();
			httpResponseMessage.EnsureSuccessStatusCode();
			httpResponseBody = httpResponseMessage.Content().ReadAsStringAsync().get();
		} catch (winrt::hresult_error const &ex) {
			httpResponseBody = ex.message();
		}
	}

	void makePostRequest() {

		winrt::Windows::Web::Http::HttpStringContent stringContent{L"{\"logs\":[{\"type\":\"event\",\"timestamp\":\"2020-06-16T04:13:15.301Z\",\"sid\":\"11f46dcc-9b22-463a-8ed9-c82e57f347af\",\"device\":{\"wrapperSdkVersion\":\"3.0.3\",\"wrapperSdkName\":\"appcenter.react-native\",\"sdkName\":\"appcenter.android\",\"sdkVersion\":\"3.2.0\",\"model\":\"Universal Windows Potato Device\",\"oemName\":\"VS Emulator\",\"osName\":\"Windows\",\"osVersion\":\"10.0\",\"osBuild\":\"MRA58K\",\"osApiLevel\":23,\"locale\":\"en_US\",\"timeZoneOffset\":-420,\"screenSize\":\"1440x2387\",\"appVersion\":\"1.0\",\"appBuild\":\"1\",\"appNamespace\":\"com.demoapp\"},\"name\":\"EventWithProperties\",\"id\":\"dcb25d80-d7ea-4503-9ce5-82ef56bf82b5\",\"typedProperties\":[{\"type\":\"string\",\"name\":\"property1\",\"value\":\"100\"},{\"type\":\"string\",\"name\":\"property2\",\"value\":\"200\"}]}]}"};
		// You can use the 'image/jpeg' content type to represent any binary data;
		// it's not necessarily an image file.

		winrt::Windows::Web::Http::HttpClient httpClient;
		auto headers{httpClient.DefaultRequestHeaders()};
		headers.Append(L"App-Secret", App_Secret);
                headers.Append(L"User-Agent",
                               L"Universal Windows Potato");
		headers.Append(L"Install-ID", Install_ID);

		stringContent.Headers().Insert(L"Content-Type", L"application/json");

          // Send the POST request asynchronously, and retrieve the response as a string.
          winrt::Windows::Web::Http::HttpResponseMessage httpResponseMessage;
          std::wstring httpResponseBody;

          try {
            // Send the POST request.
            winrt::Windows::Foundation::Uri requestUri{L"https://in.appcenter.ms/logs?api-version=1.0.0"};
            
            httpResponseMessage = httpClient.PostAsync(requestUri, stringContent).get();
            httpResponseMessage.EnsureSuccessStatusCode();
            httpResponseBody = httpResponseMessage.Content().ReadAsStringAsync().get();
          } catch (winrt::hresult_error const &ex) {
            httpResponseBody = ex.message();
          }
	}

	REACT_METHOD(IsEnabled, L"isEnabled")
	void IsEnabled(winrt::Microsoft::ReactNative::ReactPromise<bool>&& promise) noexcept {
		//promise.WriteTo(false);
		OutputDebugStringA("isEnabled\n");
		promise.Resolve(enabled);
	}

	REACT_METHOD(SetEnabled, L"setEnabled")
        void SetEnabled(bool shouldEnable,
                        winrt::Microsoft::ReactNative::ReactPromise<void> &&promise) noexcept {
		OutputDebugStringA("setEnabled\n");
		enabled = !enabled;
		promise.Resolve();
	}

	REACT_METHOD(TrackTransmissionTargetEvent, L"trackTransmissionTargetEvent")
        void TrackTransmissionTargetEvent(std::string&& eventName, 
			winrt::Microsoft::ReactNative::JSValueObject&& properties,
			std::string&& targetToken,
			winrt::Microsoft::ReactNative::ReactPromise<void>&& promise) noexcept {
		OutputDebugStringA("trackTransmissionTargetEvent\n");
		promise.Resolve();
	}

	REACT_METHOD(GetTransmissionTarget, L"getTransmissionTarget")
	void GetTransmissionTarget(std::string&& targetToken,
		winrt::Microsoft::ReactNative::JSValue &&promise) noexcept {
		OutputDebugStringA("getTransmissionTarget\n");
	}

	REACT_METHOD(IsTransmissionTargetEnabled, L"isTransmissionTargetEnabled")
	void IsTransmissionTargetEnabled(std::string &&targetToken,
		winrt::Microsoft::ReactNative::JSValue &&promise) noexcept {
		OutputDebugStringA("isTransmissionTargetEnabled\n");
	}



};

}