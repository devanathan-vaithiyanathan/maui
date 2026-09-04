using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.Controls.Sample;

public class SearchBarViewModel : INotifyPropertyChanged
{
    Shadow? shadow;

    public Shadow? Shadow
    {
        get => shadow;
        set
        {
            shadow = value;
            OnPropertyChanged();
        }
    }

    public void Reset()
    {
        Shadow = null;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}