namespace Maui.Controls.Sample;

public partial class SandboxShell : Shell
{
	public const string GeometryLeakRoute = "geometry-leak-page";

	public SandboxShell()
	{
		Routing.RegisterRoute(GeometryLeakRoute, typeof(GeometryLeakPage));
		InitializeComponent();
	}
}
