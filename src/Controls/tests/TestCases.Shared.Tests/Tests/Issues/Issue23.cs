using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
    public class Issue23 : _IssuesUITest
    {
        public override string Issue => "Horizontal swipes not working when SwipeItems are set in all directions";

        public Issue23(TestDevice device)
            : base(device)
        {
        }

        [Test]
        [Category(UITestCategories.SwipeView)]
        public void HorizontalSwipesShouldWorkWhenAllDirectionsConfigured()
        {
            var swipeView = App.WaitForElement("SwipeViewAllDirections");
            var resultLabel = App.WaitForElement("ResultLabel");

            // Verify initial state
            Assert.AreEqual("No swipe detected", resultLabel.GetText());

            // Test left swipe (swipe from right to left to reveal left items)
            var rect = swipeView.GetRect();
            var centerX = rect.X + rect.Width / 2;
            var centerY = rect.Y + rect.Height / 2;
            var leftX = rect.X + 50;

            App.DragCoordinates(centerX, centerY, leftX, centerY);
            App.WaitForElement("LeftSwipeItem");

            // Tap the left swipe item to trigger it
            App.Tap("LeftSwipeItem");

            // Wait a bit for the action to complete
            System.Threading.Thread.Sleep(500);

            // Verify left swipe was triggered
            var leftResult = resultLabel.GetText();
            Assert.AreEqual("Left swipe triggered", leftResult, "Left swipe should be triggered when swiping from right to left");

            // Close the swipe view by tapping the center
            App.Tap("SwipeViewAllDirections");
            System.Threading.Thread.Sleep(500);

            // Test right swipe (swipe from left to right to reveal right items) 
            var rightX = rect.X + rect.Width - 50;
            App.DragCoordinates(centerX, centerY, rightX, centerY);
            App.WaitForElement("RightSwipeItem");

            // Tap the right swipe item to trigger it
            App.Tap("RightSwipeItem");

            // Wait a bit for the action to complete
            System.Threading.Thread.Sleep(500);

            // Verify right swipe was triggered
            var rightResult = resultLabel.GetText();
            Assert.AreEqual("Right swipe triggered", rightResult, "Right swipe should be triggered when swiping from left to right");
        }

        [Test]
        [Category(UITestCategories.SwipeView)]
        public void VerticalSwipesShouldAlsoWorkWhenAllDirectionsConfigured()
        {
            var swipeView = App.WaitForElement("SwipeViewAllDirections");
            var resultLabel = App.WaitForElement("ResultLabel");

            // Test top swipe (swipe from bottom to top to reveal top items)
            var rect = swipeView.GetRect();
            var centerX = rect.X + rect.Width / 2;
            var centerY = rect.Y + rect.Height / 2;
            var topY = rect.Y + 20;

            App.DragCoordinates(centerX, centerY, centerX, topY);
            App.WaitForElement("TopSwipeItem");

            // Tap the top swipe item to trigger it
            App.Tap("TopSwipeItem");

            // Wait a bit for the action to complete
            System.Threading.Thread.Sleep(500);

            // Verify top swipe was triggered
            var topResult = resultLabel.GetText();
            Assert.AreEqual("Top swipe triggered", topResult, "Top swipe should be triggered when swiping from bottom to top");

            // Close the swipe view by tapping the center
            App.Tap("SwipeViewAllDirections");
            System.Threading.Thread.Sleep(500);

            // Test bottom swipe (swipe from top to bottom to reveal bottom items)
            var bottomY = rect.Y + rect.Height - 20;
            App.DragCoordinates(centerX, centerY, centerX, bottomY);
            App.WaitForElement("BottomSwipeItem");

            // Tap the bottom swipe item to trigger it
            App.Tap("BottomSwipeItem");

            // Wait a bit for the action to complete
            System.Threading.Thread.Sleep(500);

            // Verify bottom swipe was triggered
            var bottomResult = resultLabel.GetText();
            Assert.AreEqual("Bottom swipe triggered", bottomResult, "Bottom swipe should be triggered when swiping from top to bottom");
        }
    }
}