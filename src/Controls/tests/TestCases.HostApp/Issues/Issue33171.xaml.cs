using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 33171, "When TitleBar.IsVisible = false the caption buttons become unresponsive on Windows", PlatformAffected.UWP)]
public partial class Issue33171 : ContentPage
{
    private TitleBar _titleBar;
    private bool _isTitleBarVisible = true;
    private int _resizeCount = 0;

    public Issue33171()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // Create and configure the TitleBar
        _titleBar = new TitleBar
        {
            Title = "Issue 33171 Test",
            Subtitle = "TitleBar Caption Button Test",
            BackgroundColor = Colors.DarkBlue,
            ForegroundColor = Colors.White
        };

        // Set the TitleBar on the window
        if (Window != null)
        {
            Window.TitleBar = _titleBar;
            Console.WriteLine("SANDBOX: TitleBar initialized and set on window");
        }
        else
        {
            Console.WriteLine("SANDBOX: Window is null, cannot set TitleBar");
        }

        UpdateStatusDisplay();
    }

    private void OnToggleTitleBarClicked(object sender, EventArgs e)
    {
        if (_titleBar == null)
        {
            Console.WriteLine("SANDBOX: TitleBar is null, cannot toggle visibility");
            return;
        }

        _isTitleBarVisible = !_isTitleBarVisible;
        _titleBar.IsVisible = _isTitleBarVisible;

        if (_isTitleBarVisible)
        {
            ToggleTitleBarButton.Text = "Hide TitleBar (Set IsVisible = false)";
            StatusLabel.Text = "TitleBar is currently visible";
            StatusLabel.BackgroundColor = Colors.LightGreen;
        }
        else
        {
            ToggleTitleBarButton.Text = "Show TitleBar (Set IsVisible = true)";
            StatusLabel.Text = "TitleBar is hidden - caption buttons may be unresponsive";
            StatusLabel.BackgroundColor = Colors.LightCoral;
        }

        Console.WriteLine($"SANDBOX: TitleBar.IsVisible set to {_isTitleBarVisible}");
        UpdateTestResult();
    }

    private void OnResizeWindowClicked(object sender, EventArgs e)
    {
        if (Window == null)
        {
            Console.WriteLine("SANDBOX: Window is null, cannot resize");
            return;
        }

        _resizeCount++;
        
        // Simulate different window sizes that might trigger the issue
        double[] testWidths = { 800, 600, 1000, 700, 900 };
        double[] testHeights = { 600, 500, 700, 550, 650 };
        
        int index = _resizeCount % testWidths.Length;
        double newWidth = testWidths[index];
        double newHeight = testHeights[index];

        Console.WriteLine($"SANDBOX: Resizing window to {newWidth}x{newHeight} (resize #{_resizeCount})");
        
        // Note: Window resizing in MAUI may be platform-specific
        // This is mainly for testing purposes
        Window.Width = newWidth;
        Window.Height = newHeight;
        
        TestResultLabel.Text = $"Window resized to {newWidth}x{newHeight} - Test caption buttons now";
        TestResultLabel.BackgroundColor = Colors.Orange;

        UpdateTestResult();
    }

    private void OnShowInstructionsClicked(object sender, EventArgs e)
    {
        InstructionsPanel.IsVisible = !InstructionsPanel.IsVisible;
        ShowInstructionsButton.Text = InstructionsPanel.IsVisible ? "Hide Instructions" : "Show Instructions";
        
        Console.WriteLine($"SANDBOX: Instructions panel visibility: {InstructionsPanel.IsVisible}");
    }

    private void UpdateStatusDisplay()
    {
        if (_titleBar != null)
        {
            StatusLabel.Text = $"TitleBar is currently {(_isTitleBarVisible ? "visible" : "hidden")}";
            StatusLabel.BackgroundColor = _isTitleBarVisible ? Colors.LightGreen : Colors.LightCoral;
        }
    }

    private void UpdateTestResult()
    {
        if (!_isTitleBarVisible)
        {
            TestResultLabel.Text = "Test Status: TitleBar hidden - Try caption buttons after resizing";
            TestResultLabel.BackgroundColor = Colors.Yellow;
        }
        else
        {
            TestResultLabel.Text = "Test Status: TitleBar visible - Caption buttons should work";
            TestResultLabel.BackgroundColor = Colors.LightBlue;
        }
    }
}