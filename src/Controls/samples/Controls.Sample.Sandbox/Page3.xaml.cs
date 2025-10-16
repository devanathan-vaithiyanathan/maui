namespace Maui.Controls.Sample;

public partial class Page3 : ContentPage
{
    public Page3()
    {
        InitializeComponent();
    }

	private void Button_Clicked(object sender, EventArgs e)
       {
        // Navigation.PushModalAsync(new ContentPage
        // {
        //     Content = new VerticalStackLayout
        //     {
        //         Children =
        //         {
        //             new Label { Text = "This is a modal page" },
        //             new Button
        //             {
        //                 Text = "Close",
        //                 Command = new Command(async () => await Navigation.PopModalAsync())
        //             }
        //         }
        //     }
        // });
    }
}
