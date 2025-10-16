using System.Threading.Tasks;

namespace Maui.Controls.Sample;

public partial class SandboxShell : Shell
{
	public SandboxShell()
	{
		InitializeComponent();
	}

	protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);
        if (Current.CurrentPage.BindingContext is INavigationAware bindingContext)
        {
            bindingContext.OnShellNavigated(args);
        }
    }
}
