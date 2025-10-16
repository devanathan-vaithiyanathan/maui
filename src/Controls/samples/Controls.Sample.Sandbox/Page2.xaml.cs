namespace Maui.Controls.Sample;
using Maui.Controls.Sample.ViewModels;

public partial class Page2 : ContentPage
{
    public Page2()
    {
        InitializeComponent();
        BindingContext = new Page2ViewModel();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new ContentPage
        {
            Content = new VerticalStackLayout
            {
                Children =
                {
                    new Label { Text = "This is a modal page" },
                    new Button
                    {
                        Text = "Close",
                        Command = new Command(async () => await Navigation.PopModalAsync())
                    }
                }
            }
        });
    }

	
}
