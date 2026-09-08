using System;
using System.Threading.Tasks;
using Microsoft.Maui.DeviceTests.Stubs;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	public partial class GraphicsViewHandlerTests
	{
		[Fact(DisplayName = "Shadow Can Be Set At Runtime")]
		public async Task ShadowCanBeSetAtRuntime()
		{
			var graphicsView = new GraphicsViewStub();

			await AttachAndRun(graphicsView, (handler) =>
			{
				Assert.False(handler.HasContainer);

				graphicsView.Shadow = new ShadowStub();
				handler.UpdateValue(nameof(IView.Shadow));

				Assert.True(handler.HasContainer);

				graphicsView.Shadow = null;
				handler.UpdateValue(nameof(IView.Shadow));

				Assert.False(handler.HasContainer);
			});
		}

		PlatformTouchGraphicsView GetPlatformGraphicsView(GraphicsViewHandler graphicsViewHandler) =>
			graphicsViewHandler.PlatformView;
	}
}