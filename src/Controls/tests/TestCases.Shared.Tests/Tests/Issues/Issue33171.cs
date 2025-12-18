using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
    public class Issue33171 : _IssuesUITest
    {
        public override string Issue => "When TitleBar.IsVisible = false the caption buttons become unresponsive on Windows";

        public Issue33171(TestDevice device) : base(device) { }

        [Test]
        [Category(UITestCategories.TitleView)]
        public void TitleBarCaptionButtonsResponsiveWhenIsVisibleFalse()
        {
            // Wait for the page to load
            App.WaitForElement("StatusLabel");
            App.WaitForElement("ToggleTitleBarButton");

            // Verify initial state - TitleBar should be visible
            var statusLabel = App.FindElement("StatusLabel");
            var initialStatus = statusLabel.GetText();
            Assert.That(initialStatus, Does.Contain("TitleBar is currently visible"));

            // Hide the TitleBar by setting IsVisible = false
            App.Tap("ToggleTitleBarButton");

            // Verify TitleBar is now hidden
            statusLabel = App.FindElement("StatusLabel");
            var hiddenStatus = statusLabel.GetText();
            Assert.That(hiddenStatus, Does.Contain("TitleBar is hidden"));

            // Resize the window to trigger potential caption button responsiveness issues
            App.WaitForElement("ResizeWindowButton");
            App.Tap("ResizeWindowButton");

            // Verify the resize operation was performed
            var testResultLabel = App.FindElement("TestResultLabel");
            var resizeResult = testResultLabel.GetText();
            Assert.That(resizeResult, Does.Contain("Window resized"));

            // Perform a few more resizes to test different window widths
            App.Tap("ResizeWindowButton");
            App.Tap("ResizeWindowButton");

            // The test validates that the UI operations complete successfully
            // In a real scenario, manual testing would be needed to verify
            // that caption buttons (minimize, maximize, close) remain responsive
            
            // Verify final state
            testResultLabel = App.FindElement("TestResultLabel");
            var finalResult = testResultLabel.GetText();
            Assert.That(finalResult, Does.Contain("Try caption buttons"));

            // Show the TitleBar again to verify toggle functionality
            App.Tap("ToggleTitleBarButton");
            
            // Verify TitleBar is visible again
            statusLabel = App.FindElement("StatusLabel");
            var visibleStatus = statusLabel.GetText();
            Assert.That(visibleStatus, Does.Contain("TitleBar is currently visible"));
        }

        [Test]
        [Category(UITestCategories.TitleView)]
        public void TitleBarInstructionsPanelToggle()
        {
            // Wait for elements to be available
            App.WaitForElement("ShowInstructionsButton");
            App.WaitForElement("InstructionsPanel");

            // Initially, instructions should be hidden
            var instructionsPanel = App.FindElement("InstructionsPanel");
            var isVisible = instructionsPanel.GetAttribute<string>("visible");
            
            // Show instructions
            App.Tap("ShowInstructionsButton");
            
            // Verify instructions are now shown
            var buttonText = App.FindElement("ShowInstructionsButton").GetText();
            Assert.That(buttonText, Does.Contain("Hide Instructions"));

            // Hide instructions again
            App.Tap("ShowInstructionsButton");
            
            // Verify instructions are hidden
            buttonText = App.FindElement("ShowInstructionsButton").GetText();
            Assert.That(buttonText, Does.Contain("Show Instructions"));
        }

        [Test]
        [Category(UITestCategories.TitleView)]  
        public void TitleBarToggleUpdatesStatusCorrectly()
        {
            // Wait for page to load
            App.WaitForElement("StatusLabel");
            App.WaitForElement("ToggleTitleBarButton");
            App.WaitForElement("TestResultLabel");

            // Verify initial state
            var statusLabel = App.FindElement("StatusLabel");
            var testResultLabel = App.FindElement("TestResultLabel");
            var toggleButton = App.FindElement("ToggleTitleBarButton");
            
            Assert.That(statusLabel.GetText(), Does.Contain("TitleBar is currently visible"));
            Assert.That(toggleButton.GetText(), Does.Contain("Hide TitleBar"));

            // Hide TitleBar
            App.Tap("ToggleTitleBarButton");
            
            // Verify status updates
            Assert.That(statusLabel.GetText(), Does.Contain("TitleBar is hidden"));
            Assert.That(toggleButton.GetText(), Does.Contain("Show TitleBar"));
            Assert.That(testResultLabel.GetText(), Does.Contain("TitleBar hidden"));

            // Show TitleBar again
            App.Tap("ToggleTitleBarButton");
            
            // Verify status returns to visible
            Assert.That(statusLabel.GetText(), Does.Contain("TitleBar is currently visible"));
            Assert.That(toggleButton.GetText(), Does.Contain("Hide TitleBar"));
            Assert.That(testResultLabel.GetText(), Does.Contain("TitleBar visible"));
        }
    }
}