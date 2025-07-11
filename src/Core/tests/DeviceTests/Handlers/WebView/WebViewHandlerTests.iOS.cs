using Microsoft.Maui.Handlers;
using WebKit;
using System.Threading.Tasks;
using System;
using Microsoft.Maui.Controls;
using System.Threading;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	public partial class WebViewHandlerTests
	{
		WKWebView GetNativeWebView(WebViewHandler webViewHandler) =>
			webViewHandler.PlatformView;

		string GetNativeSource(WebViewHandler webViewHandler) =>
			GetNativeWebView(webViewHandler).Url.AbsoluteString;

		[Fact(DisplayName = "Reload with HtmlWebViewSource should succeed")]
		public async Task ReloadWithHtmlWebViewSourceShouldSucceed()
		{
			var webView = new WebView();
			var htmlSource = new HtmlWebViewSource
			{
				Html = "<html><body><h1>Test Page</h1><p>Initial content</p></body></html>"
			};
			
			webView.Source = htmlSource;

			var loadTcs = new TaskCompletionSource<WebNavigationResult>();
			var reloadTcs = new TaskCompletionSource<WebNavigationResult>();
			var navigatedCount = 0;

			webView.Navigated += (sender, args) =>
			{
				navigatedCount++;
				if (navigatedCount == 1)
				{
					// First navigation (initial load)
					loadTcs.TrySetResult(args.Result);
				}
				else if (navigatedCount == 2)
				{
					// Second navigation (reload)
					reloadTcs.TrySetResult(args.Result);
				}
			};

			await InvokeOnMainThreadAsync(async () =>
			{
				var handler = CreateHandler<WebViewHandler>(webView);
				
				// Wait for initial load to complete
				var loadResult = await loadTcs.Task.WaitAsync(TimeSpan.FromSeconds(10));
				Assert.Equal(WebNavigationResult.Success, loadResult);

				// Now test reload
				webView.Reload();

				// Wait for reload to complete
				var reloadResult = await reloadTcs.Task.WaitAsync(TimeSpan.FromSeconds(10));
				
				// This should succeed, not fail
				Assert.Equal(WebNavigationResult.Success, reloadResult);
			});
		}

		[Fact(DisplayName = "Reload with UrlWebViewSource should still work")]
		public async Task ReloadWithUrlWebViewSourceShouldWork()
		{
			// Skip test if no internet
			if (await AssertionExtensions.SkipTestIfNoInternetConnection())
				return;

			var webView = new WebView();
			var urlSource = new UrlWebViewSource
			{
				Url = "https://www.example.com"
			};
			
			webView.Source = urlSource;

			var loadTcs = new TaskCompletionSource<WebNavigationResult>();
			var reloadTcs = new TaskCompletionSource<WebNavigationResult>();
			var navigatedCount = 0;

			webView.Navigated += (sender, args) =>
			{
				navigatedCount++;
				if (navigatedCount == 1)
				{
					// First navigation (initial load)
					loadTcs.TrySetResult(args.Result);
				}
				else if (navigatedCount == 2)
				{
					// Second navigation (reload)
					reloadTcs.TrySetResult(args.Result);
				}
			};

			await InvokeOnMainThreadAsync(async () =>
			{
				var handler = CreateHandler<WebViewHandler>(webView);
				
				// Wait for initial load to complete
				var loadResult = await loadTcs.Task.WaitAsync(TimeSpan.FromSeconds(10));
				Assert.Equal(WebNavigationResult.Success, loadResult);

				// Now test reload
				webView.Reload();

				// Wait for reload to complete
				var reloadResult = await reloadTcs.Task.WaitAsync(TimeSpan.FromSeconds(10));
				
				// This should succeed
				Assert.Equal(WebNavigationResult.Success, reloadResult);
			});
		}
	}
}