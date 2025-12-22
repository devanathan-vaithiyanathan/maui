using System;
using Microsoft.Maui.Controls;

namespace Microsoft.Maui.Controls
{
	public partial class TitleBar
	{
		/// <summary>
		/// Default margin for TitleBar content on macOS to avoid overlapping with traffic light buttons
		/// </summary>
		private const int MacCatalystMargin = 80;

		partial void InitializePlatform()
		{
			SizeChanged += OnSizeChanged;
		}

		partial void CleanupPlatform()
		{
			SizeChanged -= OnSizeChanged;
		}

		void OnSizeChanged(object? sender, EventArgs e)
		{
			if (OperatingSystem.IsMacCatalystVersionAtLeast(16))
			{
				if (Window?.Handler?.PlatformView is UIKit.UIWindow uiWindow)
				{
					var windowScene = uiWindow.WindowScene;
					if (windowScene != null)
					{
						var fullScreen = windowScene.FullScreen;
						if (_templateRoot is Grid contentGrid)
						{
							// If in fullscreen, remove left margin, otherwise set margin to avoid traffic light overlap
							contentGrid.Margin = fullScreen ? new Thickness(0) : new Thickness(MacCatalystMargin, 0, 0, 0);
						}
					}
				}
			}
		}

		static partial void ConfigurePlatformTemplate(Grid contentGrid)
		{
			// Set default margin for macOS to avoid traffic light buttons
			contentGrid.Margin = new Thickness(MacCatalystMargin, 0, 0, 0);
		}
	}
}