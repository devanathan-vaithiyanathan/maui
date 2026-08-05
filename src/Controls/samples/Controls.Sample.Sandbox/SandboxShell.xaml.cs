namespace Maui.Controls.Sample;

public partial class SandboxShell : Shell
{
	public SandboxShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(ShellFooterReproductionPage), typeof(ShellFooterReproductionPage));
	}
}
