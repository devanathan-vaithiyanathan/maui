namespace Maui.Controls.Sample;

public partial class SandboxShell : Shell
{
	public SandboxShell()
	{
		InitializeComponent();
	}

	private void ToolbarItem_Clicked(object sender, EventArgs e)
	{
		flyoutItems.IsEnabled = !flyoutItems.IsEnabled;
	}
}
