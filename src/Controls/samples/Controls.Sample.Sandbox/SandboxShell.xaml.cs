namespace Maui.Controls.Sample;

public partial class SandboxShell : Shell
{
	readonly Button _toggleButton;

	public SandboxShell()
	{
		InitializeComponent();
		SetShellNavBarVisible(true);
	}

	public void SetShellNavBarVisible(bool isVisible)
	{
		Shell.SetNavBarIsVisible(this, isVisible);
	}

	public void ClearShellNavBarValue()
	{
		ClearValue(Shell.NavBarIsVisibleProperty);
	}
}
