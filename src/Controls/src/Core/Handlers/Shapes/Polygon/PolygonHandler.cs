#nullable disable
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.Handlers
{
	public partial class PolygonHandler : ShapeViewHandler
	{
		PointCollection _points;

		public static new IPropertyMapper<Polygon, IShapeViewHandler> Mapper = new PropertyMapper<Polygon, IShapeViewHandler>(ShapeViewHandler.Mapper)
		{
			[nameof(IShapeView.Shape)] = MapShape,
			[nameof(Polygon.Points)] = MapPoints,
			[nameof(Polygon.FillRule)] = MapFillRule,
		};

		public PolygonHandler() : base(Mapper)
		{

		}

		public PolygonHandler(IPropertyMapper mapper) : base(mapper ?? Mapper)
		{

		}

		void UpdatePoints(PointCollection points)
		{
			if (_points == points)
				return;

			var oldPoints = _points;
			oldPoints?.CollectionChanged -= OnPointsCollectionChanged;

			_points = points;

			_points?.CollectionChanged += OnPointsCollectionChanged;
		}

		void OnPointsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (VirtualView is Polygon polygon)
			{
				MapPoints(this, polygon);
			}
		}
	}
}