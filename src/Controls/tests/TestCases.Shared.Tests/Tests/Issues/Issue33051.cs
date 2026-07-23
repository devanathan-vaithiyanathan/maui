using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

// On iOS 26+, UISearchBar introduced a native clear button (circular X) inside the text field.
// Before the fix, ShowsCancelButton=true was also set when text was present, causing a second
// large X button to appear outside the text field. Verify only one clear button is shown.
public class Issue33051 : _IssuesUITest
{
    public Issue33051(TestDevice device) : base(device) { }

    public override string Issue => "[iOS 26] SearchBar shows double clear buttons when text is entered";

    [Test]
    [Category(UITestCategories.SearchBar)]
    public void SearchBarShouldNotShowDoubleClearButtons()
    {
        App.WaitForElement("SearchBar33051");
        VerifyScreenshot();
    }
}
