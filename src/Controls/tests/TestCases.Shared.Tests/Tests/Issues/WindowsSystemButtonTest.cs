using NUnit.Framework;
using UITest.Appium;
using UITest.Core;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
    public class WindowsSystemButtonTest : _IssuesUITest
    {
        public override string Issue => "Windows System Button Test - Minimize, Maximize, Close";

        public WindowsSystemButtonTest(TestDevice device) : base(device) { }

        [Test]
        [Category(UITestCategories.Window)]
        [Category(UITestCategories.TitleView)]
        public void WindowsSystemButtonsResponsive()
        {
            // Only run on Windows
            if (Device != TestDevice.Windows)
            {
                Assert.Ignore("This test is specific to Windows platform");
                return;
            }

            // Wait for the page to load
            App.WaitForElement("StatusLabel");
            App.WaitForElement("ToggleTitleBarButton");
            App.WaitForElement("TestSystemButtonsButton");

            // Verify initial state - TitleBar should be visible
            var statusLabel = App.FindElement("StatusLabel");
            var initialStatus = statusLabel.GetText();
            Assert.That(initialStatus, Does.Contain("TitleBar is currently visible"));

            // Test system buttons with TitleBar visible
            TestSystemButtonsWithTitleBarVisible();

            // Hide the TitleBar and test again
            App.Tap("ToggleTitleBarButton");

            // Verify TitleBar is now hidden
            statusLabel = App.FindElement("StatusLabel");
            var hiddenStatus = statusLabel.GetText();
            Assert.That(hiddenStatus, Does.Contain("TitleBar is hidden"));

            // Test system buttons with TitleBar hidden (this is where the bug occurs)
            TestSystemButtonsWithTitleBarHidden();

            // Show the TitleBar again to verify toggle functionality
            App.Tap("ToggleTitleBarButton");
            
            // Verify TitleBar is visible again
            statusLabel = App.FindElement("StatusLabel");
            var visibleStatus = statusLabel.GetText();
            Assert.That(visibleStatus, Does.Contain("TitleBar is currently visible"));
        }

        private void TestSystemButtonsWithTitleBarVisible()
        {
            // Test system buttons when TitleBar is visible
            var minimizeResult = TestMinimizeButton();
            var maximizeResult = TestMaximizeButton();
            var closeResult = TestCloseButtonAccessibility();

            // All buttons should be responsive when TitleBar is visible
            Assert.That(minimizeResult, Is.True, "Minimize button should be responsive when TitleBar is visible");
            Assert.That(maximizeResult, Is.True, "Maximize button should be responsive when TitleBar is visible");
            Assert.That(closeResult, Is.True, "Close button should be accessible when TitleBar is visible");
        }

        private void TestSystemButtonsWithTitleBarHidden()
        {
            // Test system buttons when TitleBar is hidden (this may reveal the bug)
            var minimizeResult = TestMinimizeButton();
            var maximizeResult = TestMaximizeButton();
            var closeResult = TestCloseButtonAccessibility();

            // These assertions will help identify if the bug exists
            Assert.That(minimizeResult, Is.True, "Minimize button should remain responsive when TitleBar is hidden");
            Assert.That(maximizeResult, Is.True, "Maximize button should remain responsive when TitleBar is hidden");
            Assert.That(closeResult, Is.True, "Close button should remain accessible when TitleBar is hidden");
        }

        private bool TestMinimizeButton()
        {
            // Use the new extension method
            return App.TestMinimizeButton(clickButton: false);
        }

        private bool TestMaximizeButton()
        {
            // Use the new extension method with actual clicking to test responsiveness
            return App.TestMaximizeButton(clickButton: true);
        }

        private bool TestCloseButtonAccessibility()
        {
            // Use the new extension method
            return App.TestCloseButtonAccessible();
        }

        [Test]
        [Category(UITestCategories.Window)]
        [Category(UITestCategories.TitleView)]
        public void WindowsSystemButtonsDiscovery()
        {
            // Only run on Windows
            if (Device != TestDevice.Windows)
            {
                Assert.Ignore("This test is specific to Windows platform");
                return;
            }

            App.WaitForElement("TestSystemButtonsButton");

            // Use the extension method to discover system buttons
            var discoveredButtons = App.DiscoverSystemButtons();
            
            Assert.That(discoveredButtons.Count, Is.GreaterThan(0), "Should discover some button elements");
            Console.WriteLine($"UITEST: Discovered {discoveredButtons.Count} button elements");
        }

        [Test]
        [Category(UITestCategories.TitleView)]
        public void AutomatedSystemButtonTest()
        {
            // Wait for page to load
            App.WaitForElement("StatusLabel");
            App.WaitForElement("TestSystemButtonsButton");
            App.WaitForElement("TestResultLabel");

            // Use the app's built-in test functionality
            App.Tap("TestSystemButtonsButton");

            // Wait for the test to complete
            System.Threading.Thread.Sleep(3000);

            // Check the test results
            var testResultLabel = App.FindElement("TestResultLabel");
            var testResult = testResultLabel.GetText();
            
            Assert.That(testResult, Does.Contain("Test Results:"), "System button test should complete");
            Assert.That(testResult, Does.Not.Contain("Error"), "System button test should not have errors");

            Console.WriteLine($"UITEST: Automated test result: {testResult}");
        }

        [Test]
        [Category(UITestCategories.Window)]
        public void TestAllSystemButtonsAtOnce()
        {
            // Only run on Windows
            if (Device != TestDevice.Windows)
            {
                Assert.Ignore("This test is specific to Windows platform");
                return;
            }

            App.WaitForElement("StatusLabel");

            // Test all system buttons using the extension method
            var results = App.TestAllSystemButtons();

            // Verify that we got results for all buttons
            Assert.That(results.ContainsKey("Minimize"), Is.True, "Should test minimize button");
            Assert.That(results.ContainsKey("Maximize"), Is.True, "Should test maximize button");
            Assert.That(results.ContainsKey("Close"), Is.True, "Should test close button");

            // Log the results
            foreach (var result in results)
            {
                Console.WriteLine($"UITEST: {result.Key} button accessible: {result.Value}");
            }

            // At least one system button should be accessible
            Assert.That(results.Values.Any(v => v), Is.True, "At least one system button should be accessible");
        }
    }
}