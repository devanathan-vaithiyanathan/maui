namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	void OnCounterClicked(object sender, EventArgs e)
	{
		ShellPage.Instance!.Detail = new NavigationPage(new SecondPage());
	}
}