namespace Maui.Controls.Sample;

public partial class SandboxShell : Shell
{
	public SandboxShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(IndicationPage), typeof(IndicationPage));
		Routing.RegisterRoute(nameof(SampleTwoPage), typeof(SampleTwoPage));
		Routing.RegisterRoute(nameof(SampleThreePage), typeof(SampleThreePage));
	}
}
