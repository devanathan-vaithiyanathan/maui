namespace Maui.Controls.Sample;

public partial class DetailPage1 : ContentPage
{
    public DetailPage1()
    {
        InitializeComponent();
    }

    void OnOpenFlyoutClicked(object sender, EventArgs e)
    {
        if (Parent is NavigationPage { Parent: MasterPage flyoutPage })
            flyoutPage.IsPresented = true;
    }
}