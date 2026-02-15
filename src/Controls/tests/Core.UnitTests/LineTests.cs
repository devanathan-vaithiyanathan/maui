using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	public class LineTests : BaseTestFixture
	{
		[Fact]
		public void XPointCanBeSetFromStyle()
		{
			var line = new Line();

			Assert.Equal(0.0, line.X1);
			line.SetValue(Line.X1Property, 1.0, new SetterSpecificity(SetterSpecificity.StyleImplicit, 0, 0, 0));
			Assert.Equal(1.0, line.X1);

			Assert.Equal(0.0, line.X2);
			line.SetValue(Line.X2Property, 100.0, new SetterSpecificity(SetterSpecificity.StyleImplicit, 0, 0, 0));
			Assert.Equal(100.0, line.X2);
		}

		[Fact]
		public void YPointCanBeSetFromStyle()
		{
			var line = new Line();

			Assert.Equal(0.0, line.Y1);
			line.SetValue(Line.Y1Property, 1.0, new SetterSpecificity(SetterSpecificity.StyleImplicit, 0, 0, 0));
			Assert.Equal(1.0, line.Y1);

			Assert.Equal(0.0, line.Y2);
			line.SetValue(Line.Y2Property, 10.0, new SetterSpecificity(SetterSpecificity.StyleImplicit, 0, 0, 0));
			Assert.Equal(10.0, line.Y2);
		}

		[Fact]
		public void LinePathCoordinatesRemainExactWithThickStroke()
		{
			// Test for the issue where thick strokes cause incorrect coordinate positioning
			var line = new Line()
			{
				X1 = 0,
				Y1 = 0,
				X2 = 100,
				Y2 = 100,
				StrokeThickness = 10
			};

			var path = line.GetPath();

			// The path should maintain exact coordinates regardless of stroke thickness
			Assert.Equal(0f, path.GetPointAtIndex(0).X);
			Assert.Equal(0f, path.GetPointAtIndex(0).Y);
			Assert.Equal(100f, path.GetPointAtIndex(1).X);
			Assert.Equal(100f, path.GetPointAtIndex(1).Y);
		}

		[Fact]
		public void LinePathForBoundsWithThickStrokePreservesCoordinates()
		{
			// Test that PathForBounds doesn't shift line coordinates incorrectly with thick strokes
			var line = new Line()
			{
				X1 = 200,
				Y1 = 0,
				X2 = 100,
				Y2 = 100,
				StrokeThickness = 10
			};

			var bounds = new RectF(0, 0, 200, 200);
			var path = line.PathForBounds(bounds);

			// For lines with Stretch.None (default), coordinates should be preserved
			// The issue manifests as the line being shifted due to stroke thickness adjustment
			var points = new System.Collections.Generic.List<PointF>();
			for (int i = 0; i < path.GetSubPathPointCount(0); i++)
			{
				points.Add(path.GetPointAtIndex(i));
			}

			// The line should go from the specified coordinates
			// These coordinates should not be affected by stroke thickness bounds adjustment
			Assert.Equal(2, points.Count); // MoveTo and LineTo
			Assert.Equal(200f, points[0].X, precision: 1);
			Assert.Equal(0f, points[0].Y, precision: 1);
			Assert.Equal(100f, points[1].X, precision: 1);
			Assert.Equal(100f, points[1].Y, precision: 1);
		}
	}
}