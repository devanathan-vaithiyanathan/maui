using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.Controls.Sample;

public partial class IndicationPage : ContentPage, IQueryAttributable, INotifyPropertyChanged
{
    string? _selectedBoxColor;

    public IndicationPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public ObservableCollection<string> BoxColors { get; } =
    [
        "#113FFC", "#00A1CD", "#7c0e64", "#d2a023", "#ca1765", "#14e147"
    ];

    public string? SelectedBoxColor
    {
        get => _selectedBoxColor;
        set
        {
            if (_selectedBoxColor == value)
                return;

            _selectedBoxColor = value;
            OnPropertyChanged();
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        SelectedBoxColor = BoxColors[1];
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    protected new void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}