namespace Maui.Controls.Sample;

public partial class SecondPage : ContentPage
{
	public SecondPage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		await Task.Delay(5000);

		await DisplayAlertAsync("Title", "Message", "OK");
	}

	void OnCounterClicked(object sender, EventArgs e)
	{
		ShellPage.Instance!.Detail = new NavigationPage(new MainPage());
	}
}
