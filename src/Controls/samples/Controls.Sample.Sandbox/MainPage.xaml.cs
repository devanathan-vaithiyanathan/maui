namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void Button_Clicked(object? sender, EventArgs e)
	{
		searchBar.ClearButtonVisibility = searchBar.ClearButtonVisibility == ClearButtonVisibility.Never
			? ClearButtonVisibility.WhileEditing
			: ClearButtonVisibility.Never;
	}
}