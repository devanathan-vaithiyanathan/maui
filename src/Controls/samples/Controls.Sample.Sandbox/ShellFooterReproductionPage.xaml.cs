namespace Maui.Controls.Sample;

public partial class ShellFooterReproductionPage : ContentPage
{
    MeasureProbeView? _oldFooter;
    MeasureProbeView? _currentFooter;

    public ShellFooterReproductionPage()
    {
        InitializeComponent();
    }

    async void OnPrepareFooterClicked(object? sender, EventArgs e)
    {
        _oldFooter = new MeasureProbeView("Footer A");
        Shell.Current.FlyoutFooter = _oldFooter;
        await Task.Delay(250);

        _currentFooter = new MeasureProbeView("Footer B");
        Shell.Current.FlyoutFooter = _currentFooter;
        await Task.Delay(250);

        _currentFooter.ResetMeasureCount();
        FooterStatus.Text = "Footer B installed. Invalidate footer A.";
    }

    async void OnInvalidateOldFooterClicked(object? sender, EventArgs e)
    {
        if (_oldFooter is null || _currentFooter is null)
        {
            FooterStatus.Text = "Replace footer A first.";
            return;
        }

        var before = _currentFooter.MeasureCount;
        _oldFooter.TriggerMeasureInvalidation();
        await Task.Delay(250);
        var after = _currentFooter.MeasureCount;

        FooterStatus.Text = after > before
            ? $"REPRODUCED: removed footer A measured footer B ({before} to {after})."
            : "NOT REPRODUCED: removed footer A did not invoke the renderer.";
    }

    sealed class MeasureProbeView : ContentView
    {
        public MeasureProbeView(string text)
        {
            Content = new Label { Text = text, Padding = 12 };
        }

        public int MeasureCount { get; private set; }

        public void ResetMeasureCount() => MeasureCount = 0;

        public void TriggerMeasureInvalidation() => InvalidateMeasure();

        protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            MeasureCount++;
            return base.MeasureOverride(widthConstraint, heightConstraint);
        }
    }
}