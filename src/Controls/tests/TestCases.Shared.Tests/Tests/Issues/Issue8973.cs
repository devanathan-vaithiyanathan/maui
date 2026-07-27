using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue8973 : _IssuesUITest
{
    public override string Issue => "There isn't a toggling event for switch";

    public Issue8973(TestDevice device) : base(device) { }

    [Test]
    [Category(UITestCategories.Switch)]
    public void SwitchTogglingEventFiresBeforeToggle()
    {
        App.WaitForElement("Issue8973Switch");

        // Initially not fired
        Assert.That(App.FindElement("Issue8973TogglingLabel").GetText(), Is.EqualTo("Toggling: not fired"));

        // Tap the switch — Toggling event should fire
        App.Tap("Issue8973Switch");

        Assert.That(App.FindElement("Issue8973TogglingLabel").GetText(), Does.Contain("Toggling: fired, Value=True"));
        Assert.That(App.FindElement("Issue8973ToggledStateLabel").GetText(), Is.EqualTo("IsToggled: True"));
    }

    [Test]
    [Category(UITestCategories.Switch)]
    public void SwitchTogglingEventCanBeCancelled()
    {
        App.WaitForElement("Issue8973Switch");

        // Enable cancellation
        App.Tap("Issue8973CancelButton");
        Assert.That(App.FindElement("Issue8973CancelStatusLabel").GetText(), Is.EqualTo("Cancel: ON"));

        // Tap the switch — toggle should be cancelled
        App.Tap("Issue8973Switch");

        // Toggling should report cancelled
        Assert.That(App.FindElement("Issue8973TogglingLabel").GetText(), Does.Contain("Toggling: cancelled"));

        // IsToggled should remain false (unchanged)
        Assert.That(App.FindElement("Issue8973ToggledStateLabel").GetText(), Is.EqualTo("IsToggled: False"));
    }
}
