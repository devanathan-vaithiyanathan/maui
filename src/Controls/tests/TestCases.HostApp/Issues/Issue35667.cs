namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 35667, "SearchHandler TextTransform does not update typed text", PlatformAffected.Android | PlatformAffected.iOS | PlatformAffected.macOS)]
public class Issue35667 : TestShell
{
    protected override void Init()
    {
        var contentPage = new ContentPage
        {
            Title = "SearchHandler TextTransform",
            Content = new Label
            {
                AutomationId = "Issue35667PageLoaded",
                Text = "Type lowercase text in the search field"
            }
        };

        SetSearchHandler(contentPage, new SearchHandler
        {
            Placeholder = "Search",
            TextTransform = TextTransform.Uppercase
        });

        Items.Add(new ShellContent
        {
            Title = "Home",
            Content = contentPage
        });
    }
}