namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	readonly SearchBarViewModel viewModel;

	public MainPage()
	{
		InitializeComponent();
		BindingContext = viewModel = new SearchBarViewModel();
	}

	async void NavigateToOptionsPage_Clicked(object? sender, EventArgs e)
	{
		viewModel.Reset();
		await Navigation.PushAsync(new SearchBarOptionsPage(viewModel));
	}
}