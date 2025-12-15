using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	private readonly MainViewModel _viewModel;

	public MainPage()
	{
		_viewModel = new MainViewModel();
		BindingContext = _viewModel;
		InitializeComponent();
	}

	private async void OnNavigateToOptionsClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new OptionsPage(_viewModel));
	}
}

public class MainViewModel : INotifyPropertyChanged
{
	private bool _isRefreshing;

	public bool IsRefreshing
	{
		get => _isRefreshing;
		set
		{
			if (_isRefreshing != value)
			{
				_isRefreshing = value;
				OnPropertyChanged();
				Console.WriteLine($"ViewModel IsRefreshing changed to: {value}");
			}
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}