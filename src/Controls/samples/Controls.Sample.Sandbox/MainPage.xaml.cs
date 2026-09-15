namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	async void OnNavigateClicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(IndicationPage));
	}
}