namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public List<Item> Items { get; set; }
	public MainPage()
	{
		Items =
		[
			new Item { Name = "Item 1" },
			new Item { Name = "Item 2" },
			new Item { Name = "Item 3" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
			new Item { Name = "Item 4" },
		];

		BindingContext = this;

		InitializeComponent();
		collectionView.SelectedItem = Items[3];

	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		collectionView.IsEnabled = !collectionView.IsEnabled;
	}
}


public class Item
{
	public string? Name { get; set; }
}