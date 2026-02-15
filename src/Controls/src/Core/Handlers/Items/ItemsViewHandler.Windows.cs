#nullable disable
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Windows.UI.ViewManagement;
using WASDKApp = Microsoft.UI.Xaml.Application;
using WASDKDataTemplate = Microsoft.UI.Xaml.DataTemplate;
using WASDKScrollBarVisibility = Microsoft.UI.Xaml.Controls.ScrollBarVisibility;
using WRect = Windows.Foundation.Rect;
using WVisibility = Microsoft.UI.Xaml.Visibility;

namespace Microsoft.Maui.Controls.Handlers.Items
{
	public abstract partial class ItemsViewHandler<TItemsView> : ViewHandler<TItemsView, ListViewBase> where TItemsView : ItemsView
	{
		protected CollectionViewSource CollectionViewSource;
		ScrollViewer _scrollViewer;
		FrameworkElement _emptyView;
		WASDKScrollBarVisibility? _defaultHorizontalScrollVisibility;
		WASDKScrollBarVisibility? _defaultVerticalScrollVisibility;
		View _formsEmptyView;
		bool _emptyViewDisplayed;
		double _previousHorizontalOffset;
		double _previousVerticalOffset;
		InputPane _inputPane;
		bool _isKeyboardHandlingEnabled = true;
		FrameworkElement _currentFocusedEntry;
		protected ListViewBase ListViewBase => PlatformView;
		protected TItemsView ItemsView => VirtualView;
		protected TItemsView Element => VirtualView;
		protected WASDKDataTemplate ViewTemplate => (WASDKDataTemplate)WASDKApp.Current.Resources["View"];
		protected WASDKDataTemplate ItemsViewTemplate => (WASDKDataTemplate)WASDKApp.Current.Resources["ItemsViewDefaultTemplate"];

		UIElement Control => PlatformView;

		protected abstract IItemsLayout Layout { get; }

		protected override ListViewBase CreatePlatformView()
		{
			return SelectListViewBase();
		}

		protected override void ConnectHandler(ListViewBase platformView)
		{
			base.ConnectHandler(platformView);
			VirtualView.ScrollToRequested += ScrollToRequested;
			FindScrollViewer(ListViewBase);
			SetupKeyboardHandling();
		}

		protected override void DisconnectHandler(ListViewBase platformView)
		{
			VirtualView.ScrollToRequested -= ScrollToRequested;
			CleanUpKeyboardHandling();
			CleanUpCollectionViewSource(platformView);
			_formsEmptyView?.Handler?.DisconnectHandler();
			base.DisconnectHandler(platformView);
		}

		public static void MapItemsSource(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
			handler.UpdateItemsSource();
		}

		public static void MapHorizontalScrollBarVisibility(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
			handler.UpdateHorizontalScrollBarVisibility();
		}

		public static void MapVerticalScrollBarVisibility(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
			handler.UpdateVerticalScrollBarVisibility();
		}

		public static void MapItemTemplate(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
			handler.UpdateItemTemplate();
		}

		public static void MapEmptyView(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
			handler.UpdateEmptyView();
		}

		public static void MapEmptyViewTemplate(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
			handler.UpdateEmptyView();
		}

		public static void MapFlowDirection(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
			handler.PlatformView.UpdateFlowDirection(itemsView);
		}

		public static void MapIsVisible(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
			handler.PlatformView.UpdateVisibility(itemsView);
		}

		public static void MapItemsUpdatingScrollMode(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
			handler.UpdateItemsUpdatingScrollMode();
		}

		void UpdateItemsUpdatingScrollMode()
		{
			if (PlatformView is null || PlatformView.Items is null)
				return;

			if (VirtualView.ItemsUpdatingScrollMode == ItemsUpdatingScrollMode.KeepScrollOffset)
			{
				// The scroll position is maintained when new items are added as the default,
				// so we don't need to watch for data changes
				PlatformView.Items.VectorChanged -= OnItemsVectorChanged;
			}
			else
			{
				PlatformView.Items.VectorChanged -= OnItemsVectorChanged;
				PlatformView.Items.VectorChanged += OnItemsVectorChanged;
			}
		}

		void OnItemsVectorChanged(global::Windows.Foundation.Collections.IObservableVector<object> sender, global::Windows.Foundation.Collections.IVectorChangedEventArgs @event)
		{
			if (VirtualView is null)
				return;

			if (sender is not ItemCollection items)
				return;

			var itemsCount = items.Count;

			if (itemsCount == 0)
				return;

			if (VirtualView.ItemsUpdatingScrollMode == ItemsUpdatingScrollMode.KeepItemsInView)
			{
				var firstItem = items[0];
				// Keeps the first item in the list displayed when new items are added.
				ListViewBase.ScrollIntoView(firstItem);
			}

			if (VirtualView.ItemsUpdatingScrollMode == ItemsUpdatingScrollMode.KeepLastItemInView)
			{
				var lastItem = items[itemsCount - 1];
				// Adjusts the scroll offset to keep the last item in the list displayed when new items are added.
				ListViewBase.ScrollIntoView(lastItem, ScrollIntoViewAlignment.Leading);
			}
		}

		protected abstract ListViewBase SelectListViewBase();

		protected virtual void CleanUpCollectionViewSource()
		{
			CleanUpCollectionViewSource(ListViewBase);
		}

		private void CleanUpCollectionViewSource(ListViewBase platformView)
		{
			if (CollectionViewSource is not null)
			{
				if (CollectionViewSource.Source is IDisposable disposableItemTemplateCollection)
				{
					disposableItemTemplateCollection.Dispose();
				}

				if (CollectionViewSource.Source is INotifyCollectionChanged incc)
				{
					incc.CollectionChanged -= ItemsChanged;
				}

				CollectionViewSource.Source = null;
				CollectionViewSource = null;
			}

			// Remove all children inside the ItemsSource
			if (VirtualView is not null)
			{
				foreach (var item in platformView.GetChildren<ItemContentControl>())
				{
					var element = item.GetVisualElement();
					VirtualView.RemoveLogicalChild(element);
				}
			}

			if (VirtualView?.ItemsSource is null)
			{
				platformView.ItemsSource = null;
				return;
			}
		}

		void ItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateEmptyViewVisibility();
		}

		protected virtual void UpdateEmptyViewVisibility()
		{
			bool isEmpty = (CollectionViewSource?.View?.Count ?? 0) == 0;

			if (isEmpty)
			{
				if (_formsEmptyView != null)
				{
					if (_emptyViewDisplayed)
						ItemsView.RemoveLogicalChild(_formsEmptyView);

					if (ItemsView.EmptyViewTemplate == null)
						ItemsView.AddLogicalChild(_formsEmptyView);
				}

				if (_emptyView != null && ListViewBase is IEmptyView emptyView)
				{
					emptyView.EmptyViewVisibility = WVisibility.Visible;

					if (PlatformView.ActualWidth >= 0 && PlatformView.ActualHeight >= 0)
						_formsEmptyView?.Arrange(new Rect(0, 0, PlatformView.ActualWidth, PlatformView.ActualHeight));
				}

				_emptyViewDisplayed = true;
			}
			else
			{
				if (_emptyViewDisplayed)
				{
					if (_emptyView != null && ListViewBase is IEmptyView emptyView)
						emptyView.EmptyViewVisibility = WVisibility.Collapsed;

					ItemsView.RemoveLogicalChild(_formsEmptyView);
				}

				_emptyViewDisplayed = false;
			}
		}

		protected virtual void UpdateItemsSource()
		{
			if (ListViewBase == null)
			{
				return;
			}

			CleanUpCollectionViewSource();

			if (Element.ItemsSource == null)
			{
				return;
			}

			CollectionViewSource = CreateCollectionViewSource();

			if (CollectionViewSource?.Source is INotifyCollectionChanged incc)
			{
				incc.CollectionChanged += ItemsChanged;
			}

			ListViewBase.ItemsSource = GetCollectionView(CollectionViewSource);

			UpdateEmptyViewVisibility();
		}

		protected virtual void UpdateItemTemplate()
		{
			if (Element == null || ListViewBase == null)
			{
				return;
			}

			ListViewBase.ItemTemplate = Element.ItemTemplate == null ? null : ItemsViewTemplate;

			UpdateItemsSource();
		}

		protected virtual CollectionViewSource CreateCollectionViewSource()
		{
			var itemsSource = Element.ItemsSource;
			var itemTemplate = Element.ItemTemplate;

			if (itemTemplate != null)
			{
				return new CollectionViewSource
				{
					Source = TemplatedItemSourceFactory.Create(itemsSource, itemTemplate, Element, mauiContext: MauiContext),
					IsSourceGrouped = false
				};
			}

			return new CollectionViewSource
			{
				Source = itemsSource,
				IsSourceGrouped = false
			};
		}

		protected virtual ICollectionView GetCollectionView(CollectionViewSource collectionViewSource)
		{
			return collectionViewSource.View;
		}

		protected virtual void UpdateEmptyView()
		{
			if (Element == null || ListViewBase == null)
			{
				return;
			}

			var emptyView = Element.EmptyView;

			if (emptyView == null)
			{
				return;
			}

			switch (emptyView)
			{
				case string text:
					_emptyView = new TextBlock
					{
						HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center,
						VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center,
						Text = text
					};
					break;
				case View view:
					_emptyView = RealizeEmptyView(view);
					break;
				default:
					_emptyView = RealizeEmptyViewTemplate(emptyView, Element.EmptyViewTemplate);
					break;
			}

			(ListViewBase as IEmptyView)?.SetEmptyView(_emptyView, _formsEmptyView);

			UpdateEmptyViewVisibility();
		}

		protected virtual void UpdateItemsLayout()
		{
			ListViewBase.IsSynchronizedWithCurrentItem = false;

			FindScrollViewer(ListViewBase);

			_defaultHorizontalScrollVisibility = null;
			_defaultVerticalScrollVisibility = null;

			UpdateItemTemplate();
			UpdateItemsSource();
			UpdateScrollBarVisibility();
			UpdateEmptyView();
		}

		void FindScrollViewer(ListViewBase listView)
		{
			var scrollViewer = listView.GetFirstDescendant<ScrollViewer>();

			if (scrollViewer != null)
			{
				OnScrollViewerFound(scrollViewer);
				return;
			}

			void ListViewLoaded(object sender, RoutedEventArgs e)
			{
				var lv = (ListViewBase)sender;
				lv.Loaded -= ListViewLoaded;
				FindScrollViewer(listView);
			}

			listView.Loaded += ListViewLoaded;
		}

		internal void UpdateScrollBarVisibility()
		{
			UpdateVerticalScrollBarVisibility();
			UpdateHorizontalScrollBarVisibility();
		}

		void UpdateVerticalScrollBarVisibility()
		{
			if (Element.VerticalScrollBarVisibility != ScrollBarVisibility.Default)
			{
				// If the value is changing to anything other than the default, record the default 
				if (_defaultVerticalScrollVisibility == null)
					_defaultVerticalScrollVisibility = ScrollViewer.GetVerticalScrollBarVisibility(Control);
			}

			if (_defaultVerticalScrollVisibility == null)
			{
				// If the default has never been recorded, then this has never been set to anything but the 
				// default value; there's nothing to do.
				return;
			}

			switch (Element.VerticalScrollBarVisibility)
			{
				case (ScrollBarVisibility.Always):
					ScrollViewer.SetVerticalScrollBarVisibility(Control, WASDKScrollBarVisibility.Visible);
					break;
				case (ScrollBarVisibility.Never):
					ScrollViewer.SetVerticalScrollBarVisibility(Control, WASDKScrollBarVisibility.Hidden);
					break;
				case (ScrollBarVisibility.Default):
					ScrollViewer.SetVerticalScrollBarVisibility(Control, _defaultVerticalScrollVisibility.Value);
					break;
			}
		}

		void UpdateHorizontalScrollBarVisibility()
		{
			if (_defaultHorizontalScrollVisibility == null)
				_defaultHorizontalScrollVisibility = ScrollViewer.GetHorizontalScrollBarVisibility(Control);

			switch (Element.HorizontalScrollBarVisibility)
			{
				case (ScrollBarVisibility.Always):
					ScrollViewer.SetHorizontalScrollBarVisibility(Control, WASDKScrollBarVisibility.Visible);
					break;
				case (ScrollBarVisibility.Never):
					ScrollViewer.SetHorizontalScrollBarVisibility(Control, WASDKScrollBarVisibility.Hidden);
					break;
				case (ScrollBarVisibility.Default):
					ScrollViewer.SetHorizontalScrollBarVisibility(Control, _defaultHorizontalScrollVisibility.Value);
					break;
			}
		}

		protected virtual void OnScrollViewerFound(ScrollViewer scrollViewer)
		{
			if (_scrollViewer == scrollViewer)
			{
				return;
			}

			if (_scrollViewer != null)
			{
				_scrollViewer.ViewChanged -= ScrollViewChanged;
			}

			_scrollViewer = scrollViewer;
			_scrollViewer.ViewChanged += ScrollViewChanged;
		}

		void ScrollViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			HandleScroll(_scrollViewer);
		}

		FrameworkElement RealizeEmptyViewTemplate(object bindingContext, DataTemplate emptyViewTemplate)
		{
			if (emptyViewTemplate == null)
			{
				return new TextBlock
				{
					HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center,
					VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center,
					Text = bindingContext.ToString()
				};
			}

			var template = emptyViewTemplate.SelectDataTemplate(bindingContext, null);
			var view = template.CreateContent() as View;
			view.BindingContext = bindingContext;

			return RealizeEmptyView(view);
		}

		FrameworkElement RealizeEmptyView(View view)
		{
			_formsEmptyView = view ?? throw new ArgumentNullException(nameof(view));

			var handler = view.ToHandler(MauiContext);
			var platformView = handler.ContainerView ?? handler.PlatformView;

			return platformView as FrameworkElement;
		}

		internal void HandleScroll(ScrollViewer scrollViewer)
		{
			var itemsViewScrolledEventArgs = new ItemsViewScrolledEventArgs
			{
				HorizontalOffset = scrollViewer.HorizontalOffset,
				HorizontalDelta = scrollViewer.HorizontalOffset - _previousHorizontalOffset,
				VerticalOffset = scrollViewer.VerticalOffset,
				VerticalDelta = scrollViewer.VerticalOffset - _previousVerticalOffset,
			};

			_previousHorizontalOffset = scrollViewer.HorizontalOffset;
			_previousVerticalOffset = scrollViewer.VerticalOffset;

			var layoutOrientaton = ItemsLayoutOrientation.Vertical;
			bool advancing = true;
			switch (Layout)
			{
				case LinearItemsLayout linearItemsLayout:
					layoutOrientaton = linearItemsLayout.Orientation == ItemsLayoutOrientation.Horizontal ? ItemsLayoutOrientation.Horizontal : ItemsLayoutOrientation.Vertical;
					advancing = itemsViewScrolledEventArgs.HorizontalDelta > 0;
					break;
				case GridItemsLayout gridItemsLayout:
					layoutOrientaton = gridItemsLayout.Orientation == ItemsLayoutOrientation.Horizontal ? ItemsLayoutOrientation.Horizontal : ItemsLayoutOrientation.Vertical;
					advancing = itemsViewScrolledEventArgs.VerticalDelta > 0;
					break;
				default:
					break;
			}

			itemsViewScrolledEventArgs = ComputeVisibleIndexes(itemsViewScrolledEventArgs, layoutOrientaton, advancing);

			Element.SendScrolled(itemsViewScrolledEventArgs);

			var remainingItemsThreshold = Element.RemainingItemsThreshold;
			if (remainingItemsThreshold > -1 &&
				ItemCount - 1 - itemsViewScrolledEventArgs.LastVisibleItemIndex <= remainingItemsThreshold)
			{
				Element.SendRemainingItemsThresholdReached();
			}
		}

		protected virtual ItemsViewScrolledEventArgs ComputeVisibleIndexes(ItemsViewScrolledEventArgs args, ItemsLayoutOrientation orientation, bool advancing)
		{
			var (firstVisibleItemIndex, lastVisibleItemIndex, centerItemIndex) = GetVisibleIndexes(orientation, advancing);

			args.FirstVisibleItemIndex = firstVisibleItemIndex;
			args.CenterItemIndex = centerItemIndex;
			args.LastVisibleItemIndex = lastVisibleItemIndex;

			return args;
		}

		(int firstVisibleItemIndex, int lastVisibleItemIndex, int centerItemIndex) GetVisibleIndexes(ItemsLayoutOrientation itemsLayoutOrientation, bool advancing)
		{
			int firstVisibleItemIndex = -1;
			int lastVisibleItemIndex = -1;

			if (ListViewBase.ItemsPanelRoot is ItemsStackPanel itemsPanel)
			{
				firstVisibleItemIndex = itemsPanel.FirstVisibleIndex;
				lastVisibleItemIndex = itemsPanel.LastVisibleIndex;
			}
			else
			{
				var presenters = ListViewBase.GetChildren<ListViewItemPresenter>();

				if (presenters != null && _scrollViewer != null)
				{
					int count = 0;
					foreach (ListViewItemPresenter presenter in presenters)
					{
						if (IsElementVisibleInContainer(presenter, _scrollViewer, itemsLayoutOrientation))
						{
							if (firstVisibleItemIndex == -1)
								firstVisibleItemIndex = count;

							lastVisibleItemIndex = count;
						}

						count++;
					}
				}
			}

			double center = (lastVisibleItemIndex + firstVisibleItemIndex) / 2.0;
			int centerItemIndex = advancing ? (int)Math.Ceiling(center) : (int)Math.Floor(center);

			return (firstVisibleItemIndex, lastVisibleItemIndex, centerItemIndex);
		}

		bool IsElementVisibleInContainer(FrameworkElement element, FrameworkElement container, ItemsLayoutOrientation itemsLayoutOrientation)
		{
			if (element == null || container == null)
				return false;

			if (element.Visibility != WVisibility.Visible)
				return false;

			var elementBounds = element.TransformToVisual(container).TransformBounds(new WRect(0, 0, element.ActualWidth, element.ActualHeight));
			var containerBounds = new WRect(0, 0, container.ActualWidth, container.ActualHeight);

			switch (itemsLayoutOrientation)
			{
				case ItemsLayoutOrientation.Vertical:
					return elementBounds.Top < containerBounds.Bottom && elementBounds.Bottom > containerBounds.Top;

				default:
					return elementBounds.Left < containerBounds.Right && elementBounds.Right > containerBounds.Left;
			}
			;
		}

		async void ScrollToRequested(object sender, ScrollToRequestEventArgs args)
		{
			await ScrollTo(args);
		}

		protected virtual async Task ScrollTo(ScrollToRequestEventArgs args)
		{
			if (!(Control is ListViewBase list))
			{
				return;
			}

			var item = FindBoundItem(args);

			if (item == null)
			{
				// Item wasn't found in the list, so there's nothing to scroll to
				return;
			}

			if (args.IsAnimated)
			{
				await ScrollHelpers.AnimateToItemAsync(list, item, args.ScrollToPosition);
			}
			else
			{
				await ScrollHelpers.JumpToItemAsync(list, item, args.ScrollToPosition);
			}
		}

		object FindBoundItem(ScrollToRequestEventArgs args)
		{
			if (args.Mode == ScrollToMode.Position)
			{
				if (args.Index >= ItemCount)
				{
					return null;
				}

				if (CollectionViewSource.IsSourceGrouped && args.GroupIndex >= 0)
				{
					if (args.GroupIndex >= CollectionViewSource.View.CollectionGroups.Count)
					{
						return null;
					}

					// CollectionGroups property is of type IObservableVector, but these objects should implement ICollectionViewGroup
					var itemGroup = CollectionViewSource.View.CollectionGroups[args.GroupIndex] as ICollectionViewGroup;
					if (itemGroup != null &&
						args.Index < itemGroup.GroupItems.Count)
					{
						return itemGroup.GroupItems[args.Index];
					}
				}

				return GetItem(args.Index);
			}

			if (Element.ItemTemplate == null)
			{
				return args.Item;
			}

			for (int n = 0; n < ItemCount; n++)
			{
				if (CollectionViewSource.View[n] is ItemTemplateContext pair)
				{
					if (Equals(pair.Item, args.Item))
					{
						return CollectionViewSource.View[n];
					}
				}
			}

			return null;
		}

		protected virtual int ItemCount => CollectionViewSource.View.Count;

		protected virtual object GetItem(int index)
		{
			return CollectionViewSource.View[index];
		}

		void SetupKeyboardHandling()
		{
			if (!_isKeyboardHandlingEnabled)
				return;

			try
			{
				_inputPane = global::Windows.UI.ViewManagement.InputPane.GetForCurrentView();
				if (_inputPane != null)
				{
					_inputPane.Showing += OnInputPaneShowing;
					_inputPane.Hiding += OnInputPaneHiding;
				}

				// Set up focus tracking to detect when Entry controls within the CollectionView get focus
				FocusManager.GotFocus += OnFocusManagerGotFocus;
				FocusManager.LostFocus += OnFocusManagerLostFocus;
			}
			catch (Exception ex)
			{
				// InputPane might not be available in all scenarios (e.g., desktop without touch)
				// Gracefully handle the exception and disable keyboard handling
				_isKeyboardHandlingEnabled = false;
				System.Diagnostics.Debug.WriteLine($"InputPane not available: {ex.Message}");
			}
		}

		void CleanUpKeyboardHandling()
		{
			if (_inputPane != null)
			{
				_inputPane.Showing -= OnInputPaneShowing;
				_inputPane.Hiding -= OnInputPaneHiding;
				_inputPane = null;
			}

			// Clean up focus tracking
			FocusManager.GotFocus -= OnFocusManagerGotFocus;
			FocusManager.LostFocus -= OnFocusManagerLostFocus;
			_currentFocusedEntry = null;
		}

		void OnInputPaneShowing(InputPane sender, InputPaneVisibilityEventArgs args)
		{
			try
			{
				// Use the tracked focused Entry instead of trying to find it at this moment
				if (_currentFocusedEntry == null)
					return;

				// Find the container (ListViewItem) that contains this Entry
				var container = FindParentContainer(_currentFocusedEntry);
				if (container == null)
					return;

				// Schedule the scroll operation for the next UI update cycle
				PlatformView?.DispatcherQueue?.TryEnqueue(() =>
				{
					ScrollToFocusedElement(container, args.OccludedRect.Height);
				});
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error in OnInputPaneShowing: {ex.Message}");
			}
		}

		void OnInputPaneHiding(InputPane sender, InputPaneVisibilityEventArgs args)
		{
			// Optional: Could restore scroll position if needed
			// For now, we'll let the CollectionView maintain its current scroll position
		}

		void OnFocusManagerGotFocus(object sender, FocusManagerGotFocusEventArgs e)
		{
			try
			{
				// Check if the newly focused element is a TextBox (Entry) within our CollectionView
				if (e.NewFocusedElement is TextBox textBox && IsWithinCollectionView(textBox))
				{
					_currentFocusedEntry = textBox;
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error in OnFocusManagerGotFocus: {ex.Message}");
			}
		}

		void OnFocusManagerLostFocus(object sender, FocusManagerLostFocusEventArgs e)
		{
			try
			{
				// Clear the tracked focused entry if it lost focus
				if (e.OldFocusedElement == _currentFocusedEntry)
				{
					_currentFocusedEntry = null;
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error in OnFocusManagerLostFocus: {ex.Message}");
			}
		}

		FrameworkElement FindParentEntry(FrameworkElement element)
		{
			var current = element;
			while (current != null)
			{
				// Check if this is a TextBox (the native control for Entry)
				if (current is TextBox textBox)
				{
					// Verify this TextBox is within our CollectionView
					if (IsWithinCollectionView(textBox))
						return textBox;
				}

				current = current.Parent as FrameworkElement;
			}
			return null;
		}

		FrameworkElement FindParentContainer(FrameworkElement element)
		{
			var current = element;
			while (current != null)
			{
				// Look for ListViewItem or SelectorItem which are the container types
				if (current is ListViewItem || current is SelectorItem)
					return current;

				current = current.Parent as FrameworkElement;
			}
			return null;
		}

		bool IsWithinCollectionView(FrameworkElement element)
		{
			var current = element;
			while (current != null)
			{
				if (current == PlatformView)
					return true;

				current = current.Parent as FrameworkElement;
			}
			return false;
		}

		void ScrollToFocusedElement(FrameworkElement container, double keyboardHeight)
		{
			if (PlatformView == null || _scrollViewer == null || container == null)
				return;

			try
			{
				// Ensure the container is still connected to the visual tree
				if (container.Parent == null)
					return;

				// Get the position of the focused element relative to the CollectionView
				var elementPosition = container.TransformToVisual(PlatformView).TransformPoint(new Windows.Foundation.Point(0, 0));
				var elementBottom = elementPosition.Y + container.ActualHeight;

				// Calculate the visible area height (subtract keyboard height)
				var visibleHeight = PlatformView.ActualHeight - keyboardHeight;

				// Only scroll if the element is actually obscured by the keyboard
				if (elementBottom > visibleHeight)
				{
					// Calculate how much we need to scroll to bring the element into view
					// Add some padding to ensure the element is clearly visible
					var scrollOffset = elementBottom - visibleHeight + 40; // Increased padding for better visibility

					// Ensure we don't scroll beyond the available content
					var maxVerticalOffset = _scrollViewer.ScrollableHeight;
					var newVerticalOffset = Math.Min(_scrollViewer.VerticalOffset + scrollOffset, maxVerticalOffset);

					// Only scroll if we actually need to move
					if (Math.Abs(newVerticalOffset - _scrollViewer.VerticalOffset) > 1)
					{
						_scrollViewer.ChangeView(null, newVerticalOffset, null, false);
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error scrolling to focused element: {ex.Message}");
			}
		}
	}
}
