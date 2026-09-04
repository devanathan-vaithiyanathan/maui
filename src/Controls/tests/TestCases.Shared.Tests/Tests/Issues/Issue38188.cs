using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue38188 : _IssuesUITest
{
    public Issue38188(TestDevice device) : base(device)
    {
    }

    public override string Issue => "Setting Shadow on SearchBar throws a COMException";

    [Test]
    [Category(UITestCategories.SearchBar)]
    public void SearchBarShadowCanBeSetAtRuntime()
    {
        App.WaitForElement("Issue38188SearchBar");
        App.WaitForElement("Issue38188ApplyShadowButton");

        App.Tap("Issue38188ApplyShadowButton");

        var status = App.WaitForElement("Issue38188StatusLabel").GetText();
        Assert.That(status, Is.EqualTo("Shadow applied"));
    }
}