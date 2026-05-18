using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue35387 : _IssuesUITest
{
	public Issue35387(TestDevice device) : base(device)
	{
	}

	public override string Issue => "PolygonHandler and PolylineHandler leak when Points is replaced before disconnect";

	[Test]
	[Category(UITestCategories.Layout)]
	public void PolygonPolylinePointsLeakUITest()
	{
		App.WaitForElement("RunLeakTestButton");
		App.Tap("RunLeakTestButton");
		App.WaitForElement("LeakResultLabel");

		var result = App.FindElement("LeakResultLabel").GetText();
		Assert.That(result, Is.EqualTo("Success: No leak detected"), "Memory leak detected or test did not run as expected.");
	}
}
