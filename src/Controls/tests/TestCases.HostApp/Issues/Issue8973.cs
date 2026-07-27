namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 8973, "There isn't a toggling event for switch", PlatformAffected.All)]
public class Issue8973 : ContentPage
{
    const string SwitchId = "Issue8973Switch";
    const string TogglingEventLabelId = "Issue8973TogglingLabel";
    const string ToggledStateLabelId = "Issue8973ToggledStateLabel";
    const string CancelToggleButtonId = "Issue8973CancelButton";
    const string CancelStatusLabelId = "Issue8973CancelStatusLabel";

    bool _cancelNextToggle;

    public Issue8973()
    {
        var theSwitch = new Switch
        {
            AutomationId = SwitchId,
            HorizontalOptions = LayoutOptions.Center,
        };

        var togglingLabel = new Label
        {
            AutomationId = TogglingEventLabelId,
            Text = "Toggling: not fired",
        };

        var toggledStateLabel = new Label
        {
            AutomationId = ToggledStateLabelId,
            Text = $"IsToggled: {theSwitch.IsToggled}",
        };

        var cancelStatusLabel = new Label
        {
            AutomationId = CancelStatusLabelId,
            Text = "Cancel: OFF",
        };

        var cancelButton = new Button
        {
            AutomationId = CancelToggleButtonId,
            Text = "Enable Cancel",
        };

        cancelButton.Clicked += (s, e) =>
        {
            _cancelNextToggle = !_cancelNextToggle;
            cancelStatusLabel.Text = _cancelNextToggle ? "Cancel: ON" : "Cancel: OFF";
            cancelButton.Text = _cancelNextToggle ? "Disable Cancel" : "Enable Cancel";
        };

        theSwitch.Toggling += (s, e) =>
        {
            togglingLabel.Text = $"Toggling: fired, Value={e.Value}";
            if (_cancelNextToggle)
            {
                e.Cancel = true;
                togglingLabel.Text = $"Toggling: cancelled, Value={e.Value}";
            }
        };

        theSwitch.Toggled += (s, e) =>
        {
            toggledStateLabel.Text = $"IsToggled: {e.Value}";
        };

        Content = new VerticalStackLayout
        {
            Spacing = 10,
            Padding = new Thickness(20),
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                new Label { Text = "Switch Toggling Event Test", FontAttributes = FontAttributes.Bold },
                theSwitch,
                togglingLabel,
                toggledStateLabel,
                cancelButton,
                cancelStatusLabel,
            }
        };
    }
}
