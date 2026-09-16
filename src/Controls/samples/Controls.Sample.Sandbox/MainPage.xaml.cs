namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	async void OnSample1Clicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(IndicationPage));
	}

	async void OnSample2Clicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(SampleTwoPage));
	}

	async void OnSample3Clicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(SampleThreePage));
	}
}