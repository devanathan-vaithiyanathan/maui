namespace Maui.Controls.Sample;

public partial class SandboxShell : Shell
{
	readonly Button _toggleButton;

	public SandboxShell()
	{
		InitializeComponent();
		_toggleButton = new Button
		{
			Text = Shell.GetNavBarIsVisible(this) ? "Hide NavBar" : "Show NavBar",
			HeightRequest = 50,
			WidthRequest = 150,
			AutomationId = "NavBarToggleButton",
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};

		_toggleButton.Clicked += OnNavBarToggleButtonClicked;

		var issuePage = new ContentPage
		{
			Title = "Home",
			Content = new StackLayout
			{
				VerticalOptions = LayoutOptions.Center,
				Children =
				{
					new Label
					{
						Text = "Toggle NavBar visibility",
						HorizontalOptions = LayoutOptions.Center
					},
					_toggleButton
				}
			}
		};

		Shell.SetNavBarIsVisible(issuePage, false);

		Items.Add(new ShellContent
		{
			Title = "Issue17550",
			Content = issuePage
		});
	}

	void OnNavBarToggleButtonClicked(object? sender, EventArgs e)
	{
		bool isCurrentlyVisible = Shell.GetNavBarIsVisible(this);
		bool newVisibility = !isCurrentlyVisible;
		Shell.SetNavBarIsVisible(this, newVisibility);

		_toggleButton.Text = newVisibility ? "Hide NavBar" : "Show NavBar";
	}
}
