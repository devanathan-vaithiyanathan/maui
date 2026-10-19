namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		indicatorView.ItemsSource = new[]
	{
		"Item1",
		"Item2",
		"Item3",
		"Item4",
		"Item5"
	};

		indicatorView.Position = 1;
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		if (indicatorView != null)
			indicatorView.FlowDirection = indicatorView.FlowDirection == FlowDirection.LeftToRight ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
	}

	private void Button_Clicked1(object sender, EventArgs e)
	{
	}
}