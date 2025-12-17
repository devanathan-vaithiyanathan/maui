namespace Maui.Controls.Sample;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        BindingContext = new MainWindowViewModel();
    }
}