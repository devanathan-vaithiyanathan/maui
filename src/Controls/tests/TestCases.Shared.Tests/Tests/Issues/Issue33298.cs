using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue33298 : _IssuesUITest
{
	public Issue33298(TestDevice device) : base(device) { }

	public override string Issue => "[WinUI]Editor cursor renders as “dot” after clearing text when parent Border.StrokeThickness is changed dynamically";

	[Test]
	[Category(UITestCategories.Editor)]
	public void VerifyEditorCursorRendering()
	{
		App.WaitForElement("AddText");
		App.Click("AddText");
		App.Click("EmptyText");
		VerifyScreenshot();
	}
}