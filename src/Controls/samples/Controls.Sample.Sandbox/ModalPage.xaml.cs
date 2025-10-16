namespace Maui.Controls.Sample;
using Maui.Controls.Sample.ViewModels;

public partial class ModalPage : ContentPage
{
    public ModalPage()
    {
        InitializeComponent();
        BindingContext = new ModalPageViewModel();
    }
}
