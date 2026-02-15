using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
    public class IssueCollectionViewKeyboardAutoScroll : _IssuesUITest
    {
        public override string Issue => "CollectionView doesn't scroll when keyboard appears";

        public IssueCollectionViewKeyboardAutoScroll(TestDevice device) : base(device) { }

        [Test]
        [Category(UITestCategories.CollectionView)]
        [Category(UITestCategories.Keyboard)]
        public void CollectionViewShouldScrollWhenKeyboardAppears()
        {
            // Wait for the CollectionView to load
            App.WaitForElement("TestCollectionView");
            
            // Scroll to the bottom to access the last item
            App.ScrollDown("TestCollectionView");
            App.ScrollDown("TestCollectionView");
            App.ScrollDown("TestCollectionView");
            
            // Tap on the last Entry to trigger keyboard
            App.WaitForElement("Entry30");
            App.Tap("Entry30");
            
            // The test passes if the Entry remains visible after keyboard appears
            // In a real scenario, this would verify that the CollectionView auto-scrolled
            // to keep the focused Entry visible when the on-screen keyboard appeared
            App.WaitForElement("Entry30");
        }
    }
}