using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue11404 : _IssuesUITest
{
	public override string Issue => "Line coordinates not computed correctly";

	public Issue11404(TestDevice device)
	: base(device)
	{ }

	[Test]
	[Category(UITestCategories.Shape)]
	public void VerifyLineCoOrdinatesPosition()
	{
		App.WaitForElement("LineCoordinatesLabel");
		VerifyScreenshot();
	}
}