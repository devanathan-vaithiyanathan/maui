using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using UIKit;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	[Category(TestCategory.Shell)]
	public partial class ShellTests
	{
#if IOS
		[Fact(DisplayName = "Shell TabBar Background Color Preserves Liquid Glass")]
		public async Task ShellTabBarBackgroundColorPreservesLiquidGlass()
		{
			if (!OperatingSystem.IsIOSVersionAtLeast(26))
				return;

			var expectedColor = Colors.LightYellow;
			await RunShellTabBarTests(
				shell =>
				{
					Shell.SetBackgroundColor(shell, Colors.Red);
					Shell.SetTabBarBackgroundColor(shell, expectedColor);
				},
				shell =>
				{
					var tabBar = GetTabBar(shell.CurrentSection);

					Assert.Equal(expectedColor, tabBar.StandardAppearance.BackgroundColor.ToColor());
					Assert.Equal(0, tabBar.BackgroundColor.CGColor.Alpha);
					return Task.CompletedTask;
				});
		}

		[Fact(DisplayName = "Shell Background Color Applies To TabBar")]
		public async Task ShellBackgroundColorAppliesToTabBar()
		{
			if (!OperatingSystem.IsIOSVersionAtLeast(26))
				return;

			var expectedColor = Colors.LightYellow;
			await RunShellTabBarTests(
				shell => Shell.SetBackgroundColor(shell, expectedColor),
				shell =>
				{
					var tabBar = GetTabBar(shell.CurrentSection);

					Assert.Equal(expectedColor, tabBar.StandardAppearance.BackgroundColor.ToColor());
					Assert.Equal(expectedColor, tabBar.BackgroundColor.ToColor());
					return Task.CompletedTask;
				});
		}
#endif

		UITabBar GetTabBar(ShellSection item)
		{
			var shellItem = item.Parent as ShellItem;
			var shell = shellItem.Parent as Shell;

			var pagerParent = (shell.CurrentPage.Handler as IPlatformViewHandler)
				.PlatformView.FindParent(x => x.NextResponder is UITabBarController);

			// In macOS 15 Sequoia, the UITabBar is nested within the second subview (index 1) of the pagerParent. 
			if (OperatingSystem.IsMacCatalystVersionAtLeast(15, 0) || OperatingSystem.IsMacOSVersionAtLeast(15, 0))
			{
				var subview = pagerParent.Subviews.ElementAtOrDefault(1);

				if (subview?.Subviews is null)
					return null;

				return subview.Subviews.OfType<UITabBar>().FirstOrDefault();
			}

			return pagerParent.Subviews.OfType<UITabBar>().FirstOrDefault();
		}

		async Task ValidateTabBarIconColor(
			ShellSection item,
			Color iconColor,
			bool hasColor)
		{
			if (hasColor)
			{
				await AssertionExtensions.AssertTabItemIconContainsColor(GetTabBar(item),
					item.Title, iconColor, MauiContext);
			}
			else
			{
				await AssertionExtensions.AssertTabItemIconDoesNotContainColor(GetTabBar(item),
					item.Title, iconColor, MauiContext);
			}
		}

		async Task ValidateTabBarTextColor(
				ShellSection item,
				Color textColor,
				bool hasColor)
		{
			if (hasColor)
			{
				await AssertionExtensions.AssertTabItemTextContainsColor(GetTabBar(item),
					item.Title, textColor, MauiContext);
			}
			else
			{
				await AssertionExtensions.AssertTabItemTextDoesNotContainColor(GetTabBar(item),
					item.Title, textColor, MauiContext);
			}
		}
	}
}