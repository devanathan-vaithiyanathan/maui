using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

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
        App.Tap("Issue38188Options");
        App.WaitForElement("Issue38188ShadowTrueButton");
        App.Tap("Issue38188ShadowTrueButton");
        App.Tap("Issue38188Apply");
        App.WaitForElement("Issue38188SearchBar");
    }
}