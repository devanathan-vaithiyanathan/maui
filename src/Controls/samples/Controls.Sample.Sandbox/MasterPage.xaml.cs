namespace Maui.Controls.Sample;

public partial class MasterPage : FlyoutPage
{
    public MasterPage()
    {
        InitializeComponent();
    }

    protected override bool OnBackButtonPressed()
    {
        ShowBackHandledAlert("FlyoutPage");
        return true;
    }

    void OnShowContentPageClicked(object sender, EventArgs e) =>
        Window.Page = new BackHandlingContentPage();

    void OnShowNavigationPageClicked(object sender, EventArgs e) =>
        Window.Page = new BackHandlingNavigationPage();

    void OnCloseFlyoutClicked(object sender, EventArgs e) => IsPresented = false;

    async void ShowBackHandledAlert(string pageType) =>
        await DisplayAlertAsync("Back handled", $"{pageType}.OnBackButtonPressed was called.", "OK");
}