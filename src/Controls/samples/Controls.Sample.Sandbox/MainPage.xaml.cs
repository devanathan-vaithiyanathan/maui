namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	const string InitialValue = "Report.pdf";
	const string PromptTitle = "Rename";
	const string PromptMessage = "Type X and accept the prompt.";

	public MainPage()
	{
		InitializeComponent();
	}

	async void OnPromptClicked(object? sender, EventArgs e)
	{
		var scenario = (sender as Button)?.CommandParameter as string;
		var result = await (scenario switch
		{
			"Default" => DisplayPromptAsync(PromptTitle, PromptMessage, initialValue: InitialValue),
			"Null" => DisplayPromptAsync(PromptTitle, PromptMessage, "OK", "Cancel", null, -1, null, InitialValue, initialSelection: null),
			"None" => DisplayPromptAsync(PromptTitle, PromptMessage, "OK", "Cancel", null, -1, null, InitialValue, PromptTextSelection.None),
			"All" => DisplayPromptAsync(PromptTitle, PromptMessage, "OK", "Cancel", null, -1, null, InitialValue, PromptTextSelection.All),
			"Range" => ShowRangePrompt(InitialValue, new PromptTextSelection(0, 6)),
			"Caret" => ShowRangePrompt(InitialValue, new PromptTextSelection(6, 0)),
			"ClampedRange" => ShowRangePrompt(InitialValue, new PromptTextSelection(0, int.MaxValue)),
			"ClampedCaret" => ShowRangePrompt(InitialValue, new PromptTextSelection(int.MaxValue, 0)),
			"Empty" => DisplayPromptAsync(PromptTitle, PromptMessage, "OK", "Cancel", null, -1, null, "", PromptTextSelection.All),
			"Unicode" => ShowRangePrompt("A😀Report.pdf", new PromptTextSelection(3, 6)),
			"MaxLength" => DisplayPromptAsync(PromptTitle, "Select the displayed text and replace it with X.", "OK", "Cancel", null, 4, null, InitialValue, PromptTextSelection.All),
			_ => throw new InvalidOperationException($"Unknown prompt selection scenario: {scenario}")
		});

		ResultLabel.Text = result is null ? "Result: Canceled" : $"Result: \"{result}\"";
	}

	Task<string> ShowRangePrompt(string initialValue, PromptTextSelection selection) =>
		DisplayPromptAsync(PromptTitle, PromptMessage, "OK", "Cancel", null, -1, null, initialValue, selection);
}