using System;
using System.Drawing;
using ImageMagick;
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue40370 : _IssuesUITest
{
	public Issue40370(TestDevice device) : base(device)
	{
	}

	public override string Issue => "Resetting ProgressColor restores the platform default";

	[Test]
	[Category(UITestCategories.ProgressBar)]
	public void ResettingProgressColorToNullRestoresDefaultAppearance()
	{
		App.WaitForElement("ResetProgressColorButton");
		App.Tap("ResetProgressColorButton");
		VerifyScreenshot();
	}
}