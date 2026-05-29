namespace Maui.Controls.Sample;

public partial class LeakReproLauncherPage : ContentPage
{
	bool _isRunning;

	public LeakReproLauncherPage()
	{
		InitializeComponent();
		RefreshLeakProbeText();
	}

	async void OnGoToReproPageClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new MainPage());
	}

	async void OnRunCyclesClicked(object sender, EventArgs e)
	{
		if (_isRunning)
		{
			return;
		}

		_isRunning = true;
		try
		{
			for (var i = 1; i <= 30; i++)
			{
				var page = new MainPage();
				await Navigation.PushAsync(page, false);
				await page.SimulateCollectionViewLifecycleAsync();
				await Navigation.PopAsync(false);

				if (i % 5 == 0)
				{
					LeakProbe.Log($"cycle={i} {LeakProbe.BuildSummary()}");
					RefreshLeakProbeText();
				}

				await Task.Delay(80);
			}

			LeakProbe.Log("cycle run completed");
		}
		finally
		{
			_isRunning = false;
			RefreshLeakProbeText();
		}
	}

	void OnRefreshLeakSummaryClicked(object sender, EventArgs e)
	{
		RefreshLeakProbeText();
	}

	void RefreshLeakProbeText()
	{
		LeakSummaryLabel.Text = LeakProbe.BuildSummary();
		LeakDetailsLabel.Text = LeakProbe.BuildAliveDetails();
	}
}