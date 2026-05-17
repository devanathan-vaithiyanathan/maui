using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Controls.Xaml;

namespace Maui.Controls.Sample.Issues
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[Issue(IssueTracker.Github, 25, "Button and Border Controls Remain in Pressed State on Windows Touch Screens", 
		PlatformAffected.UWP)]
	public partial class Issue25 : ContentPage
	{
		public Issue25()
		{
			InitializeComponent();
		}

		private void OnButtonPressed(object sender, System.EventArgs e)
		{
			if (sender is Button button)
			{
				StatusLabel.Text = $"{button.Text} pressed at {System.DateTime.Now:HH:mm:ss}";
				StatusLabel.TextColor = Colors.Red;
			}
		}

		private void OnButtonReleased(object sender, System.EventArgs e)
		{
			if (sender is Button button)
			{
				StatusLabel.Text = $"{button.Text} released at {System.DateTime.Now:HH:mm:ss}";
				StatusLabel.TextColor = Colors.Green;
			}
		}

		private void OnButtonClicked(object sender, System.EventArgs e)
		{
			if (sender is Button button)
			{
				StatusLabel.Text = $"{button.Text} clicked at {System.DateTime.Now:HH:mm:ss}";
				StatusLabel.TextColor = Colors.Blue;
			}
		}

		private void OnBorderTapped(object sender, System.EventArgs e)
		{
			StatusLabel.Text = $"Border tapped at {System.DateTime.Now:HH:mm:ss}";
			StatusLabel.TextColor = Colors.Purple;
		}
	}
}