namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	private bool _isRtl = false;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnToggleClicked(object sender, EventArgs e)
	{
		_isRtl = !_isRtl;
		
		var newFlowDirection = _isRtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
		
		SearchBar1.FlowDirection = newFlowDirection;
		SearchBar2.FlowDirection = newFlowDirection;
		SearchBar3.FlowDirection = newFlowDirection;
		
		ToggleButton.Text = _isRtl ? "Toggle All to LTR" : "Toggle All to RTL";
		StatusLabel.Text = $"Current state: {(_isRtl ? "Right to Left" : "Left to Right")}";
	}
}