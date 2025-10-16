namespace Maui.Controls.Sample;
using Maui.Controls.Sample.ViewModels;

public partial class Page1 : ContentPage
{
    public Page1()
    {
        InitializeComponent();
        BindingContext = new Page1ViewModel();
    }

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();
	}
}
