namespace Maui.Controls.Sample;

public sealed class BackHandlingContentPage : ContentPage
{
    public BackHandlingContentPage()
    {
        Title = "ContentPage override";
        Content = CreateScenarioContent("ContentPage", "ContentPageScenarioLabel");
    }

    protected override bool OnBackButtonPressed()
    {
        ShowBackHandledAlert("ContentPage");
        return true;
    }

    async void ShowBackHandledAlert(string pageType) =>
        await DisplayAlertAsync("Back handled", $"{pageType}.OnBackButtonPressed was called.", "OK");

    static View CreateScenarioContent(string pageType, string automationId)
    {
        var status = new Label
        {
            AutomationId = automationId,
            HorizontalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            Text = $"{pageType} scenario\n\nPress the Android back button. The app should remain open and show a {pageType} alert.",
            VerticalOptions = LayoutOptions.Center,
        };

        var returnButton = new Button
        {
            AutomationId = "ReturnToFlyoutPageButton",
            Text = "Return to scenario selector",
        };
        returnButton.Clicked += (_, _) => CurrentWindow().Page = CreateFlyoutPage();

        var grid = new Grid { Padding = new Thickness(24) };
        grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
        grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        grid.Children.Add(status);

        Grid.SetRow(returnButton, 1);
        grid.Children.Add(returnButton);
        return grid;
    }

    internal static Window CurrentWindow() =>
        Application.Current?.Windows.FirstOrDefault() ?? throw new InvalidOperationException("No active window.");

    internal static MasterPage CreateFlyoutPage() =>
        new()
        {
            Detail = new NavigationPage(new DetailPage1()),
        };

    internal static View CreateNavigationScenarioContent() =>
        CreateScenarioContent("NavigationPage", "NavigationPageScenarioLabel");
}

public sealed class BackHandlingNavigationPage : NavigationPage
{
    public BackHandlingNavigationPage()
        : base(new ContentPage
        {
            Title = "NavigationPage override",
            Content = BackHandlingContentPage.CreateNavigationScenarioContent(),
        })
    {
    }

    protected override bool OnBackButtonPressed()
    {
        ShowBackHandledAlert();
        return true;
    }

    async void ShowBackHandledAlert() =>
        await DisplayAlertAsync("Back handled", "NavigationPage.OnBackButtonPressed was called.", "OK");
}