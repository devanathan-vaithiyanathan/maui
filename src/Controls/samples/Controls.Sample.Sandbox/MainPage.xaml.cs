
namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		Switch smwitch = new Switch();
		smwitch.Toggling += (s, e) =>
		{
			System.Diagnostics.Debug.WriteLine("e.Value = " + e.Value);
			//e.Cancel = true;
		};
		Content = smwitch;
	}
}