using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	int count = 0;

	public AViewModel A { get; } = new();

	public MainPage()
	{
		InitializeComponent();
		this.BindingContext = this;
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

public class AViewModel : Observable
{
	public BViewModel B
	{
		get;
		set => Set(ref field, value);
	}
}

public class BViewModel : Observable
{
	public CViewModel C
	{
		get;
		set => Set(ref field, value);
	}
}

public class CViewModel : Observable
{
	public string Text
	{
		get;
		set => Set(ref field, value);
	} = "Hello World!!";
}

public abstract class Observable : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	protected void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
	{
		if (!Equals(storage, value))
		{
			storage = value;
			this.PropertyChanged?.Invoke(this, new(propertyName));
		}
	}
}