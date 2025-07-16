namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		searchBar.SetAppThemeColor(SearchBar.PlaceholderColorProperty, Colors.Red, Colors.Green);
		this.SetAppThemeColor(BackgroundProperty, Colors.White, Colors.Black);
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		if (Application.Current != null)
		{
			Application.Current.UserAppTheme = Application.Current.UserAppTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;
		}
	}
}