namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	int _navigatedToCount;
	bool _gettingStartedCompleted;

	public MainPage()
	{
		InitializeComponent();
		Title = "Getting started";
		NavigatedTo += OnPageNavigatedTo;
	}

	async void OnPageNavigatedTo(object? sender, NavigatedToEventArgs args)
	{
		var shell = (SandboxShell)Shell.Current;
		_navigatedToCount++;
		shell.Record($"Enter GettingStartedPage.NavigatedTo #{_navigatedToCount}, "
			+ $"operation={args.NavigationType}, previous='{args.PreviousPage?.Title}', "
			+ $"completed={_gettingStartedCompleted}");
		shell.ReportNavigation(_navigatedToCount);

		// Record duplicates, but don't mask the first redirect with another navigation.
		if (_navigatedToCount > 1)
			return;

		shell.Record("GettingStartedPage: calling GoToAsync(\"//SplashPage\")");
		await shell.GoToAsync("//SplashPage");
		_gettingStartedCompleted = true;
		shell.Record("Leave GettingStartedPage.NavigatedTo; completed=True");
	}
}