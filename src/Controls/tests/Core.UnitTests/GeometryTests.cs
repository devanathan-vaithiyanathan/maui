using System.Collections.Generic;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Xunit;

namespace Microsoft.Maui.Controls.Core.UnitTests
{

	public class GeometryTests : BaseTestFixture
	{
		[Theory]
		[InlineData(0, true)]
		[InlineData(0, false)]
		[InlineData(45, true)]
		[InlineData(45, false)]
		[InlineData(180, true)]
		[InlineData(180, false)]
		[InlineData(270, true)]
		[InlineData(270, false)]
		public void FlattenArcTest(double angle, bool isLargeArc)
		{
			var path = new Path
			{
				HeightRequest = 200,
				WidthRequest = 200,
				Stroke = Brush.Black
			};

			PathFigure figure = new PathFigure();

			ArcSegment arcSegment = new ArcSegment
			{
				Point = new Point(10, 100),
				Size = new Size(100, 50),
				RotationAngle = angle,
				IsLargeArc = isLargeArc
			};

			figure.Segments.Add(arcSegment);

			path.Data = new PathGeometry
			{
				Figures = new PathFigureCollection
				{
					figure
				}
			};

			List<Point> points = new List<Point>();

			GeometryHelper.FlattenArc(
				points,
				Point.Zero,
				arcSegment.Point,
				arcSegment.Size.Width,
				arcSegment.Size.Height,
				arcSegment.RotationAngle,
				arcSegment.IsLargeArc,
				arcSegment.SweepDirection == SweepDirection.CounterClockwise,
				1);

			Assert.NotEmpty(points);
		}

		[Theory]
		[InlineData(0.01)]
		[InlineData(1)]
		[InlineData(100)]
		public void FlattenSmallArcProducesFiniteEndpoints(double tolerance)
		{
			var start = new Point(17.49219, 16.86133);
			var end = new Point(17.5, 16.86523);
			var points = new List<Point>();

			GeometryHelper.FlattenArc(points, start, end, 1, 1, 0, false, true, tolerance);

			Assert.True(points.Count >= 2);
			Assert.All(points, point =>
			{
				Assert.False(double.IsNaN(point.X) || double.IsInfinity(point.X));
				Assert.False(double.IsNaN(point.Y) || double.IsInfinity(point.Y));
			});
			Assert.Equal(start.X, points[0].X, 6);
			Assert.Equal(start.Y, points[0].Y, 6);
			Assert.Equal(end.X, points[points.Count - 1].X, 6);
			Assert.Equal(end.Y, points[points.Count - 1].Y, 6);
		}

		[Fact]
		public void LucideIconPathProducesFinitePoints()
		{
			const string data =
				"M0 0 z M24 24 z M6.5 5.001953 C5.94841 5.001953 5.355534 5.349387 5.115234 5.925781 " +
				"L1.076172 15.61523 A1 1 0 0 0 1.615234 16.92383 A1 1 0 0 0 2.923828 16.38477 " +
				"L3.917969 14 L9.082031 14 L10.07617 16.38477 A1 1 0 0 0 11.38477 16.92383 " +
				"A1 1 0 0 0 11.92383 15.61523 L7.884766 5.925781 " +
				"C7.644466 5.349387 7.05159 5.001953 6.5 5.001953 z M18 6 A1 1 0 0 0 17 7 " +
				"L17 13.58594 L14.70703 11.29297 A1 1 0 0 0 14 11 A1 1 0 0 0 13.29297 11.29297 " +
				"A1 1 0 0 0 13.29297 12.70703 L17.29297 16.70703 " +
				"A1.0001 1.0001 0 0 0 17.3418 16.75 A1 1 0 0 0 17.38477 16.78711 " +
				"A1.0001 1.0001 0 0 0 17.43945 16.82422 A1 1 0 0 0 17.49219 16.86133 " +
				"A1.0001 1.0001 0 0 0 17.5 16.86523 A1 1 0 0 0 17.61133 16.92188 " +
				"A1.0001 1.0001 0 0 0 17.61719 16.92383 A1 1 0 0 0 17.79492 16.97656 " +
				"A1.0001 1.0001 0 0 0 18.25781 16.96484 A1 1 0 0 0 18.38281 16.92383 " +
				"A1.0001 1.0001 0 0 0 18.56055 16.82422 A1 1 0 0 0 18.61523 16.78711 " +
				"A1.0001 1.0001 0 0 0 18.6582 16.75 A1 1 0 0 0 18.7168 16.69727 " +
				"L22.70703 12.70703 A1 1 0 0 0 22.70703 11.29297 A1 1 0 0 0 21.29297 11.29297 " +
				"L19 13.58594 L19 7 A1 1 0 0 0 18 6 z M6.5 7.802734 L8.248047 12 " +
				"L4.751953 12 L6.5 7.802734 z";
			var geometry = Assert.IsType<PathGeometry>(new PathGeometryConverter().ConvertFromInvariantString(data));
			var shape = new Path { Data = geometry };

			var path = shape.GetPath();

			Assert.NotEmpty(path.Points);
			Assert.All(path.Points, point =>
			{
				Assert.False(float.IsNaN(point.X) || float.IsInfinity(point.X));
				Assert.False(float.IsNaN(point.Y) || float.IsInfinity(point.Y));
			});
			Assert.Equal(new PointF(6.5f, 7.802734f), path.LastPoint);
		}

		[Fact]
		public void TestRoundLineGeometryConstruction()
		{
			var lineGeometry = new LineGeometry(new Point(0, 0), new Point(100, 100));

			Assert.NotNull(lineGeometry);
			Assert.Equal(0, lineGeometry.StartPoint.X);
			Assert.Equal(0, lineGeometry.StartPoint.Y);
			Assert.Equal(100, lineGeometry.EndPoint.X);
			Assert.Equal(100, lineGeometry.EndPoint.Y);
		}

		[Fact]
		public void TestEllipseGeometryConstruction()
		{
			var ellipseGeometry = new EllipseGeometry(new Point(50, 50), 10, 20);

			Assert.NotNull(ellipseGeometry);
			Assert.Equal(50, ellipseGeometry.Center.X);
			Assert.Equal(50, ellipseGeometry.Center.Y);
			Assert.Equal(10, ellipseGeometry.RadiusX);
			Assert.Equal(20, ellipseGeometry.RadiusY);
		}

		[Fact]
		public void TestRectangleGeometryConstruction()
		{
			var rectangleGeometry = new RectangleGeometry(new Rect(0, 0, 150, 150));

			Assert.NotNull(rectangleGeometry);
			Assert.Equal(150, rectangleGeometry.Rect.Height);
			Assert.Equal(150, rectangleGeometry.Rect.Width);
		}

		[Fact]
		public void TestRoundRectangleGeometryConstruction()
		{
			var roundRectangleGeometry = new RoundRectangleGeometry(new CornerRadius(12, 0, 0, 12), new Rect(0, 0, 150, 150));

			Assert.NotNull(roundRectangleGeometry);
			Assert.Equal(12, roundRectangleGeometry.CornerRadius.TopLeft);
			Assert.Equal(0, roundRectangleGeometry.CornerRadius.TopRight);
			Assert.Equal(0, roundRectangleGeometry.CornerRadius.BottomLeft);
			Assert.Equal(12, roundRectangleGeometry.CornerRadius.BottomRight);
			Assert.Equal(150, roundRectangleGeometry.Rect.Height);
			Assert.Equal(150, roundRectangleGeometry.Rect.Width);
		}
	}
}
