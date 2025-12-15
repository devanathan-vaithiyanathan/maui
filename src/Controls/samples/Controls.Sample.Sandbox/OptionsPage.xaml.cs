namespace Maui.Controls.Sample;

public partial class OptionsPage : ContentPage
{
	private readonly MainViewModel _viewModel;

	public OptionsPage(MainViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

	private void OnSetTrueClicked(object sender, EventArgs e)
	{
		_viewModel.IsRefreshing = true;
		Console.WriteLine("SANDBOX: Set IsRefreshing to TRUE");
	}

	private void OnSetFalseClicked(object sender, EventArgs e)
	{
		_viewModel.IsRefreshing = false;
		Console.WriteLine("SANDBOX: Set IsRefreshing to FALSE");
	}

	private async void OnBackClicked(object sender, EventArgs e)
	{
		Console.WriteLine("SANDBOX: Navigating back to MainPage");
		await Navigation.PopAsync();
	}
}
