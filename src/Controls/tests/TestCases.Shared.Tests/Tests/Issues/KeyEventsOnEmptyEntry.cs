using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
    public class KeyEventsOnEmptyEntry : _IssuesUITest
    {
        public KeyEventsOnEmptyEntry(TestDevice testDevice) : base(testDevice)
        {
        }

        public override string Issue => "Key events (KeyDown/KeyUp) should trigger when backspace is pressed on empty Entry on iOS/Mac";

        [Test]
        [Category(UITestCategories.Entry)]
        public void KeyEventsTriggeredOnEmptyEntry()
        {
            App.WaitForElement("TestEntry");
            
            // Focus the entry
            App.Tap("TestEntry");
            
            // Clear any existing text to ensure entry is empty
            App.ClearText("TestEntry");
            
            // Press backspace on empty entry
            // Note: This simulates the backspace key press on an empty field
            App.PressKeycode("Backspace");
            
            // Wait a moment for events to process
            System.Threading.Thread.Sleep(1000);
            
            // Verify that key events were triggered
            var keyDownLabel = App.FindElement("KeyDownLabel");
            var keyUpLabel = App.FindElement("KeyUpLabel"); 
            var eventCountLabel = App.FindElement("EventCountLabel");
            
            // Check that KeyDown event was triggered with "Backspace"
            Assert.That(keyDownLabel.GetText(), Does.Contain("Backspace"), 
                "KeyDown event should be triggered with 'Backspace' key when backspace is pressed on empty Entry");
            
            // Check that KeyUp event was triggered with "Backspace"
            Assert.That(keyUpLabel.GetText(), Does.Contain("Backspace"),
                "KeyUp event should be triggered with 'Backspace' key when backspace is pressed on empty Entry");
            
            // Check that at least 2 events were triggered (KeyDown + KeyUp)
            Assert.That(eventCountLabel.GetText(), Does.Not.Contain("Total Events: 0"),
                "Both KeyDown and KeyUp events should be triggered when backspace is pressed on empty Entry");
        }
        
        [Test]
        [Category(UITestCategories.Entry)]
        public void KeyEventsTriggeredWhenTypingInEntry()
        {
            App.WaitForElement("TestEntry");
            
            // Clear log first
            App.Tap("ClearButton");
            
            // Focus the entry and type some text
            App.Tap("TestEntry");
            App.EnterText("TestEntry", "A");
            
            // Wait a moment for events to process
            System.Threading.Thread.Sleep(1000);
            
            // Verify that events were triggered for typing
            var eventCountLabel = App.FindElement("EventCountLabel");
            Assert.That(eventCountLabel.GetText(), Does.Not.Contain("Total Events: 0"),
                "Key events should be triggered when typing in Entry");
        }
    }
}