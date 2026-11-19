namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 40370, "Resetting ProgressColor restores the platform default", PlatformAffected.Android | PlatformAffected.UWP)]
public class Issue40370 : ContentPage
{
	public Issue40370()
	{
		var defaultProgressBar = new ProgressBar
		{
			Progress = 0.75,
			AutomationId = "DefaultProgressBar"
		};

		var progressBarToReset = new ProgressBar
		{
			Progress = 0.75,
			AutomationId = "ProgressBarToReset"
		};

		var resetStatus = new Label
		{
			Text = "Ready",
			AutomationId = "ProgressColorResetStatus"
		};

		var resetButton = new Button
		{
			Text = "Set custom color, then reset",
			AutomationId = "ResetProgressColorButton"
		};

		resetButton.Clicked += (_, _) =>
		{
			progressBarToReset.ProgressColor = Colors.Red;
			progressBarToReset.ProgressColor = null;
			resetStatus.Text = "Reset complete";
		};

		Content = new VerticalStackLayout
		{
			Padding = 24,
			Spacing = 16,
			Children =
			{
				defaultProgressBar,
				progressBarToReset,
				resetButton,
				resetStatus
			}
		};
	}
}
