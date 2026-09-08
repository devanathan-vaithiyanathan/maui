using System;
using System.Threading.Tasks;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Platform;
using Xunit;
using static Microsoft.Maui.DeviceTests.AssertHelpers;

namespace Microsoft.Maui.DeviceTests
{
	public partial class GraphicsViewHandlerTests
	{
		[Fact(DisplayName = "Shadow Can Be Set At Runtime")]
		public async Task ShadowCanBeSetAtRuntime()
		{
			var graphicsView = new GraphicsViewStub();

			await AttachAndRun(graphicsView, async handler =>
			{
				await AssertEventually(() => handler.PlatformView.IsLoaded());
				Assert.False(handler.HasContainer);

				graphicsView.Shadow = new ShadowStub
				{
					Paint = new SolidPaintStub(Colors.Violet),
					Radius = 10,
					Offset = Point.Zero,
					Opacity = 1
				};
				handler.UpdateValue(nameof(IView.Shadow));

				await AssertEventually(() => handler.ContainerView is WrapperView { HasShadow: true });

				graphicsView.Shadow = null;
				handler.UpdateValue(nameof(IView.Shadow));

				Assert.False(handler.HasContainer);
			});
		}

		PlatformTouchGraphicsView GetPlatformGraphicsView(GraphicsViewHandler graphicsViewHandler) =>
			graphicsViewHandler.PlatformView;
	}
}