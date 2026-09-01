namespace Maui.Controls.Sample;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var page = new MasterPage
		{
			Detail = new NavigationPage(new DetailPage1())
		};

		return new Window(page);
	}
}
