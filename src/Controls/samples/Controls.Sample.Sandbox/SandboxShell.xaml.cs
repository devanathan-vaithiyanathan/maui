using Maui.Controls.Sample.PathDataSharedGeometryLeakRepro;

namespace Maui.Controls.Sample;

public partial class SandboxShell : Shell
{
	public const string PathDataLeakRoute = "path-data-leak-page";

	public SandboxShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(PathDataLeakRoute, typeof(PathDataLeakPage));
	}
}
