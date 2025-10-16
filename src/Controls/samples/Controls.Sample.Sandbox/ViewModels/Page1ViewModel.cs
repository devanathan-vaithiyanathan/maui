using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Maui.Controls.Sample.ViewModels;

public class Page1ViewModel : INavigationAware
{
    private bool _wasModalShown;

    public Command NavigateForwardAsyncCommand => new(async () => await Shell.Current.GoToAsync(nameof(Page2)));
    public Command OpenModalAsyncCommand => new(async () => await OpenModalAsync());

    private async Task OpenModalAsync()
    {
        _wasModalShown = true;
        await Shell.Current.GoToAsync(nameof(ModalPage));
    }

    public void OnShellNavigated(ShellNavigatedEventArgs args)
    {
        if (_wasModalShown)
        {
            NavigateForwardAsyncCommand.Execute(null);
        }
    }
}