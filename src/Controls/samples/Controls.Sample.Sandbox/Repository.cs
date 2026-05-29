using System.Collections.ObjectModel;

namespace Maui.Controls.Sample;

public class Repository
{
	public ObservableCollection<Order> OrderCollection { get; set; }

	public Repository()
	{
		OrderCollection = new ObservableCollection<Order>();
		GenerateOrders();
	}

	void GenerateOrders()
	{
		OrderCollection.Add(new Order("1001", "Maria Anders", "Germany", "ALFKI", "Berlin"));
		OrderCollection.Add(new Order("1002", "Ana Trujillo", "Mexico", "ANATR", "Mexico D.F."));
		OrderCollection.Add(new Order("1003", "Ant Fuller", "Mexico", "ANTON", "Mexico D.F."));
		OrderCollection.Add(new Order("1004", "Thomas Hardy", "UK", "AROUT", "London"));
		OrderCollection.Add(new Order("1005", "Tim Adams", "Sweden", "BERGS", "London"));
		OrderCollection.Add(new Order("1006", "Hanna Moos", "Germany", "BLAUS", "Mannheim"));
		OrderCollection.Add(new Order("1007", "Andrew Fuller", "France", "BLONP", "Strasbourg"));
		OrderCollection.Add(new Order("1008", "Martin King", "Spain", "BOLID", "Madrid"));
		OrderCollection.Add(new Order("1009", "Lenny Lin", "France", "BONAP", "Marsiella"));
		OrderCollection.Add(new Order("1010", "John Carter", "Canada", "BOTTM", "Lenny Lin"));
		OrderCollection.Add(new Order("1011", "Laura King", "UK", "AROUT", "London"));
		OrderCollection.Add(new Order("1012", "Anne Wilson", "Germany", "BLAUS", "Mannheim"));
		OrderCollection.Add(new Order("1013", "Martin King", "France", "BLONP", "Strasbourg"));
		OrderCollection.Add(new Order("1014", "Gina Irene", "UK", "AROUT", "London"));
	}
}