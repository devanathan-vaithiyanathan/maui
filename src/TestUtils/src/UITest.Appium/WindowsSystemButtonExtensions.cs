using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using UITest.Core;

namespace UITest.Appium
{
    /// <summary>
    /// Extension methods for testing Windows system buttons (minimize, maximize, close)
    /// </summary>
    public static class WindowsSystemButtonExtensions
    {
        /// <summary>
        /// Tests if the Windows minimize button is accessible and responsive
        /// </summary>
        /// <param name="app">The Windows app instance</param>
        /// <param name="clickButton">Whether to actually click the minimize button (default: false)</param>
        /// <returns>True if the minimize button is accessible and responsive</returns>
        public static bool TestMinimizeButton(this IApp app, bool clickButton = false)
        {
            if (app is not AppiumWindowsApp windowsApp)
                return false;

            var windowsDriver = windowsApp.Driver as WindowsDriver;
            if (windowsDriver == null)
                return false;

            try
            {
                var minimizeButton = FindSystemButton(windowsDriver, "Minimize", "MinimizeButton", "PART_Min");
                
                if (minimizeButton != null && minimizeButton.Displayed && minimizeButton.Enabled)
                {
                    if (clickButton)
                    {
                        minimizeButton.Click();
                    }
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UITest: Error testing minimize button: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Tests if the Windows maximize/restore button is accessible and responsive
        /// </summary>
        /// <param name="app">The Windows app instance</param>
        /// <param name="clickButton">Whether to actually click the maximize button (default: false)</param>
        /// <returns>True if the maximize/restore button is accessible and responsive</returns>
        public static bool TestMaximizeButton(this IApp app, bool clickButton = false)
        {
            if (app is not AppiumWindowsApp windowsApp)
                return false;

            var windowsDriver = windowsApp.Driver as WindowsDriver;
            if (windowsDriver == null)
                return false;

            try
            {
                var maximizeButton = FindSystemButton(windowsDriver, "Maximize", "MaximizeButton", "PART_Max", "Restore");
                
                if (maximizeButton != null && maximizeButton.Displayed && maximizeButton.Enabled)
                {
                    if (clickButton)
                    {
                        maximizeButton.Click();
                    }
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UITest: Error testing maximize button: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Tests if the Windows close button is accessible (does not actually close the app)
        /// </summary>
        /// <param name="app">The Windows app instance</param>
        /// <returns>True if the close button is accessible</returns>
        public static bool TestCloseButtonAccessible(this IApp app)
        {
            if (app is not AppiumWindowsApp windowsApp)
                return false;

            var windowsDriver = windowsApp.Driver as WindowsDriver;
            if (windowsDriver == null)
                return false;

            try
            {
                var closeButton = FindSystemButton(windowsDriver, "Close", "CloseButton", "PART_Close");
                
                return closeButton != null && closeButton.Displayed && closeButton.Enabled;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UITest: Error testing close button accessibility: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Tests all Windows system buttons (minimize, maximize, close) for accessibility
        /// </summary>
        /// <param name="app">The Windows app instance</param>
        /// <returns>A dictionary with the test results for each button</returns>
        public static Dictionary<string, bool> TestAllSystemButtons(this IApp app)
        {
            var results = new Dictionary<string, bool>();

            results["Minimize"] = app.TestMinimizeButton(clickButton: false);
            results["Maximize"] = app.TestMaximizeButton(clickButton: false);
            results["Close"] = app.TestCloseButtonAccessible();

            return results;
        }

        /// <summary>
        /// Discovers and logs all available system buttons in the current window
        /// </summary>
        /// <param name="app">The Windows app instance</param>
        /// <returns>A list of discovered button information</returns>
        public static List<string> DiscoverSystemButtons(this IApp app)
        {
            var discoveredButtons = new List<string>();

            if (app is not AppiumWindowsApp windowsApp)
                return discoveredButtons;

            var windowsDriver = windowsApp.Driver as WindowsDriver;
            if (windowsDriver == null)
                return discoveredButtons;

            try
            {
                // Get all button elements
                var allButtons = windowsDriver.FindElements(By.XPath("//*[@ControlType='ControlType.Button']"));
                
                Console.WriteLine($"UITest: Found {allButtons.Count} button elements");
                
                foreach (var button in allButtons)
                {
                    try
                    {
                        var name = button.GetAttribute("Name");
                        var automationId = button.GetAttribute("AutomationId");
                        var className = button.GetAttribute("ClassName");
                        
                        var buttonInfo = $"Name: '{name}', AutomationId: '{automationId}', ClassName: '{className}'";
                        discoveredButtons.Add(buttonInfo);
                        
                        // Check if this looks like a system button
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (name.Contains("close", StringComparison.OrdinalIgnoreCase) || 
                                name.Contains("minimize", StringComparison.OrdinalIgnoreCase) || 
                                name.Contains("maximize", StringComparison.OrdinalIgnoreCase) || 
                                name.Contains("restore", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine($"UITest: *** SYSTEM BUTTON FOUND: {buttonInfo} ***");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"UITest: Error examining button: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UITest: Error discovering buttons: {ex.Message}");
            }

            return discoveredButtons;
        }

        /// <summary>
        /// Attempts to find a Windows system button using multiple identification strategies
        /// </summary>
        private static IWebElement? FindSystemButton(WindowsDriver driver, params string[] identifiers)
        {
            foreach (var identifier in identifiers)
            {
                try
                {
                    // Strategy 1: By AutomationId/Id
                    try
                    {
                        var element = driver.FindElement(By.Id(identifier));
                        if (element != null)
                            return element;
                    }
                    catch { }

                    // Strategy 2: By Name
                    try
                    {
                        var element = driver.FindElement(By.Name(identifier));
                        if (element != null)
                            return element;
                    }
                    catch { }

                    // Strategy 3: By XPath with Name attribute
                    try
                    {
                        var element = driver.FindElement(By.XPath($"//*[@Name='{identifier}']"));
                        if (element != null)
                            return element;
                    }
                    catch { }

                    // Strategy 4: By XPath with AutomationId attribute
                    try
                    {
                        var element = driver.FindElement(By.XPath($"//*[@AutomationId='{identifier}']"));
                        if (element != null)
                            return element;
                    }
                    catch { }

                    // Strategy 5: By XPath with partial name match (for localized systems)
                    try
                    {
                        var element = driver.FindElement(By.XPath($"//*[contains(@Name, '{identifier}')]"));
                        if (element != null)
                            return element;
                    }
                    catch { }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"UITest: Exception searching for {identifier}: {ex.Message}");
                }
            }

            return null;
        }
    }
}