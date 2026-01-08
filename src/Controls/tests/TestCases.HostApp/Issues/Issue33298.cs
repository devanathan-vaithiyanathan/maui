using System.Collections.ObjectModel;
using Microsoft.Maui.Controls.Shapes;

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 33298, "[WinUI]Editor cursor renders as “dot” after clearing text when parent Border.StrokeThickness is changed dynamically", PlatformAffected.UWP)]

public class Issue33298 : ContentPage
{
	Issue33298CustomEditorView chatEditorView;

	public Issue33298()
	{
		// Root Grid
		var grid = new Grid
		{
			RowDefinitions =
			{
				new RowDefinition(GridLength.Auto),
				new RowDefinition(GridLength.Auto),
				new RowDefinition(GridLength.Auto)
			}
		};

		// Border (typing area)
		var typingAreaView = new Border
		{
			StrokeThickness = 1,
			Padding = new Thickness(16, 0, 8, 0),
			Stroke = Colors.Blue,
			StrokeShape = new RoundRectangle
			{
				CornerRadius = new CornerRadius(42)
			}
		};

		// Custom Editor
		chatEditorView = new Issue33298CustomEditorView
		{
			BackgroundColor = Colors.Transparent,
			Placeholder = "Type TextMessage..",
			VerticalTextAlignment = TextAlignment.Center,
			VerticalOptions = LayoutOptions.Center,
			AutoSize = EditorAutoSizeOption.TextChanges,
			CharacterSpacing = 0.25
		};

		typingAreaView.Content = chatEditorView;

		Grid.SetRow(typingAreaView, 0);
		grid.Children.Add(typingAreaView);

		var addText = new Button
		{
			Text = "Add Text",
			AutomationId = "AddText"
		};
		Grid.SetRow(addText, 2);
		grid.Children.Add(addText);
		addText.Clicked += (s, e) =>
		{
			chatEditorView.Text = "Some Text ";
		};

		// Button
		var button = new Button
		{
			Text = "Empty Text",
			AutomationId = "EmptyText"
		};
		button.Clicked += Button_Clicked;

		Grid.SetRow(button, 1);
		grid.Children.Add(button);

		Content = grid;
	}

	void Button_Clicked(object sender, EventArgs e)
	{
		chatEditorView.Text = string.Empty;
	}
}

public class Issue33298CustomEditorView : Editor
{
	public Issue33298CustomEditorView()
	{
		TextChanged += CustomEditorView_TextChanged;
	}

	void CustomEditorView_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (Parent is Border border)
		{
			border.StrokeThickness =
				!string.IsNullOrWhiteSpace(Text) ? 2 : 1;
		}
	}
}