using Microsoft.Maui.Controls.Shapes;

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 11404, "Line coordinates not computed correctly", PlatformAffected.All)]
public class Issue11404 : ContentPage
{
	public Issue11404()
	{
		// Create a Grid
            var grid = new Grid
            {
                WidthRequest = 200,
                HeightRequest = 200,
                BackgroundColor = Colors.LightGray
            };

            // First Line
            var redLine = new Line
            {
                X1 = 0,
                Y1 = 0,
                X2 = 100,
                Y2 = 100,
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = 10
            };

            // Second Line
            var redline = new Line
            {
                X1 = 200,
                Y1 = 0,
                X2 = 100,
                Y2 = 100,
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = 10
            };

            // Add lines to grid
            grid.Children.Add(redLine);
            grid.Children.Add(redline);
            
            var stackLayout = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = "This test checks if the lines are drawn correctly.",
                        AutomationId = "LineCoordinatesLabel"
                    }, 

                    grid
                }
            };

            // Set the grid as the content of the page
        Content = stackLayout;
	}
}