namespace Maui.Controls.Sample;

public partial class SearchBarOptionsPage : ContentPage
{
    readonly SearchBarViewModel viewModel;

    public SearchBarOptionsPage(SearchBarViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = this.viewModel = viewModel;
    }

    async void ApplyButton_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    void OnShadowRadioButtonCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            viewModel.Shadow = new Shadow
            {
                Brush = Colors.Violet,
                Radius = 10,
                Offset = new Point(0, 0),
                Opacity = 1f
            };
        }
    }
}