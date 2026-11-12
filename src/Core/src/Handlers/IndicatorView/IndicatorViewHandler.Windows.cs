using System;

namespace Microsoft.Maui.Handlers
{
	public partial class IndicatorViewHandler : ViewHandler<IIndicatorView, MauiPageControl>
	{
		protected override MauiPageControl CreatePlatformView() => new MauiPageControl();

		protected override void ConnectHandler(MauiPageControl platformView)
		{
			base.ConnectHandler(platformView);

			platformView?.SetIndicatorView(VirtualView);
		}

		public static void MapCount(IIndicatorViewHandler handler, IIndicatorView indicator) => handler.PlatformView?.CreateIndicators();
		public static void MapPosition(IIndicatorViewHandler handler, IIndicatorView indicator) => handler.PlatformView?.UpdateIndicatorsColor();
		public static void MapHideSingle(IIndicatorViewHandler handler, IIndicatorView indicator) => handler.PlatformView?.CreateIndicators();
		public static void MapMaximumVisible(IIndicatorViewHandler handler, IIndicatorView indicator) => handler.PlatformView?.CreateIndicators();
		public static void MapIndicatorSize(IIndicatorViewHandler handler, IIndicatorView indicator) => handler.PlatformView?.CreateIndicators();
		public static void MapIndicatorColor(IIndicatorViewHandler handler, IIndicatorView indicator) => handler.PlatformView?.UpdateIndicatorsColor();
		public static void MapSelectedIndicatorColor(IIndicatorViewHandler handler, IIndicatorView indicator) => handler.PlatformView?.UpdateIndicatorsColor();
		public static void MapIndicatorShape(IIndicatorViewHandler handler, IIndicatorView indicator) => handler.PlatformView?.CreateIndicators();

		internal static void MapIndicatorsLayoutOverride(IIndicatorViewHandler handler, IIndicatorView indicator)
		{
			var layout = (indicator as ITemplatedIndicatorView)?.IndicatorsLayoutOverride;
			if (layout == null)
			{
				if (!handler.PlatformView.IsTemplateIndicator)
					return;

				handler.PlatformView.SetIndicatorTemplate(null, null);
				handler.PlatformView.UpdateIndicatorsColor();
				handler.UpdateValue(nameof(IIndicatorView.IndicatorsShape));
			}
			else
			{
				handler.PlatformView.SetIndicatorTemplate(layout, handler.MauiContext);
			}
		}
	}
}
