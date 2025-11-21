namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	private RefreshViewModel _viewModel;

	public MainPage()
	{
		_viewModel = new RefreshViewModel();
		BindingContext = _viewModel;
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
	}

	private async void OnNavigateToControlPageClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new RefreshControlPage(_viewModel));
	}
}