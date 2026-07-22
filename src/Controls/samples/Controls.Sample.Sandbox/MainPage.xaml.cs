namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	const string InitialValue = "1234234234";

	public MainPage()
	{
		InitializeComponent();
	}

	// Old behaviour: cursor is placed at the end; typing "2" results in "12".
	async void OnNoPreselectClicked(object sender, EventArgs e)
	{
		var result = await DisplayPromptAsync(
			"Copies",
			"Enter number of copies:",
			initialValue: InitialValue,
			preselectInitialValue: false);

		ResultLabel.Text = result is null ? "Cancelled" : $"Result (no preselect): {result}";
	}

	// New behaviour: initial value is pre-selected; typing "2" replaces it with "2".
	async void OnWithPreselectClicked(object sender, EventArgs e)
	{
		var result = await DisplayPromptAsync(
			"Copies",
			"Enter number of copies:",
			initialValue: InitialValue,
			preselectInitialValue: true);

		ResultLabel.Text = result is null ? "Cancelled" : $"Result (with preselect): {result}";
	}
}