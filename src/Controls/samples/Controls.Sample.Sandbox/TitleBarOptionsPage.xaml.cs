using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample;

public partial class TitleBarOptionsPage : ContentPage
{
    private TitleBarViewModel _viewModel;

    public TitleBarOptionsPage(TitleBarViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private void OnResetChangesClicked(object sender, EventArgs e)
    {
        BindingContext = _viewModel = new TitleBarViewModel();
    }

    private void OnFlowDirectionCheckBoxChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            _viewModel.FlowDirection = FlowDirection.RightToLeft;
        }
        else
        {
            _viewModel.FlowDirection = FlowDirection.LeftToRight;
        }
    }
}

public class TitleBarViewModel : INotifyPropertyChanged
    {
        private string _title = "My MAUI App";
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _subtitle = "Demo TitleBar Integration";
        public string Subtitle
        {
            get => _subtitle;
            set
            {
                if (_subtitle != value)
                {
                    _subtitle = value;
                    OnPropertyChanged();
                }
            }
        }
        private Color _color = Colors.Red;
        public Color Color
        {
            get => _color;
            set { _color = value; OnPropertyChanged(); }
        }


        private bool _isRedChecked = true;
        public bool IsRedChecked
        {
            get => _isRedChecked;
            set
            {
                if (_isRedChecked != value)
                {
                    _isRedChecked = value;
                    if (value)
                        Color = Colors.Red;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isOrangeChecked = false;
        public bool IsOrangeChecked
        {
            get => _isOrangeChecked;
            set
            {
                if (_isOrangeChecked != value)
                {
                    _isOrangeChecked = value;
                    if (value)
                        Color = Colors.Orange;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    OnPropertyChanged();
                }
            }
        }
        private FlowDirection _flowDirection = FlowDirection.LeftToRight;
        public FlowDirection FlowDirection
        {
            get => _flowDirection;
            set
            {
                if (_flowDirection != value)
                {
                    _flowDirection = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

