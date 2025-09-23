using System.Collections.ObjectModel;

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public ObservableCollection<string> TestCollection { get; }

	public MainPage()
	{
		TestCollection = new ObservableCollection<string>();

        for (int c = 0; c < 100; c++)
            TestCollection.Add($"Item index {c}");

        InitializeComponent();
	}
}