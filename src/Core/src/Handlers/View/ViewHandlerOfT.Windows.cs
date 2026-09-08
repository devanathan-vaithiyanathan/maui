#nullable enable
using System;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Microsoft.Maui.Handlers
{
	public abstract partial class ViewHandler<TVirtualView, TPlatformView> : IPlatformViewHandler
	{
		public override void PlatformArrange(Rect rect) =>
			this.PlatformArrangeHandler(rect);

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint) =>
			this.GetDesiredSizeFromHandler(widthConstraint, heightConstraint);

		protected override void SetupContainer()
		{
			if (PlatformView is null || ContainerView is not null)
			{
				return;
			}

#pragma warning disable RS0030 // Do not use banned APIs; Panel.Children is banned for performance reasons. MauiPanel might not be used everywhere though.
			var oldParentChildren = PlatformView.Parent is MauiPanel mauiPanel
				? mauiPanel.CachedChildren
				: (PlatformView.Parent as Panel)?.Children;
#pragma warning restore RS0030 // Do not use banned APIs

			var oldIndex = oldParentChildren?.IndexOf(PlatformView);
			ContainerView = new WrapperView();

			if (oldIndex is int oldIdx && oldIdx >= 0)
			{
				oldParentChildren![oldIdx] = ContainerView;
			}

			((WrapperView)ContainerView).Child = PlatformView;
		}

		protected override void RemoveContainer()
		{
			if (PlatformView is null || ContainerView is null || PlatformView.Parent != ContainerView)
			{
				CleanupContainerView(ContainerView);
				ContainerView = null;
				return;
			}

#pragma warning disable RS0030 // Do not use banned APIs; Panel.Children is banned for performance reasons. MauiPanel might not be used everywhere though.
			var oldParentChildren = ContainerView.Parent is MauiPanel mauiPanel
				? mauiPanel.CachedChildren
				: (ContainerView.Parent as Panel)?.Children;
#pragma warning restore RS0030 // Do not use banned APIs

			var oldIndex = oldParentChildren?.IndexOf(ContainerView);
			((WrapperView)ContainerView).Child = null;

			if (oldIndex is int oldIdx && oldIdx >= 0)
				oldParentChildren![oldIdx] = PlatformView;

			CleanupContainerView(ContainerView);
			ContainerView = null;

			static void CleanupContainerView(FrameworkElement? containerView)
			{
				if (containerView is WrapperView wrapperView)
				{
					wrapperView.Child = null;
					wrapperView.Dispose();
				}
			}
		}
	}
}