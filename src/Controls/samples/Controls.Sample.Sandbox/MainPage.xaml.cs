namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		//Shell.SetNavBarIsVisible(this, false);
		UpdateStatus();
	}

	private void OnCounterClicked(object? sender, EventArgs e)
	{
		UpdateStatus();
	}

	void OnShellTrueClicked(object? sender, EventArgs e)
	{
		if (FindShell() is SandboxShell shell)
			shell.SetShellNavBarVisible(true);

		UpdateStatus();
	}

	void OnShellFalseClicked(object? sender, EventArgs e)
	{
		if (FindShell() is SandboxShell shell)
			shell.SetShellNavBarVisible(false);

		UpdateStatus();
	}

	void OnShellClearClicked(object? sender, EventArgs e)
	{
		if (FindShell() is SandboxShell shell)
			shell.ClearShellNavBarValue();

		UpdateStatus();
	}

	void OnPageTrueClicked(object? sender, EventArgs e)
	{
		Shell.SetNavBarIsVisible(this, true);
		UpdateStatus();
	}

	void OnPageFalseClicked(object? sender, EventArgs e)
	{
		Shell.SetNavBarIsVisible(this, false);
		UpdateStatus();
	}

	void OnPageClearClicked(object? sender, EventArgs e)
	{
		ClearValue(Shell.NavBarIsVisibleProperty);
		UpdateStatus();
	}

	void UpdateStatus()
	{
		var shell = FindShell();
		var shellIsSet = shell?.IsSet(Shell.NavBarIsVisibleProperty) == true;
		var shellValue = shell is null ? "n/a" : Shell.GetNavBarIsVisible(shell).ToString();
		var pageIsSet = IsSet(Shell.NavBarIsVisibleProperty);
		var pageValue = Shell.GetNavBarIsVisible(this);

		StatusLabel.Text =
			$"Shell: {(shellIsSet ? $"set {shellValue}" : $"unset ({shellValue})")} | " +
			$"Page: {(pageIsSet ? $"set {pageValue}" : $"unset ({pageValue})")}";
	}

	Shell? FindShell()
	{
		Element? current = this;
		while (current is not null)
		{
			if (current is Shell shell)
				return shell;

			current = current.Parent;
		}

		return null;
	}
}
