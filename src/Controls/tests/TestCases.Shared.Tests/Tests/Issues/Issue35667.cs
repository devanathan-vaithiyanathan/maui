using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue35667 : _IssuesUITest
{
    public Issue35667(TestDevice device) : base(device) { }

    public override string Issue => "SearchHandler TextTransform does not update typed text";

    [Test]
    [Category(UITestCategories.Shell)]
    public void SearchHandlerShouldUppercaseTypedTextAndKeepCursorAtEnd()
    {
        App.WaitForElement("Issue35667PageLoaded");
        var searchHandler = App.GetShellSearchHandler();

        searchHandler.Tap();
        searchHandler.SendKeys("lower");
        Assert.That(searchHandler.GetText(), Is.EqualTo("LOWER"));

        searchHandler.SendKeys("case");
        Assert.That(searchHandler.GetText(), Is.EqualTo("LOWERCASE"));
    }
}