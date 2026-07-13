#if IOS
using UIKit;
#endif

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void OnFirstSpanTapped(object? sender, TappedEventArgs e)
	{
		DisplayAlertAsync("Alert", "First span tapped", "OK");
	}

	private void OnSecondSpanTapped(object? sender, TappedEventArgs e)
	{
		DisplayAlertAsync("Alert", "Second span tapped", "OK");
	}

	/// <summary>
	/// Logs SpanLabel's position in multiple coordinate systems.
	/// KEY: compare "SpanLabel origin in window" Y with the span hitbox Y values
	/// printed by [Span#36505] TK2 segment lines to verify they match the visual text.
	/// </summary>
	void LogLabelBounds()
	{
#if IOS
		if (SpanLabel.Handler?.PlatformView is not UIKit.UIView spanView) return;
		var window = spanView.Window;
		if (window == null) return;

		// Label origin/size in UIWindow coordinates (i.e., screen points)
		var labelInWindow = spanView.ConvertRectToView(spanView.Bounds, window);
		System.Diagnostics.Debug.WriteLine(
			$"[Span#36505] SpanLabel in WINDOW: x={labelInWindow.X:F1} y={labelInWindow.Y:F1} " +
			$"w={labelInWindow.Width:F1} h={labelInWindow.Height:F1}");

		// MAUI page view in UIWindow coordinates (tells us page-to-window offset)
		if (Handler?.PlatformView is UIKit.UIView pageView)
		{
			var pageInWindow = pageView.ConvertRectToView(pageView.Bounds, window);
			System.Diagnostics.Debug.WriteLine(
				$"[Span#36505] Page in WINDOW: x={pageInWindow.X:F1} y={pageInWindow.Y:F1}");

			// Label origin relative to the MAUI ContentPage (matches e.GetPosition(this))
			var labelInPage_X = labelInWindow.X - pageInWindow.X;
			var labelInPage_Y = labelInWindow.Y - pageInWindow.Y;
			System.Diagnostics.Debug.WriteLine(
				$"[Span#36505] SpanLabel origin in PAGE: x={labelInPage_X:F1} y={labelInPage_Y:F1}");
			System.Diagnostics.Debug.WriteLine(
				$"[Span#36505] *** To verify fix: tapY(page) - labelOriginY(page) = label-local tapY " +
				$"which should match a TK2 segment y value above ***");
		}
#endif
	}
}
