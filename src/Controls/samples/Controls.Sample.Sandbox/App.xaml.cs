namespace Maui.Controls.Sample;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		// To test shell scenarios, change this to true
		bool useShell = false;

		if (!useShell)
		{
			// Use MainWindow for TitleBar demo
			return new MainWindow();
		}
		else
		{
			return new Window(new SandboxShell());
		}
	}
}
