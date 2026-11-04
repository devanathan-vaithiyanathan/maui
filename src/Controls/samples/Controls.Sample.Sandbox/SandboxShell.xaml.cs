using System.Diagnostics;
using System.Reflection;

namespace Maui.Controls.Sample;

public partial class SandboxShell : Shell
{
	readonly Stopwatch _elapsed = Stopwatch.StartNew();

	public string NavigationStatus { get; private set; } = "Waiting for startup navigation.";
	public string EventLog { get; private set; } = string.Empty;
	public string ControlsVersion { get; } = "Loaded Controls: "
		+ typeof(Shell).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
		+ "\nReported regression: 10.0.120; comparison: 10.0.110. "
		+ "Sandbox uses local framework sources by default, not those NuGet versions.";

	public SandboxShell()
	{
		Navigating += (_, args) =>
			Record($"Shell.Navigating: {args.Source}, {args.Current?.Location} -> {args.Target.Location}");
		Navigated += (_, args) =>
			Record($"Shell.Navigated: {args.Source}, {args.Previous?.Location} -> {args.Current.Location}");
		InitializeComponent();
	}

	public void Record(string message)
	{
		var line = $"{_elapsed.Elapsed.TotalSeconds:F3}s {message}";
		Debug.WriteLine($"SANDBOX 39859: {line}");
		EventLog += line + Environment.NewLine;
		OnPropertyChanged(nameof(EventLog));
	}

	public void ReportNavigation(int count)
	{
		NavigationStatus = $"GettingStartedPage.NavigatedTo calls: {count}. "
			+ (count == 1 ? "No duplicate observed yet."
				: "Duplicate startup callback observed.");
		OnPropertyChanged(nameof(NavigationStatus));
	}

	void OnSplashNavigatedTo(object? sender, NavigatedToEventArgs args)
	{
		Record($"SplashPage.NavigatedTo: operation={args.NavigationType}, previous='{args.PreviousPage?.Title}'");
	}
}
