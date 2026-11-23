namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void Button_Clicked(object? sender, EventArgs e)
	{
		searchBar.ShowCancelButton = !searchBar.ShowCancelButton;
	}
}