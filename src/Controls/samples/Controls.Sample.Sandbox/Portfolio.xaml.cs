namespace Maui.Controls.Sample;

public partial class Portfolio : ContentPage
{
    public Portfolio()
    {
        InitializeComponent();
    }

    private void ButtonProject1_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Project());
    }
}
