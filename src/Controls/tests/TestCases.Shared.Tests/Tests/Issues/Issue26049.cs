using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Issue26049 : _IssuesUITest
	{
		public Issue26049(TestDevice device) : base(device)
		{
		}

		public override string Issue => "[iOS] Fix ShellContent Title Does Not Update at Runtime";

		[Test, Order(1)]
		[Category(UITestCategories.Shell)]
		public void VerifyFirstShellContentTitle()
		{
			App.WaitForElement("ChangeShellContentTitle");
			App.Click("ChangeShellContentTitle");
			TapNavigationViewItemIfWindows();
			VerifyScreenshot();
		}

		[Test, Order(2)]
		[Category(UITestCategories.Shell)]
		public void VerifyNewlyAddedShellContentTitle()
		{
			TapNavigationViewItemIfWindows();
			App.WaitForElement("AddShellContent");
			App.Click("AddShellContent");
			App.Click("UpdateNewShellContentTitle");
			TapNavigationViewItemIfWindows();
			VerifyScreenshot();
		}

		[Test, Order(3)]
		[Category(UITestCategories.Shell)]
		public void VerifyExistingTabTitle()
		{
			TapNavigationViewItemIfWindows();
			App.WaitForElement("RemoveShellContent");
			App.Click("RemoveShellContent");
			App.Click("UpdateThirdTabTitle");
			TapNavigationViewItemIfWindows();
			VerifyScreenshot();
		}

		void TapNavigationViewItemIfWindows()
		{
#if WINDOWS
			App.Tap("navViewItem");
#endif
		}
	}
}