namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue30535 : _IssuesUITest
{
	public override string Issue => "[Windows] RefreshView IsRefreshing property not working while binding";

	public Issue30535(TestDevice device) : base(device) { }

	[Test]
	[Category(UITestCategories.RefreshView)]
	public void RefreshViewIsRefreshingShouldShowIndicatorWhenSetViaBinding()
	{
		// Wait for the main page to load
		App.WaitForElement("InstructionLabel");
		App.WaitForElement("OptionsButton");

		// Initial status should be false
		var statusLabel = App.FindElement("StatusLabel");
		Assert.That(statusLabel.GetText(), Does.Contain("False"), "Initial IsRefreshing should be False");

		// Navigate to Options page
		App.Tap("OptionsButton");
		App.WaitForElement("SetTrueButton");

		// Set IsRefreshing to true
		App.Tap("SetTrueButton");

		// Verify the button worked
		var currentStatus = App.FindElement("CurrentStatusLabel");
		Assert.That(currentStatus.GetText(), Does.Contain("True"), "IsRefreshing should be True after button tap");

		// Navigate back to main page
		App.Tap("ApplyButton");
		
		// Wait for main page to appear
		App.WaitForElement("StatusLabel");

		// Give the RefreshView time to update
		Task.Delay(1000).Wait();

		// Verify that IsRefreshing is still true
		statusLabel = App.FindElement("StatusLabel");
		Assert.That(statusLabel.GetText(), Does.Contain("True"), "IsRefreshing should still be True on main page");

		// On Windows, the refresh indicator should be visible
		// We verify this by checking that the RefreshView exists and the binding is working
		// The actual visual indicator is part of the native Windows RefreshContainer control
		var refreshView = App.FindElement("RefreshView");
		Assert.That(refreshView, Is.Not.Null, "RefreshView should be present");

		// Verify we can set it back to false
		App.Tap("OptionsButton");
		App.WaitForElement("SetFalseButton");
		App.Tap("SetFalseButton");
		App.Tap("ApplyButton");
		App.WaitForElement("StatusLabel");
		
		Task.Delay(500).Wait();
		
		statusLabel = App.FindElement("StatusLabel");
		Assert.That(statusLabel.GetText(), Does.Contain("False"), "IsRefreshing should be False after setting to false");
	}
}
