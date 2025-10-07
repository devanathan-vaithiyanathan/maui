namespace Maui.Controls.Sample;

public partial class MainFlyoutFlyout : ContentPage
{
    public MainFlyoutFlyout()
    {
        InitializeComponent();
    }

    private void ButtonPortfolio_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var mainFlyout = button?.Parent?.Parent?.Parent as FlyoutPage;
        if (mainFlyout != null)
        {
            mainFlyout.Detail = new NavigationPage(new Portfolio());
            mainFlyout.IsPresented = false;
        }
    }

    private void ButtonArchive_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var mainFlyout = button?.Parent?.Parent?.Parent as FlyoutPage;
        if (mainFlyout != null)
        {
            mainFlyout.Detail = new NavigationPage(new Archive());
            mainFlyout.IsPresented = false;
        }
    }
}
