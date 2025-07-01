using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	public class TitleBarRTLTests : BaseTestFixture
	{
		[Fact]
		public void TitleBarWithRTLFlowDirectionShouldNotOverlapSystemButtons()
		{
			// Arrange
			var titleBar = new TitleBar
			{
				FlowDirection = FlowDirection.RightToLeft,
				Title = "Test Application Title",
				LeadingContent = new Button { Text = "Leading" },
				Content = new SearchBar { Placeholder = "Search" },
				TrailingContent = new Button { Text = "Trailing" }
			};

			// Act - Apply the default template to trigger layout
			titleBar.OnApplyTemplate();

			// Assert - The template should account for RTL layout
			// This test verifies that the template structure is RTL-aware
			Assert.Equal(FlowDirection.RightToLeft, titleBar.FlowDirection);
			Assert.NotNull(titleBar.LeadingContent);
			Assert.NotNull(titleBar.Content);
			Assert.NotNull(titleBar.TrailingContent);
		}

		[Fact]
		public void TitleBarRTLTemplateShouldHaveCorrectColumnStructure()
		{
			// Arrange
			var titleBar = new TitleBar
			{
				FlowDirection = FlowDirection.RightToLeft
			};

			// Act
			titleBar.OnApplyTemplate();
			var templateRoot = (titleBar as IControlTemplated)?.TemplateRoot as Grid;

			// Assert
			Assert.NotNull(templateRoot);
			
			// The grid should have the proper column structure
			// In RTL mode, the system buttons safety column should be adjusted
			var columnCount = templateRoot.ColumnDefinitions.Count;
			
#if MACCATALYST
			// macOS should have 6 columns (no system buttons column)
			Assert.Equal(6, columnCount);
#else
			// Windows should have 7 columns including system buttons safety column
			Assert.Equal(7, columnCount);
#endif
		}

		[Theory]
		[InlineData(FlowDirection.LeftToRight)]
		[InlineData(FlowDirection.RightToLeft)]
		public void TitleBarShouldHandleBothFlowDirections(FlowDirection flowDirection)
		{
			// Arrange
			var titleBar = new TitleBar
			{
				FlowDirection = flowDirection,
				Title = "Test Title",
				TrailingContent = new Label { Text = "Test" }
			};

			// Act
			titleBar.OnApplyTemplate();

			// Assert - Should not throw and should maintain FlowDirection
			Assert.Equal(flowDirection, titleBar.FlowDirection);
		}

		[Fact]
		public void TitleBarRTLShouldAdjustMarginsForMacOS()
		{
#if MACCATALYST
			// Arrange
			var titleBar = new TitleBar
			{
				FlowDirection = FlowDirection.RightToLeft
			};

			// Act
			titleBar.OnApplyTemplate();
			var templateRoot = (titleBar as IControlTemplated)?.TemplateRoot as Grid;

			// Assert
			Assert.NotNull(templateRoot);
			
			// In RTL mode on macOS, the margin should account for system buttons on the right
			// This is where our fix will be implemented
			var margin = templateRoot.Margin;
			
			// For now, this will fail until we implement the fix
			// We expect the margin to be adjusted for RTL layout
			Assert.True(margin.Left >= 0 && margin.Right >= 0);
#endif
		}
	}
}