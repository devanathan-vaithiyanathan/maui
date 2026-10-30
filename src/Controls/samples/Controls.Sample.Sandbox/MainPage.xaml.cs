namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();

		// Set the initial position and bind the indicator view
		MyIndicator.SetBinding(IndicatorView.PositionProperty, new Binding("Position", source: MyCarousel));
		MyIndicator.SetBinding(IndicatorView.ItemsSourceProperty, new Binding("ItemsSource", source: MyCarousel));
	}
	private void OnIconTemplateClicked(object sender, EventArgs e)
	{
		MyIndicator.IndicatorTemplate = new DataTemplate(() =>
		{
			return new Label
			{
				Text = "\uf30c",
				FontFamily = "ionicons",
				FontSize = 12,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};
		});
	}

	private void OnNormalIndicatorClicked(object sender, EventArgs e)
	{
		MyIndicator.IndicatorTemplate = null;
	}
}
