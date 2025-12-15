using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 30535, "[Windows] RefreshView IsRefreshing property not working while binding", PlatformAffected.Windows)]
public partial class Issue30535 : ContentPage
{
	public Issue30535()
	{
		InitializeComponent();
		BindingContext = new Issue30535ViewModel();
	}

	private async void OnNavigateToControlPageClicked(object sender, EventArgs e)
	{
		var viewModel = (Issue30535ViewModel)BindingContext;
		await Navigation.PushAsync(new Issue30535ControlPage(viewModel));
	}
}

public class Issue30535ViewModel : INotifyPropertyChanged
{
	private bool _isRefreshing = false;
	public bool IsRefreshing
	{
		get => _isRefreshing;
		set
		{
			if (_isRefreshing != value)
			{
				_isRefreshing = value;
				Console.WriteLine($"[Issue30535] IsRefreshing changed to: {value}");
				OnPropertyChanged();
			}
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}

public class Issue30535ControlPage : ContentPage
{
	private Issue30535ViewModel _viewModel;

	public Issue30535ControlPage(Issue30535ViewModel viewModel)
	{
		_viewModel = viewModel;
		BindingContext = _viewModel;

		var layout = new VerticalStackLayout
		{
			Spacing = 20,
			Padding = 20,
			VerticalOptions = LayoutOptions.Center
		};

		layout.Add(new Label
		{
			Text = "Current IsRefreshing: False",
			FontSize = 18,
			HorizontalOptions = LayoutOptions.Center,
			TextColor = Colors.Blue,
			AutomationId = "CurrentStatusLabel"
		});

		// Bind the label text to IsRefreshing
		var currentStatusLabel = layout.Children[0] as Label;
		currentStatusLabel.SetBinding(Label.TextProperty, new Binding("IsRefreshing", BindingMode.OneWay, null, null, "Current IsRefreshing: {0}"));

		var setTrueButton = new Button
		{
			Text = "Set IsRefreshing = True",
			BackgroundColor = Colors.Green,
			TextColor = Colors.White,
			AutomationId = "SetTrueButton"
		};
		setTrueButton.Clicked += (s, e) =>
		{
			Console.WriteLine("[Issue30535] Setting IsRefreshing to True via button");
			_viewModel.IsRefreshing = true;
		};
		layout.Add(setTrueButton);

		var setFalseButton = new Button
		{
			Text = "Set IsRefreshing = False",
			BackgroundColor = Colors.Red,
			TextColor = Colors.White,
			AutomationId = "SetFalseButton"
		};
		setFalseButton.Clicked += (s, e) =>
		{
			Console.WriteLine("[Issue30535] Setting IsRefreshing to False via button");
			_viewModel.IsRefreshing = false;
		};
		layout.Add(setFalseButton);

		var applyButton = new Button
		{
			Text = "Apply and Go Back",
			BackgroundColor = Colors.Blue,
			TextColor = Colors.White,
			AutomationId = "ApplyButton"
		};
		applyButton.Clicked += async (s, e) =>
		{
			Console.WriteLine($"[Issue30535] Navigating back with IsRefreshing={_viewModel.IsRefreshing}");
			await Navigation.PopAsync();
		};
		layout.Add(applyButton);

		Content = layout;

		ToolbarItems.Add(new ToolbarItem
		{
			Text = "Apply",
			AutomationId = "ApplyToolbar"
		});
		ToolbarItems[0].Clicked += async (s, e) =>
		{
			Console.WriteLine($"[Issue30535] Navigating back via toolbar with IsRefreshing={_viewModel.IsRefreshing}");
			await Navigation.PopAsync();
		};

		Title = "Refresh Control";
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		Console.WriteLine($"[Issue30535] ControlPage appearing with IsRefreshing={_viewModel.IsRefreshing}");
	}
}
