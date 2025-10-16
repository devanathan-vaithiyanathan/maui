namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}
	private void Button_OnClicked(object? sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(Page1));
    }
}