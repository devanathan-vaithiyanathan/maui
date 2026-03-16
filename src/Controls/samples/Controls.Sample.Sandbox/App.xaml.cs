namespace Maui.Controls.Sample;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		ShellPage shell = new() { Flyout = new ShellPageFlyout(), Detail = new NavigationPage(new MainPage()) };
		return new Window(shell);
	}
}
