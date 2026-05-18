#nullable disable
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.Handlers
{
	public partial class PolylineHandler : ShapeViewHandler
	{
		PointCollection _points;

		public static new IPropertyMapper<Polyline, IShapeViewHandler> Mapper = new PropertyMapper<Polyline, IShapeViewHandler>(ShapeViewHandler.Mapper)
		{
			[nameof(IShapeView.Shape)] = MapShape,
			[nameof(Polyline.Points)] = MapPoints,
			[nameof(Polyline.FillRule)] = MapFillRule,
		};

		public PolylineHandler() : base(Mapper)
		{

		}

		public PolylineHandler(IPropertyMapper mapper) : base(mapper ?? Mapper)
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
			if (VirtualView is Polyline polyline)
			{
				MapPoints(this, polyline);
			}
		}
	}
}