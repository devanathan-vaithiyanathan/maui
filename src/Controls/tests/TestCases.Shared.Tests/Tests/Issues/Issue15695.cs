#if !MACCATALYST
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue15695 : _IssuesUITest
{
	public override string Issue => "Support for Switch OFF State color";

	public Issue15695(TestDevice device) : base(device)
	{
	}

	[Test]
	[Category(UITestCategories.Switch)]
	public void VerifySwitchOffColorAfterToggling()
	{
		App.WaitForElement("Switch");
		App.Tap("SwitchButton");
		App.Tap("SwitchButton");
		VerifyScreenshot();
	}
}
#endif