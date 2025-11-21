namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}
}

public class Bugzilla45743 : NavigationPage
{
	public Bugzilla45743()
	{
		Init();
	}
	protected void Init()
	{
		PushAsync(new ContentPage
		{
			Content = new StackLayout
			{
				AutomationId = "Page1",
				Children =
			{
				new Label { Text = "Page 1" }
			}
			}
		});

		MainThread.BeginInvokeOnMainThread(async () =>
		{
			await DisplayAlertAsync("Title", "Message", "Accept", "Cancel");
		});

		MainThread.BeginInvokeOnMainThread(async () =>
		{
			await PushAsync(new ContentPage
			{
				AutomationId = "Page2",
				Content = new StackLayout
				{
					Children =
				{
					new Label { Text = "Page 2", AutomationId = "Page 2" }
				}
				}
			});
		});

		MainThread.BeginInvokeOnMainThread(async () =>
		{
			await DisplayAlertAsync("Title 2", "Message", "Accept", "Cancel");
		});

		MainThread.BeginInvokeOnMainThread(async () =>
		{
			await DisplayActionSheetAsync("ActionSheet Title", "Cancel", "Close", new string[] { "Test", "Test 2" });
		});

	}
}