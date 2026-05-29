namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	Repository? _repository;
	CollectionView? _collectionView;
	readonly string _pageProbeId;
	string? _collectionProbeId;

	public MainPage()
	{
		InitializeComponent();
		_pageProbeId = LeakProbe.Track(this, nameof(MainPage));
		Loaded += OnPageLoaded;
		Unloaded += OnPageUnloaded;
	}

	void OnAddCollectionViewClicked(object sender, EventArgs e)
	{
		AddCollectionView();
	}

	void OnRemoveCollectionViewClicked(object sender, EventArgs e)
	{
		RemoveCollectionView();
	}

	void AddCollectionView()
	{
		if (_collectionView is null)
		{
			_collectionView = new CollectionView();
			_collectionProbeId = LeakProbe.Track(_collectionView, nameof(CollectionView));
			HookCollectionViewEvents(_collectionView);

			_repository = new Repository();

			_collectionView.ItemsSource = _repository.OrderCollection;
			_collectionView.ItemTemplate = new DataTemplate(() =>
			{
				var grid = new Grid
				{
					ColumnDefinitions =
					{
						new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
						new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
						new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					}
				};

				var orderIdLabel = new Label();
				orderIdLabel.SetBinding(Label.TextProperty, nameof(Order.OrderID));
				grid.Add(orderIdLabel);
				Grid.SetColumn(orderIdLabel, 0);

				var customerIdLabel = new Label();
				customerIdLabel.SetBinding(Label.TextProperty, nameof(Order.CustomerID));
				grid.Add(customerIdLabel);
				Grid.SetColumn(customerIdLabel, 1);

				var shipCityLabel = new Label();
				shipCityLabel.SetBinding(Label.TextProperty, nameof(Order.ShipCity));
				grid.Add(shipCityLabel);
				Grid.SetColumn(shipCityLabel, 2);

				return grid;
			});

			layout.Children.Add(_collectionView);
			LeakProbe.Mark(_collectionProbeId, "added-to-layout");
		}
	}

	void RemoveCollectionView()
	{
		if (_collectionView is null)
		{
			return;
		}

		LeakProbe.Mark(_collectionProbeId, "remove-clicked");

		_repository?.OrderCollection.Clear();
		_collectionView.ItemsSource = null;
		_collectionView.ItemTemplate = null;
		layout.Children.Remove(_collectionView);
		UnhookCollectionViewEvents(_collectionView);

		if (_collectionView is IDisposable disposableCollectionView)
		{
			disposableCollectionView.Dispose();
			LeakProbe.Mark(_collectionProbeId, "disposed-via-idisposable");
		}
		else
		{
			LeakProbe.Mark(_collectionProbeId, "idisposable-not-supported");
		}

		_collectionView = null;
		_repository = null;
	}

	void HookCollectionViewEvents(CollectionView collectionView)
	{
		collectionView.Loaded += OnCollectionViewLoaded;
		collectionView.Unloaded += OnCollectionViewUnloaded;
		collectionView.HandlerChanging += OnCollectionViewHandlerChanging;
		collectionView.HandlerChanged += OnCollectionViewHandlerChanged;
		collectionView.ParentChanged += OnCollectionViewParentChanged;
	}

	void UnhookCollectionViewEvents(CollectionView collectionView)
	{
		collectionView.Loaded -= OnCollectionViewLoaded;
		collectionView.Unloaded -= OnCollectionViewUnloaded;
		collectionView.HandlerChanging -= OnCollectionViewHandlerChanging;
		collectionView.HandlerChanged -= OnCollectionViewHandlerChanged;
		collectionView.ParentChanged -= OnCollectionViewParentChanged;
	}

	void OnPageLoaded(object? sender, EventArgs e) => LeakProbe.Mark(_pageProbeId, "loaded");

	void OnPageUnloaded(object? sender, EventArgs e) => LeakProbe.Mark(_pageProbeId, "unloaded");

	protected override void OnAppearing()
	{
		base.OnAppearing();
		LeakProbe.Mark(_pageProbeId, "appearing");
	}

	protected override void OnDisappearing()
	{
		LeakProbe.Mark(_pageProbeId, "disappearing");
		base.OnDisappearing();
	}

	void OnCollectionViewLoaded(object? sender, EventArgs e) => LeakProbe.Mark(_collectionProbeId, "cv-loaded");

	void OnCollectionViewUnloaded(object? sender, EventArgs e) => LeakProbe.Mark(_collectionProbeId, "cv-unloaded");

	void OnCollectionViewHandlerChanging(object? sender, HandlerChangingEventArgs e)
	{
		LeakProbe.Mark(_collectionProbeId, $"cv-handler-changing old={(e.OldHandler is null ? "null" : "set")} new={(e.NewHandler is null ? "null" : "set")}");
	}

	void OnCollectionViewHandlerChanged(object? sender, EventArgs e)
	{
		var state = _collectionView?.Handler is null ? "cv-handler-now-null" : "cv-handler-now-set";
		LeakProbe.Mark(_collectionProbeId, state);
	}

	void OnCollectionViewParentChanged(object? sender, EventArgs e)
	{
		var state = _collectionView?.Parent is null ? "cv-parent-null" : "cv-parent-set";
		LeakProbe.Mark(_collectionProbeId, state);
	}

	public async Task SimulateCollectionViewLifecycleAsync()
	{
		AddCollectionView();
		await Task.Delay(90);
		RemoveCollectionView();
		await Task.Delay(90);
	}
}