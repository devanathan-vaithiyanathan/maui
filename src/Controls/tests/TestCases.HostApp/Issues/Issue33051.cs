namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 33051, "[iOS 26] SearchBar shows double clear buttons when text is entered", PlatformAffected.iOS)]
public class Issue33051 : ContentPage
{
    public Issue33051()
    {
        var searchBar = new SearchBar
        {
            Text = "Hello",
            Placeholder = "Search",
            AutomationId = "SearchBar33051"
        };

        Content = new VerticalStackLayout
        {
            Padding = new Thickness(20),
            Children =
            {
                searchBar
            }
        };
    }
}
