namespace Maui.Controls.Sample;

public partial class SecondaryPage : ContentPage
{
	public SecondaryPage()
	{
		InitializeComponent();
		UpdateStatus();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		UpdateStatus();
	}

	void OnPageTrueClicked(object? sender, EventArgs e)
	{
		Shell.SetNavBarIsVisible(this, true);
		UpdateStatus();
	}

	void OnPageFalseClicked(object? sender, EventArgs e)
	{
		Shell.SetNavBarIsVisible(this, false);
		UpdateStatus();
	}

	void OnPageClearClicked(object? sender, EventArgs e)
	{
		ClearValue(Shell.NavBarIsVisibleProperty);
		UpdateStatus();
	}

	void UpdateStatus()
	{
		var pageIsSet = IsSet(Shell.NavBarIsVisibleProperty);
		var pageValue = Shell.GetNavBarIsVisible(this);
		PageStatusLabel.Text = pageIsSet
			? $"Page: set {pageValue}"
			: $"Page: unset ({pageValue})";
	}
}