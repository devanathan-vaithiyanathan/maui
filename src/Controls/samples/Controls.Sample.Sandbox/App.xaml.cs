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
		bool useShell = true;

		if (!useShell)
		{
			return new Window(new NavigationPage(new MainPage()));
		}
		else
		{
			var shell = new SandboxShell();
			var window = new Window(shell);
			window.Created += (_, _) => shell.Record("Window.Created");
			window.Activated += (_, _) => shell.Record("Window.Activated");
			return window;
		}
	}
}
