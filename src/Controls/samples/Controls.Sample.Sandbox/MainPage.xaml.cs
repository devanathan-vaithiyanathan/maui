namespace Maui.Controls.Sample;

// Reproduction for Issue #17916:
// Shadow on Label disappears permanently after opacity is tweened to zero on Windows.
// Steps to observe:
//   1. Tap "Fade Out (tween)" — labels animate from opacity 1 → 0
//   2. Tap "Fade In (tween)"  — labels animate from opacity 0 → 1
//   3. BUG: shadows are gone after fade-in on Windows
//   The blue-shadowed reference label uses hard-set opacity (not tweened) and keeps its shadow.
public partial class MainPage : ContentPage
{
	List<Label> _tweenedLabels = [];
	bool _isFadingOut;
	bool _isFadingIn;

	public MainPage()
	{
		InitializeComponent();
		_tweenedLabels = [ShadowLabel1, ShadowLabel2, ShadowLabel3];
	}

	async void OnFadeOutClicked(object sender, EventArgs e)
	{
		if (_isFadingOut || _isFadingIn) return;
		_isFadingOut = true;
		FadeOutButton.IsEnabled = false;
		StatusLabel.Text = "Tweening opacity 1 → 0 …";

		// Hard-set reference label to 0 (not a tween) — shadow should survive
		HardSetLabel.Opacity = 0;

		// Tween the opacity down gradually over 1 second
		var tasks = _tweenedLabels.Select(l => l.FadeToAsync(0, 1000)).ToArray();
		await Task.WhenAll(tasks);

		_isFadingOut = false;
		FadeInButton.IsEnabled = true;
		StatusLabel.Text = "Faded out. Tap 'Fade In' — watch if shadows return.";
	}

	async void OnFadeInClicked(object sender, EventArgs e)
	{
		if (_isFadingOut || _isFadingIn) return;
		_isFadingIn = true;
		FadeInButton.IsEnabled = false;
		StatusLabel.Text = "Tweening opacity 0 → 1 …";

		// Hard-set reference label back to 1 — shadow should still be visible
		HardSetLabel.Opacity = 1;

		// Tween the opacity back up — BUG: shadows don't come back on Windows
		var tasks = _tweenedLabels.Select(l => l.FadeToAsync(1, 1000)).ToArray();
		await Task.WhenAll(tasks);

		_isFadingIn = false;
		FadeOutButton.IsEnabled = true;
		StatusLabel.Text = "Faded in. Check whether black shadows are visible (bug: they won't be on Windows).";
	}
}