namespace Maui.Controls.Sample;

public partial class ShellPage : FlyoutPage
{
	public ShellPage()
	{
		InitializeComponent();
		Instance = this;
	}

	public static ShellPage? Instance { get; private set; }
}
