using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Maui.Controls.Sample.ViewModels;

public class ModalPageViewModel
{
    public Command CloseModalAsyncCommand => new(async () => await Shell.Current.GoToAsync(".."));
}