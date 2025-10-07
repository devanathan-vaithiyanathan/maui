namespace Maui.Controls.Sample;

public partial class Archive : ContentPage
{
    public Archive()
    {
        InitializeComponent();
    }

    private void ButtonArchive1_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Project());
    }
}
