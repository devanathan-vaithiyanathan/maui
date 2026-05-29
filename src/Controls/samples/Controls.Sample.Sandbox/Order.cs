namespace Maui.Controls.Sample;

public class Order
{
	public string OrderID { get; set; }
	public string CustomerID { get; set; }
	public string ShipCountry { get; set; }
	public string Customer { get; set; }
	public string ShipCity { get; set; }

	public Order(string orderId, string customerId, string country, string customer, string shipCity)
	{
		OrderID = orderId;
		CustomerID = customerId;
		ShipCountry = country;
		Customer = customer;
		ShipCity = shipCity;
	}
}