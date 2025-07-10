#if MACCATALYST
using System;
using System.Diagnostics.CodeAnalysis;
using CoreGraphics;
using UIKit;
using System.Threading.Tasks;
using AppKit;

namespace Microsoft.Maui.Platform;

internal class WindowViewController : UIViewController
{
	WeakReference<IView?> _iTitleBarRef;
	bool _isTitleBarVisible = true;
	internal bool IsFirstLayout { get; set; }
	internal bool HasCustomTitleBar { get; set; }

   	[UnconditionalSuppressMessage("Memory", "MEM0002", Justification = "Proven safe in device test: 'TitleBar Does Not Leak'")]
	UIView? _titleBar;

	[UnconditionalSuppressMessage("Memory", "MEM0002", Justification = "Proven safe in device test: 'TitleBar Does Not Leak'")]
	UIView? _contentWrapperView;

	[UnconditionalSuppressMessage("Memory", "MEM0002", Justification = "Proven safe in device test: 'TitleBar Does Not Leak'")]
	internal NSLayoutConstraint? _contentWrapperTopConstraint;

	/// <summary>
	/// Instantiate a new <see cref="WindowViewController"/> object.
	/// </summary>
	/// <param name="contentViewController">An instance of the <see cref="UIViewController"/> that is the RootViewController.</param>
	/// <param name="window">An instance of the <see cref="IWindow"/>.</param>
	/// <param name="mauiContext">An instance of the <see cref="IMauiContext"/>.</param>
	/// <remarks>
	/// Only dragging the top of the titlebar will move the window.
	/// The top of the TitleBar will also drag the window inside of elements like buttons.
	/// Gestures such as swiping and controls like swipeview will not work inside the TitleBar.
	/// The TitleBar will not be accessible when a modal is being presented.
	/// </remarks>
	public WindowViewController(UIViewController contentViewController, IWindow window, IMauiContext mauiContext)
	{
		_iTitleBarRef = new WeakReference<IView?>(null);

		// Note: Maintain the order for adding a new ViewController to a Container ViewController
		// 1. Add the Subview
		// 2. Arrange the Subview's frame
		// 3. AddChildViewController
		// 4. Call DidMoveToParentViewController
		if (View is not null && contentViewController.View is not null)
		{
			_contentWrapperView = new UIView
			{
				TranslatesAutoresizingMaskIntoConstraints = false
			};
			View.AddSubview(_contentWrapperView);
			_contentWrapperView.AddSubview(contentViewController.View);
			_contentWrapperTopConstraint = _contentWrapperView.TopAnchor.ConstraintEqualTo(View.TopAnchor, 0);

			NSLayoutConstraint.ActivateConstraints(new[]
			{
				_contentWrapperView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor),
				_contentWrapperView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor),
				_contentWrapperTopConstraint,
				_contentWrapperView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor)
			});

		}

		SetUpTitleBar(window, mauiContext);

		AddChildViewController(contentViewController);
		contentViewController.DidMoveToParentViewController(this);
	}

	public override void ViewWillLayoutSubviews()
	{
		LayoutTitleBar();
		base.ViewWillLayoutSubviews();
	}

	public override void ViewDidLayoutSubviews()
	{
		UpdateContentWrapperContentFrame();
		base.ViewDidLayoutSubviews();
	}

	void UpdateContentWrapperContentFrame()
	{
		// At this point the _contentWrapperView bounds haven't been set
		// so we just use the windows bounds to set this value
		var frame = new CGRect(0, 0, View!.Bounds.Width, View!.Bounds.Height - (_contentWrapperTopConstraint?.Constant ?? 0));

		if (_contentWrapperView is not null && _contentWrapperView.Subviews[0].Frame != frame)
		{
			_contentWrapperView.Subviews[0].Frame = frame;
		}
	}

	/// <summary>
	/// Sets up the TitleBar in the ViewController.
	/// </summary>
	/// <param name="window">An instance of the <see cref="IWindow"/>.</param>
	/// <param name="mauiContext">An instance of the <see cref="IMauiContext"/>.</param>
	public void SetUpTitleBar(IWindow window, IMauiContext mauiContext)
	{
		var platformWindow = window.Handler?.PlatformView as UIWindow;

		if (platformWindow is null || View is null)
		{
			return;
		}

		var newTitleBar = window.TitleBar;
		var newPlatTitleBar = newTitleBar?.ToPlatform(mauiContext);
		HasCustomTitleBar = newPlatTitleBar is not null;

		IView? iTitleBar = null;
		_iTitleBarRef?.TryGetTarget(out iTitleBar);

		if (newTitleBar != iTitleBar)
		{
			_titleBar?.RemoveFromSuperview();
			iTitleBar?.DisconnectHandlers();
			iTitleBar = null;

			if (newPlatTitleBar is not null)
			{
				iTitleBar = newTitleBar;
				View.AddSubview(newPlatTitleBar);
			}

			_titleBar = newPlatTitleBar;
			_iTitleBarRef = new WeakReference<IView?>(iTitleBar);
		}
		
		_isTitleBarVisible = (iTitleBar?.Visibility == Visibility.Visible);

		var platformTitleBar = platformWindow.WindowScene?.Titlebar;

		if (newTitleBar is not null && platformTitleBar is not null)
		{
			platformTitleBar.TitleVisibility = UITitlebarTitleVisibility.Hidden;
		}

		else if (newTitleBar is null && platformTitleBar is not null)
		{
			platformTitleBar.TitleVisibility = UITitlebarTitleVisibility.Visible;
			// Clear any custom toolbar when reverting to system titlebar
			platformTitleBar.Toolbar = null;
		}

		IsFirstLayout = true;
		LayoutTitleBar();
	}

	/// <summary>
	/// Measures and arranges the TitleBar and adjusts the frame for the window content to make space for the TitleBar.
	/// </summary>
	public void LayoutTitleBar()
	{
		if (_contentWrapperTopConstraint is null || View is null)
		{
			return;
		}

		var current = _contentWrapperTopConstraint.Constant;
		_iTitleBarRef.TryGetTarget(out var iTitleBar);

		nfloat titleBarHeight = 0;

		if (_isTitleBarVisible && iTitleBar is not null)
		{
			var measured = iTitleBar.Measure(View.Bounds.Width, double.PositiveInfinity);
			iTitleBar.Arrange(new Graphics.Rect(0, 0, View.Bounds.Width, measured.Height));
			titleBarHeight = (nfloat)measured.Height;
		}

		_contentWrapperTopConstraint.Constant = titleBarHeight;

		// Configure toolbar based on titlebar height for proper traffic lights alignment
		ConfigureToolbarForTrafficLights(titleBarHeight);

		UpdateContentWrapperContentFrame();
	}

	/// <summary>
	/// Configures window-level properties to help with traffic lights positioning for custom titlebar.
	/// </summary>
	void ConfigureToolbarForTrafficLights(nfloat titleBarHeight)
	{
		var platformWindow = View?.Window;
		var platformTitleBar = platformWindow?.WindowScene?.Titlebar;
		
		// Only configure when we have a custom titlebar with measurable height
		if (platformTitleBar?.TitleVisibility == UITitlebarTitleVisibility.Hidden && titleBarHeight > 0)
		{
			// For MacCatalyst, the traffic lights positioning is largely controlled by the system
			// We can try to influence it by configuring the window's minimum size constraints
			// to hint at the titlebar area dimensions
			var windowScene = platformWindow?.WindowScene;
			if (windowScene?.SizeRestrictions != null)
			{
				// Get current window size
				var currentSize = platformWindow?.Bounds.Size ?? CGSize.Empty;
				
				// If we have a valid current size, ensure minimum height accounts for custom titlebar
				if (currentSize.Height > 0)
				{
					var minHeight = Math.Max(titleBarHeight + 100, currentSize.Height); // Ensure adequate space
					var currentMinSize = windowScene.SizeRestrictions.MinimumSize;
					
					// Only update if we're increasing the minimum constraints
					if (currentMinSize.Height < minHeight)
					{
						windowScene.SizeRestrictions.MinimumSize = new CGSize(
							Math.Max(currentMinSize.Width, 300), // Reasonable minimum width
							minHeight
						);
					}
				}
			}
			
			// Alternative approach: Use a minimal toolbar configuration that may help with layout
			// Some users report better results with an empty toolbar vs no toolbar
			if (platformTitleBar.Toolbar == null)
			{
				var toolbar = new NSToolbar("CustomTitleBarToolbar");
				toolbar.ShowsBaselineSeparator = false;
				platformTitleBar.Toolbar = toolbar;
			}
		}
		else if (platformTitleBar?.TitleVisibility == UITitlebarTitleVisibility.Visible)
		{
			// Clean up when reverting to system titlebar
			platformTitleBar.Toolbar = null;
		}
	}

	public void SetTitleBarVisibility(bool isVisible)
	{
		if (_contentWrapperTopConstraint is null || View is null)
			return;

		_isTitleBarVisible = isVisible;
		_iTitleBarRef.TryGetTarget(out var iTitleBar);
		iTitleBar?.InvalidateMeasure();
	}
}
#endif // MACCATALYST
