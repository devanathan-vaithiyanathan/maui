using System.Reflection;
using Microsoft.Maui.Controls.Shapes;

namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 35387, "PolygonHandler and PolylineHandler leak when Points is replaced before disconnect", PlatformAffected.All)]
public class Issue35387 : ContentPage
{
	readonly Grid _host = new()
	{
		HeightRequest = 160,
		WidthRequest = 180
	};

	public Issue35387()
	{
		Title = "Issue 35387: Polygon/Polyline Points Leak";
		var resultLabel = new Label { AutomationId = "LeakResultLabel" };
		var runButton = new Button { Text = "Run Leak Test", AutomationId = "RunLeakTestButton" };

		runButton.Clicked += async (s, e) =>
		{
			resultLabel.Text = "Running...";
			await RunLeakTest(resultLabel);
		};

		Content = new StackLayout
		{
			Padding = 20,
			Spacing = 12,
			Children = { runButton, _host, resultLabel }
		};
	}

	async Task RunLeakTest(Label resultLabel)
	{
		var polygonBaselineLeaked = await ProbeStaleSubscriptionAsync(isPolygon: true, replacePoints: false);
		var polygonReplaceLeaked = await ProbeStaleSubscriptionAsync(isPolygon: true, replacePoints: true);
		var polylineBaselineLeaked = await ProbeStaleSubscriptionAsync(isPolygon: false, replacePoints: false);
		var polylineReplaceLeaked = await ProbeStaleSubscriptionAsync(isPolygon: false, replacePoints: true);

		resultLabel.Text = (!polygonBaselineLeaked && !polygonReplaceLeaked && !polylineBaselineLeaked && !polylineReplaceLeaked)
			? "Success: No leak detected"
			: $"Failure: Leak detected (Polygon baseline={polygonBaselineLeaked}, Polygon replace={polygonReplaceLeaked}, Polyline baseline={polylineBaselineLeaked}, Polyline replace={polylineReplaceLeaked})";
	}

	async Task<bool> ProbeStaleSubscriptionAsync(bool isPolygon, bool replacePoints)
	{
		var originalPoints = CreatePoints(0);
		Shape shape = isPolygon
			? new Polygon { Points = originalPoints, StrokeThickness = 2 }
			: new Polyline { Points = originalPoints, StrokeThickness = 2 };

		_host.Children.Add(shape);
		await WaitForHandlerAsync(shape);

		var handler = shape.Handler;
		if (handler is null)
			throw new InvalidOperationException("Handler was not created.");

		if (replacePoints)
			SetPoints(shape, CreatePoints(20));

		_host.Children.Remove(shape);
		handler.DisconnectHandler();
		shape.Handler = null;

		return ContainsSubscriptionTarget(originalPoints, handler);
	}

	static bool ContainsSubscriptionTarget(PointCollection points, object target)
	{
		var field = GetCollectionChangedField(points.GetType());
		if (field?.GetValue(points) is not MulticastDelegate multicast)
			return false;

		return multicast.GetInvocationList().Any(d => ReferenceEquals(d.Target, target));
	}

	static FieldInfo GetCollectionChangedField(Type type)
	{
		while (type is not null)
		{
			var field = type.GetField("CollectionChanged", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
			if (field is not null)
				return field;

			type = type.BaseType;
		}

		return null;
	}

	async Task WaitForHandlerAsync(VisualElement element)
	{
		for (var i = 0; i < 40; i++)
		{
			if (element.Handler?.PlatformView is not null)
				return;

			await Task.Delay(50);
		}

		throw new TimeoutException($"{element.GetType().Name} did not receive a handler.");
	}

	static PointCollection CreatePoints(double offset)
	{
		return new PointCollection
			{
				new Point(10 + offset, 10),
				new Point(100 + offset, 20),
				new Point(75 + offset, 80),
				new Point(15 + offset, 70)
			};
	}

	static void SetPoints(Shape shape, PointCollection points)
	{
		switch (shape)
		{
			case Polygon polygon:
				polygon.Points = points;
				break;
			case Polyline polyline:
				polyline.Points = points;
				break;
			default:
				throw new ArgumentException($"Unsupported shape: {shape.GetType().Name}", nameof(shape));
		}
	}
}
