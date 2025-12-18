# Windows System Button Testing in .NET MAUI

This document describes the new Windows system button testing functionality added to the MAUI test framework.

## Overview

Windows applications have system buttons (minimize, maximize/restore, close) in the title bar. These buttons are part of the OS window chrome and can become unresponsive under certain conditions, such as when `TitleBar.IsVisible = false`.

This testing framework provides automated ways to:
- Test if system buttons are accessible
- Click system buttons programmatically
- Discover system buttons in the current window
- Validate system button responsiveness

## Usage

### Extension Methods

The following extension methods are available on `IApp` for Windows testing:

```csharp
// Test individual buttons (without clicking)
bool isMinimizeAccessible = App.TestMinimizeButton(clickButton: false);
bool isMaximizeAccessible = App.TestMaximizeButton(clickButton: false);
bool isCloseAccessible = App.TestCloseButtonAccessible();

// Test with actual clicking (for functionality testing)
bool maximizeWorked = App.TestMaximizeButton(clickButton: true);

// Test all buttons at once
var allResults = App.TestAllSystemButtons();
// Returns: Dictionary<string, bool> with keys: "Minimize", "Maximize", "Close"

// Discover available buttons (for debugging)
var discoveredButtons = App.DiscoverSystemButtons();
// Returns: List<string> with button information
```

### Example Test

```csharp
[Test]
[Category(UITestCategories.Window)]
public void TestWindowSystemButtons()
{
    // Only run on Windows
    if (Device.Platform != TestDevice.TestPlatform.Windows)
    {
        Assert.Ignore("This test is specific to Windows platform");
        return;
    }

    // Test all system buttons
    var results = App.TestAllSystemButtons();
    
    // Verify at least one button is accessible
    Assert.That(results.Values.Any(v => v), Is.True, 
               "At least one system button should be accessible");
    
    // Test specific functionality
    if (results["Maximize"])
    {
        // Click maximize and verify it works
        bool clicked = App.TestMaximizeButton(clickButton: true);
        Assert.That(clicked, Is.True, "Maximize should respond to clicks");
    }
}
```

## Files Created

### Test Files
1. **`WindowsSystemButtonTest.cs`** (HostApp) - Test page that provides UI for manual testing
2. **`WindowsSystemButtonTest.cs`** (UI Tests) - Automated tests for system button functionality
3. **`WindowsSystemButtonExample.cs`** (UI Tests) - Example showing how to use the new functionality

### Framework Files
4. **`WindowsSystemButtonExtensions.cs`** (UITest.Appium) - Extension methods for easy system button testing

## Test Strategy

The tests use multiple strategies to find Windows system buttons:

1. **By AutomationId/Id** - Standard accessibility identifiers
2. **By Name** - Button names (may be localized)
3. **By XPath with Name** - XPath queries using Name attribute
4. **By XPath with AutomationId** - XPath queries using AutomationId attribute
5. **By partial name match** - For localized systems

Common button identifiers tested:
- **Minimize**: "Minimize", "MinimizeButton", "PART_Min"
- **Maximize**: "Maximize", "MaximizeButton", "PART_Max", "Restore"
- **Close**: "Close", "CloseButton", "PART_Close"

## Platform Support

- **Windows**: Full support via Windows Application Driver
- **Other platforms**: Tests are skipped with `Assert.Ignore()`

## Safety Features

- **Close button**: Only tests accessibility, never actually clicks (to avoid closing the test app)
- **Minimize button**: By default only tests accessibility (can be enabled to click)
- **Maximize button**: Safe to click as it just toggles window state

## Use Cases

1. **TitleBar Issues**: Test that system buttons remain responsive when TitleBar visibility changes
2. **Window State Testing**: Verify maximize/restore functionality works correctly
3. **Accessibility Validation**: Ensure system buttons are always accessible to users
4. **Regression Testing**: Catch issues where system buttons become unresponsive

## Integration with Existing Tests

You can easily add system button validation to existing Windows tests:

```csharp
[Test]
public void ExistingWindowTest()
{
    // ... your existing test code ...
    
    // Add system button validation
    var systemButtonResults = App.TestAllSystemButtons();
    Assert.That(systemButtonResults.Values.Any(v => v), Is.True, 
               "System buttons should remain accessible");
}
```

## Debugging

Use the `DiscoverSystemButtons()` method to see what buttons are available:

```csharp
var buttons = App.DiscoverSystemButtons();
foreach (var button in buttons)
{
    Console.WriteLine($"Found button: {button}");
}
```

This will log all discovered buttons with their properties, helping you identify the correct accessibility identifiers for system buttons on different Windows configurations.