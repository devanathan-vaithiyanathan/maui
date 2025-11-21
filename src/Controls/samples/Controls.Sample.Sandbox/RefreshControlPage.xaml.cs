namespace Maui.Controls.Sample;

public partial class RefreshControlPage : ContentPage
{
	private RefreshViewModel _viewModel;

	public RefreshControlPage(RefreshViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
	}

	private void OnSetTrueClicked(object sender, EventArgs e)
	{
		_viewModel.IsRefreshing = true;
	}

	private void OnSetFalseClicked(object sender, EventArgs e)
	{
		_viewModel.IsRefreshing = false;
	}

	private async void OnBackClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}