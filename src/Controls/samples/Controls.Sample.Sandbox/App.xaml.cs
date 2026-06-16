using Maui.Controls.Sample.PathDataSharedGeometryLeakRepro;
using Microsoft.Maui.Controls.Shapes;

namespace Maui.Controls.Sample;

public partial class App : Application
{
	public const string SharedPathGeometryResourceKey = "SharedPathGeometry";
	public const string SharedScaleTransformResourceKey = "SharedScaleTransform";

	public App()
	{
		InitializeComponent();
		
		// Initialize shared resources
		Resources[SharedPathGeometryResourceKey] = PathDataCardFactory.CreateSharedGeometry();
		Resources[SharedScaleTransformResourceKey] = new ScaleTransform(1, 1, 12, 12);
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		// To test the PathData leak scenario, keep useShell = true
		bool useShell = true;

		if (!useShell)
		{
			return new Window(new NavigationPage(new MainPage()));
		}
		else
		{
			return new Window(new SandboxShell());
		}
	}
}
