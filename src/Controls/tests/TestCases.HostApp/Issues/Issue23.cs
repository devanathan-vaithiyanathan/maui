namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 23, "Horizontal swipes not working when SwipeItems are set in all directions", PlatformAffected.Windows)]
public partial class Issue23 : ContentPage
{
    public Issue23()
    {
        Title = "Issue 23 - SwipeView All Directions";

        var layout = new StackLayout
        {
            Margin = new Thickness(20),
            Spacing = 20
        };

        // Create SwipeItems for all four directions
        var leftSwipeItem = new SwipeItem
        {
            BackgroundColor = Colors.Blue,
            Text = "Left",
            AutomationId = "LeftSwipeItem"
        };

        var rightSwipeItem = new SwipeItem
        {
            BackgroundColor = Colors.Red,
            Text = "Right",
            AutomationId = "RightSwipeItem"
        };

        var topSwipeItem = new SwipeItem
        {
            BackgroundColor = Colors.Green,
            Text = "Top",
            AutomationId = "TopSwipeItem"
        };

        var bottomSwipeItem = new SwipeItem
        {
            BackgroundColor = Colors.Orange,
            Text = "Bottom",
            AutomationId = "BottomSwipeItem"
        };

        // Add event handlers to track which swipe was triggered
        var resultLabel = new Label
        {
            AutomationId = "ResultLabel",
            Text = "No swipe detected",
            HorizontalOptions = LayoutOptions.Center,
            FontSize = 16
        };

        leftSwipeItem.Invoked += (s, e) => resultLabel.Text = "Left swipe triggered";
        rightSwipeItem.Invoked += (s, e) => resultLabel.Text = "Right swipe triggered";
        topSwipeItem.Invoked += (s, e) => resultLabel.Text = "Top swipe triggered";
        bottomSwipeItem.Invoked += (s, e) => resultLabel.Text = "Bottom swipe triggered";

        // Create SwipeView with items in all four directions
        var swipeView = new SwipeView
        {
            AutomationId = "SwipeViewAllDirections",
            HeightRequest = 120,
            WidthRequest = 300,
            HorizontalOptions = LayoutOptions.Center,
            LeftItems = new SwipeItems { leftSwipeItem },
            RightItems = new SwipeItems { rightSwipeItem },
            TopItems = new SwipeItems { topSwipeItem },
            BottomItems = new SwipeItems { bottomSwipeItem }
        };

        // Create content for the SwipeView
        var swipeContent = new Grid
        {
            BackgroundColor = Colors.LightGray
        };

        var contentLabel = new Label
        {
            Text = "Swipe in any direction\n(Left/Right should work with Top/Bottom)",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            TextColor = Colors.Black,
            HorizontalTextAlignment = TextAlignment.Center
        };

        swipeContent.Children.Add(contentLabel);
        swipeView.Content = swipeContent;

        // Instructions
        var instructionsLabel = new Label
        {
            Text = "Test: Swipe LEFT and RIGHT when all four directions are configured.\nBoth horizontal swipes should work correctly.",
            FontSize = 14,
            HorizontalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            Margin = new Thickness(0, 10)
        };

        layout.Children.Add(instructionsLabel);
        layout.Children.Add(swipeView);
        layout.Children.Add(resultLabel);

        Content = layout;
    }
}