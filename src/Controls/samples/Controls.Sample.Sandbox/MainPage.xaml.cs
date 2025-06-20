namespace Maui.Controls.Sample;

public partial class MainPage : Window
{
	public MainPage()
	{
		InitializeComponent();
		var viewModel = new TitleBarViewModel();
        BindingContext = viewModel;

        Page = new NavigationPage(new TitleBarOptionsPage(viewModel));
	}
}