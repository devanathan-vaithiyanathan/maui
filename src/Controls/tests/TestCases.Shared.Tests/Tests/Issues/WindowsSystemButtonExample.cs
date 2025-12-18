using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
    /// <summary>
    /// Example test demonstrating how to use Windows system button testing
    /// This shows how other tests can easily incorporate system button validation
    /// </summary>
    public class WindowsSystemButtonExample : _IssuesUITest
    {
        public override string Issue => "Example: How to test Windows system buttons in your MAUI tests";

        public WindowsSystemButtonExample(TestDevice device) : base(device) { }

        [Test]
        [Category(UITestCategories.Window)]
        public void ExampleSystemButtonTest()
        {
            // Skip on non-Windows platforms
            if (Device != TestDevice.Windows)
            {
                Assert.Ignore("This example is for Windows platform only");
                return;
            }

            // Example 1: Test individual buttons
            bool isMinimizeAccessible = App.TestMinimizeButton(clickButton: false);
            bool isMaximizeAccessible = App.TestMaximizeButton(clickButton: false);
            bool isCloseAccessible = App.TestCloseButtonAccessible();

            Console.WriteLine($"Example: Minimize accessible: {isMinimizeAccessible}");
            Console.WriteLine($"Example: Maximize accessible: {isMaximizeAccessible}");
            Console.WriteLine($"Example: Close accessible: {isCloseAccessible}");

            // Example 2: Test all buttons at once
            var allResults = App.TestAllSystemButtons();
            foreach (var result in allResults)
            {
                Console.WriteLine($"Example: {result.Key} button result: {result.Value}");
            }

            // Example 3: Discover what buttons are available
            var discoveredButtons = App.DiscoverSystemButtons();
            Console.WriteLine($"Example: Discovered {discoveredButtons.Count} button elements");

            // Example assertions - adapt these to your test needs
            Assert.That(isMinimizeAccessible || isMaximizeAccessible || isCloseAccessible, 
                       Is.True, 
                       "At least one system button should be accessible");
        }

        [Test]
        [Category(UITestCategories.Window)]
        public void ExampleTestMaximizeButtonFunctionality()
        {
            // Skip on non-Windows platforms
            if (Device != TestDevice.Windows)
            {
                Assert.Ignore("This example is for Windows platform only");
                return;
            }

            // Example: Actually click the maximize button to test window state changes
            // This demonstrates testing actual functionality, not just accessibility
            bool maximizeWorked = App.TestMaximizeButton(clickButton: true);
            
            Console.WriteLine($"Example: Maximize button click result: {maximizeWorked}");
            
            // You could add additional verification here:
            // - Check window dimensions changed
            // - Verify the button changed from "Maximize" to "Restore" 
            // - Test that clicking again restores the window
            
            Assert.That(maximizeWorked, Is.True, "Maximize button should respond to clicks");
            
            // Wait a bit then test clicking it again (should restore)
            System.Threading.Thread.Sleep(1000);
            bool restoreWorked = App.TestMaximizeButton(clickButton: true);
            
            Console.WriteLine($"Example: Restore button click result: {restoreWorked}");
            Assert.That(restoreWorked, Is.True, "Restore button should respond to clicks");
        }
    }
}