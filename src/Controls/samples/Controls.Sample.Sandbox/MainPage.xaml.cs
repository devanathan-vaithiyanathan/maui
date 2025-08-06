using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.Controls.Sample;
#nullable enable
public partial class MainPage : ContentPage
{
    private ScrollViewViewModel _viewModel;

    public MainPage(ScrollViewViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void NavigateToOptionsPage_Clicked(object sender, EventArgs e)
    {
        BindingContext = _viewModel = new ScrollViewViewModel();
        _viewModel.ScrollX = 0;
        _viewModel.ScrollY = 0;
        _viewModel.ContentSize = new Size(0, 0);
        Dispatcher.Dispatch(async () =>
        {
            if (_viewModel.Content != null)
            {
                await MyScrollView.ScrollToAsync(_viewModel.Content, ScrollToPosition.MakeVisible, false);
            }
        });
        await Navigation.PushAsync(new ScrollViewOptionsPage(_viewModel));
    }

    private void OnScrollViewScrolled(object sender, ScrolledEventArgs e)
    {
        if (BindingContext is ScrollViewViewModel vm)
        {
            vm.ScrollX = e.ScrollX;
            vm.ScrollY = e.ScrollY;

        }
    }

    private void ScrollViewContent_SizeChanged(object sender, EventArgs e)
    {
        if (BindingContext is ScrollViewViewModel viewModel && sender is View content)
            viewModel.ContentSize = new Size(content.Width, content.Height);
    }


    private async void OnScrollToPositionButtonClicked(object sender, EventArgs e)
    {
        if (BindingContext is ScrollViewViewModel vm && sender is Button btn)
        {
            ScrollToPosition position = ScrollToPosition.MakeVisible;
            switch (btn.Text.ToLowerInvariant())
            {
                case "start":
                    position = ScrollToPosition.Start;
                    break;
                case "center":
                    position = ScrollToPosition.Center;
                    break;
                case "end":
                    position = ScrollToPosition.End;
                    break;
                case "visible":
                    position = ScrollToPosition.MakeVisible;
                    break;
            }
            if (vm.Content != null)
                await MyScrollView.ScrollToAsync(vm.Content, position, true);
        }
    }

}

public class ScrollViewViewModel : INotifyPropertyChanged
{
	private bool _isVisible = true;
	private bool _isEnabled = true;
	private FlowDirection _flowDirection = FlowDirection.LeftToRight;
	private ScrollBarVisibility _horizontalScrollBarVisibility = ScrollBarVisibility.Never;
	private ScrollBarVisibility _verticalScrollBarVisibility = ScrollBarVisibility.Never;
	private Size _contentSize;
	private string _contentText;
	public string ContentSizeString => $"{ContentSize.Width:F0} x {ContentSize.Height:F0}";
	private double _scrollX;
	private double _scrollY;
	public int ScrollXInt => (int)Math.Round(ScrollX);
	public int ScrollYInt => (int)Math.Round(ScrollY);
	private View _content;
	private ScrollOrientation _orientation = ScrollOrientation.Vertical;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	public ScrollViewViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	{
		Content = new Label
		{
			Text = string.Join(Environment.NewLine, Enumerable.Range(1, 60).Select(i => $"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed euismod, urna eu tincidunt consectetur, nisi nisl aliquam enim, eget facilisis enim nisl nec elit . Sed euismod, urna eu tincidunt consectetur, nisi nisl aliquam enim Eget facilisis enim nisl nec elit Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae. Nullam ac erat at dui laoreet aliquet. Praesent euismod, justo at dictum facilisis, urna erat dictum enim.{i}")),
			FontSize = 18,
			Padding = 10
		};
	}
	public bool IsVisible
	{
		get => _isVisible;
		set { if (_isVisible != value) { _isVisible = value; OnPropertyChanged(); } }
	}

	public bool IsEnabled
	{
		get => _isEnabled;
		set { if (_isEnabled != value) { _isEnabled = value; OnPropertyChanged(); } }
	}

	public FlowDirection FlowDirection
	{
		get => _flowDirection;
		set { if (_flowDirection != value) { _flowDirection = value; OnPropertyChanged(); } }
	}

	public ScrollBarVisibility HorizontalScrollBarVisibility
	{
		get => _horizontalScrollBarVisibility;
		set { if (_horizontalScrollBarVisibility != value) { _horizontalScrollBarVisibility = value; OnPropertyChanged(); } }
	}

	public ScrollBarVisibility VerticalScrollBarVisibility
	{
		get => _verticalScrollBarVisibility;
		set { if (_verticalScrollBarVisibility != value) { _verticalScrollBarVisibility = value; OnPropertyChanged(); } }
	}

	public string ContentText
	{
		get => _contentText;
		set
		{
			if (_contentText != value)
			{
				_contentText = value;
				Content = new Label { Text = _contentText }; // Update Content when ContentText changes
				OnPropertyChanged();
			}
		}
	}

	public View Content
	{
		get => _content;
		set
		{
			if (_content != value)
			{
				_content = value;
				OnPropertyChanged();
			}
		}
	}

	public Size ContentSize
	{
		get => _contentSize;
		set
		{
			if (_contentSize != value)
			{
				_contentSize = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(ContentSizeString));
			}
		}
	}

	public double ScrollX
	{
		get => _scrollX;
		set
		{
			if (_scrollX != value)
			{
				_scrollX = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(ScrollXInt));
			}
		}
	}

	public double ScrollY
	{
		get => _scrollY;
		set
		{
			if (_scrollY != value)
			{
				_scrollY = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(ScrollYInt));
			}
		}
	}
	private ScrollToPosition _selectedScrollToPosition = ScrollToPosition.MakeVisible;
	public ScrollToPosition SelectedScrollToPosition
	{
		get => _selectedScrollToPosition;
		set { if (_selectedScrollToPosition != value) { _selectedScrollToPosition = value; OnPropertyChanged(); } }
	}

	public ScrollOrientation Orientation
	{
		get => _orientation;
		set { if (_orientation != value) { _orientation = value; OnPropertyChanged(); } }
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
#nullable restore
