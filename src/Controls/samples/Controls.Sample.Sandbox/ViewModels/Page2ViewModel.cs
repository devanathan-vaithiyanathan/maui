using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Maui.Controls.Sample.ViewModels;

public class Page2ViewModel
{
    public Command NavigateForwardAsyncCommand => new(async () => await Shell.Current.GoToAsync(nameof(Page3)));
}
