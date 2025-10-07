# TabbedPage Multiple Selection Bug - Sandbox Sample

This sandbox sample demonstrates a bug in TabbedPage where multiple tabs appear selected simultaneously on Windows.

## Project Structure

This sample includes:
- **MainFlyout**: A FlyoutPage with navigation menu
- **Project**: A TabbedPage with 6 tabs (Activities, Stakeholders, Risks, Requirements, Issues, Reports)
- **Portfolio**: A page with a button to navigate to the Project TabbedPage
- **Archive**: A page with a button to navigate to the Project TabbedPage
- **Tab Content Pages**: Individual content pages for each tab

## How to Reproduce the Bug

1. Build and run the project on Windows
2. The app starts with the MainFlyout showing the Portfolio page
3. Click the "Project" button → The Project TabbedPage appears
4. Click the "Activities" tab to select it
5. Open the Flyout menu (hamburger menu) and choose "Archive"
6. Click the "Archive" button → A new Project TabbedPage appears
7. **BUG**: The new TabbedPage shows both "Stakeholders" and "Risks" tabs as selected, which should not be possible

## Expected Behavior

Only one tab should be selected at a time in a TabbedPage.

## Actual Behavior

Multiple tabs appear selected simultaneously, indicating a bug in the TabbedPage control on Windows platform.

## Based on Sample

This sample is based on the GitHub repository: https://github.com/greatoceansoftware/TabbedPageMultipleSelectedItems

The sample has been adapted to work within the MAUI Controls Sample Sandbox structure while maintaining the same navigation flow that triggers the bug.
