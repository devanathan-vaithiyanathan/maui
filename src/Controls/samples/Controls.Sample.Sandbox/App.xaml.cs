namespace Maui.Controls.Sample;

public partial class App : Application
{
	ScrollViewViewModel _viewModel;
	public App()
	{
		InitializeComponent();
		_viewModel = new ScrollViewViewModel();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		// To test shell scenarios, change this to true
		bool useShell = false;

		if (!useShell)
		{
			return new Window(new NavigationPage(new MainPage(_viewModel)));
		}
		else
		{
			return new Window(new SandboxShell());
		}
	}
}
