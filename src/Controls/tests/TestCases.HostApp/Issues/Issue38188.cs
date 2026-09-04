namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 38188, "Setting Shadow on SearchBar throws a COMException", PlatformAffected.UWP)]
public class Issue38188 : ContentPage
{
    public Issue38188()
    {
        var searchBar = new SearchBar
        {
            AutomationId = "Issue38188SearchBar",
            Placeholder = "Search"
        };

        var statusLabel = new Label
        {
            AutomationId = "Issue38188StatusLabel",
            Text = "Shadow not applied"
        };

        var applyShadowButton = new Button
        {
            AutomationId = "Issue38188ApplyShadowButton",
            Text = "Apply shadow"
        };

        applyShadowButton.Clicked += (sender, args) =>
        {
            searchBar.Shadow = new Shadow
            {
                Brush = Colors.Black,
                Offset = new Point(0, 4),
                Opacity = 0.5f,
                Radius = 10
            };

            statusLabel.Text = "Shadow applied";
        };

        Content = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 12,
            Children =
            {
                searchBar,
                applyShadowButton,
                statusLabel
            }
        };
    }
}