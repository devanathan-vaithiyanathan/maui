#nullable disable
using System;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Controls.Handlers.Items;
using Microsoft.Maui.Graphics;
using ObjCRuntime;
using UIKit;

namespace Microsoft.Maui.Controls.Handlers.Items2
{
	public class StructuredItemsViewController2<TItemsView> : ItemsViewController2<TItemsView>
		where TItemsView : StructuredItemsView
	{
		public const int HeaderTag = 111;
		public const int FooterTag = 222;

		bool _disposed;

		UIView _headerUIView;
		VisualElement _headerViewFormsElement;

		UIView _footerUIView;
		VisualElement _footerViewFormsElement;

		public StructuredItemsViewController2(TItemsView structuredItemsView, UICollectionViewLayout layout)
			: base(structuredItemsView, layout)
		{
		}

		protected override void RegisterViewTypes()
		{
			base.RegisterViewTypes();
			
			RegisterSupplementaryViews(UICollectionElementKindSection.Header);
			RegisterSupplementaryViews(UICollectionElementKindSection.Footer);
		}

		internal override void Disconnect()
		{
			base.Disconnect();

			if (_headerViewFormsElement is not null)
			{
				_headerViewFormsElement.MeasureInvalidated -= OnFormsElementMeasureInvalidated;
			}

			if (_footerViewFormsElement is not null)
			{
				_footerViewFormsElement.MeasureInvalidated -= OnFormsElementMeasureInvalidated;
			}

			_headerUIView = null;
			_headerViewFormsElement = null;
			_footerUIView = null;
			_footerViewFormsElement = null;
		}

#pragma warning disable RS0016 // Add public types and members to the declared API
		protected void OnFormsElementMeasureInvalidated(object sender, EventArgs e)
#pragma warning restore RS0016 // Add public types and members to the declared API
		{
			if (sender is VisualElement formsElement)
			{
				HandleFormsElementMeasureInvalidated(formsElement);
			}
		}

		

#pragma warning disable RS0016 // Add public types and members to the declared API
		protected virtual void HandleFormsElementMeasureInvalidated(VisualElement formsElement)
#pragma warning restore RS0016 // Add public types and members to the declared API
		{
			RemeasureLayout(formsElement);
		}

#pragma warning disable RS0016 // Add public types and members to the declared API
		protected void RemeasureLayout(VisualElement formsElement)
#pragma warning restore RS0016 // Add public types and members to the declared API
		{
			if (IsHorizontal)
			{
				var request = formsElement.Measure(double.PositiveInfinity, CollectionView.Frame.Height);
				formsElement.Arrange(new Rect(0, 0, request.Width, CollectionView.Frame.Height));
			}
			else
			{
				var request = formsElement.Measure(CollectionView.Frame.Width, double.PositiveInfinity);
				formsElement.Arrange(new Rect(0, 0, CollectionView.Frame.Width, request.Height));
			}
		}

		internal void UpdateFooterView()
		{
			UpdateSubview(ItemsView?.Footer, ItemsView?.FooterTemplate, FooterTag,
				ref _footerUIView, ref _footerViewFormsElement);
			UpdateHeaderFooterPosition();
		}

		internal void UpdateHeaderView()
		{
			UpdateSubview(ItemsView?.Header, ItemsView?.HeaderTemplate, HeaderTag,
				ref _headerUIView, ref _headerViewFormsElement);
			UpdateHeaderFooterPosition();
		}


		internal void UpdateSubview(object view, DataTemplate viewTemplate, nint viewTag, ref UIView uiView, ref VisualElement formsElement)
		{
			uiView?.RemoveFromSuperview();

			if (formsElement != null)
			{
				ItemsView.RemoveLogicalChild(formsElement);
				formsElement.MeasureInvalidated -= OnFormsElementMeasureInvalidated;
			}

			UpdateView(view, viewTemplate, ref uiView, ref formsElement);

			if (uiView != null)
			{
				uiView.Tag = viewTag;
				CollectionView.AddSubview(uiView);
			}

			if (formsElement != null)
			{
				ItemsView.AddLogicalChild(formsElement);
			}

			if (formsElement != null)
			{
				RemeasureLayout(formsElement);
				formsElement.MeasureInvalidated += OnFormsElementMeasureInvalidated;
			}
			else
			{
				uiView?.SizeToFit();
			}
		}

		void UpdateHeaderFooterPosition()
		{
			var emptyView = CollectionView.ViewWithTag(EmptyTag);

			if (IsHorizontal)
			{
				var currentInset = CollectionView.ContentInset;

				nfloat headerWidth = ((ItemsView?.Header is View) ? _headerViewFormsElement?.ToPlatform() : _headerUIView)?.Frame.Width ?? 0f;
				nfloat footerWidth = ((ItemsView?.Footer is View) ? _footerViewFormsElement?.ToPlatform() : _footerUIView)?.Frame.Width ?? 0f;
				nfloat emptyWidth = emptyView?.Frame.Width ?? 0f; 

				if (_headerUIView != null )
				{
					_headerUIView.Frame = new CoreGraphics.CGRect(-headerWidth, 0, headerWidth, CollectionView.Frame.Height);
				}

				if (_footerUIView != null && (_footerUIView.Frame.X != ItemsViewLayout.CollectionViewContentSize.Width || emptyWidth > 0))
					_footerUIView.Frame = new CoreGraphics.CGRect(ItemsViewLayout.CollectionViewContentSize.Width + emptyWidth, 0, footerWidth, CollectionView.Frame.Height);

				if (true)
				{
					var currentOffset = CollectionView.ContentOffset;
					CollectionView.ContentInset = new UIEdgeInsets(0, headerWidth, 0, footerWidth);

					var xOffset = currentOffset.X + (currentInset.Left - CollectionView.ContentInset.Left);

					if (CollectionView.ContentSize.Width + headerWidth <= CollectionView.Bounds.Width)
						xOffset = -headerWidth;

					CollectionView.ContentOffset = new CoreGraphics.CGPoint(xOffset, CollectionView.ContentOffset.Y);
				}
			}
			else
			{
				var currentInset = CollectionView.ContentInset;
				nfloat headerHeight = ((ItemsView?.Header is View) ? _headerViewFormsElement?.ToPlatform() : _headerUIView)?.Frame.Height ?? 0f;
				nfloat footerHeight = ((ItemsView?.Footer is View) ? _footerViewFormsElement?.ToPlatform() : _footerUIView)?.Frame.Height ?? 0f;
				nfloat emptyHeight = emptyView?.Frame.Height ?? 0f;

				if (CollectionView.ContentInset.Top != headerHeight || CollectionView.ContentInset.Bottom != footerHeight)
				{
					var currentOffset = CollectionView.ContentOffset;
					CollectionView.ContentInset = new UIEdgeInsets(headerHeight, 0, footerHeight, 0);

					var yOffset = currentOffset.Y + (currentInset.Top - CollectionView.ContentInset.Top);

					if (CollectionView.ContentSize.Height + headerHeight <= CollectionView.Bounds.Height)
						yOffset = -headerHeight;

					if (currentOffset.Y.Value < headerHeight)
					{
						CollectionView.ContentOffset = new CoreGraphics.CGPoint(CollectionView.ContentOffset.X, yOffset);
					}
				}

				if (_headerUIView != null && _headerUIView.Frame.Y != headerHeight)
				{
					_headerUIView.Frame = new CoreGraphics.CGRect(0, -headerHeight, CollectionView.Frame.Width, headerHeight);
				}

				nfloat height = 0;

				if (IsViewLoaded && View.Window != null)
				{
					height = ItemsViewLayout.CollectionViewContentSize.Height;
				}

				if (_footerUIView != null && (_footerUIView.Frame.Y != height || emptyHeight > 0 || _footerUIView.Frame.Height != footerHeight))
				{
					_footerUIView.Frame = new CoreGraphics.CGRect(0, height + emptyHeight, CollectionView.Frame.Width, footerHeight);
				}
			}
		}

		
		protected override void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			_disposed = true;

			if (disposing)
			{

			}

			base.Dispose(disposing);
		}

		protected override bool IsHorizontal => (ItemsView?.ItemsLayout as ItemsLayout)?.Orientation == ItemsLayoutOrientation.Horizontal;

		public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
		{
			// We don't have a header or footer, so we don't need to do anything
			if (ItemsView.Header is null && ItemsView.Footer is null && ItemsView.HeaderTemplate is null && ItemsView.FooterTemplate is null)
			{
				return null;
			}

			var reuseId = DetermineViewReuseId(elementKind);

			var view = collectionView.DequeueReusableSupplementaryView(elementKind, reuseId, indexPath) as UICollectionReusableView;

			switch (view)
			{
				case DefaultCell2 defaultCell:
					UpdateDefaultSupplementaryView(defaultCell, elementKind);
					break;
				case TemplatedCell2 templatedCell:
					UpdateTemplatedSupplementaryView(templatedCell, elementKind);
					break;
			}

			return view;
		}

		private protected virtual void RegisterSupplementaryViews(UICollectionElementKindSection kind)
		{
			if (IsHorizontal)
			{
				CollectionView.RegisterClassForSupplementaryView(typeof(HorizontalSupplementaryView2),
					kind, HorizontalSupplementaryView2.ReuseId);
				CollectionView.RegisterClassForSupplementaryView(typeof(HorizontalDefaultSupplementalView2),
					kind, HorizontalDefaultSupplementalView2.ReuseId);
			}
			else
			{
				CollectionView.RegisterClassForSupplementaryView(typeof(VerticalSupplementaryView2),
					kind, VerticalSupplementaryView2.ReuseId);
				CollectionView.RegisterClassForSupplementaryView(typeof(VerticalDefaultSupplementalView2),
					kind, VerticalDefaultSupplementalView2.ReuseId);
			}
		}

		string DetermineViewReuseId(NSString elementKind)
		{
			return DetermineViewReuseId(elementKind == UICollectionElementKindSectionKey.Header
				? ItemsView.HeaderTemplate
				: ItemsView.FooterTemplate, elementKind == UICollectionElementKindSectionKey.Header
				? ItemsView.Header
				: ItemsView.Footer);
		}

		void UpdateDefaultSupplementaryView(DefaultCell2 cell, NSString elementKind)
		{
			var obj = elementKind == UICollectionElementKindSectionKey.Header
				? ItemsView.Header
				: ItemsView.Footer;

			cell.Label.Text = obj?.ToString();
		}

		void UpdateTemplatedSupplementaryView(TemplatedCell2 cell, NSString elementKind)
		{
			bool isHeader = elementKind == UICollectionElementKindSectionKey.Header;

			if (isHeader)
			{
				if (ItemsView.Header is View headerView)
				{
					cell.Bind(headerView, ItemsView);
				}
				else if (ItemsView.HeaderTemplate is not null)
				{
					cell.Bind(ItemsView.HeaderTemplate, ItemsView.Header, ItemsView);
				}
				cell.Tag = HeaderTag;
			}
			else
			{
				if (ItemsView.Footer is View footerView)
				{
					cell.Bind(footerView, ItemsView);
				}
				else if (ItemsView.FooterTemplate is not null)
				{
					cell.Bind(ItemsView.FooterTemplate, ItemsView.Footer, ItemsView);
				}
				cell.Tag = FooterTag;
			}
		}

		string DetermineViewReuseId(DataTemplate template, object item)
		{
			if (template == null)
			{
				if (item is View)
				{
					// No template, but we can fall back to the view
					return IsHorizontal
						? HorizontalSupplementaryView2.ReuseId
						: VerticalSupplementaryView2.ReuseId;
				}
				// No template, no item, fall back to the default supplemental views
				return IsHorizontal
					? HorizontalDefaultSupplementalView2.ReuseId
					: VerticalDefaultSupplementalView2.ReuseId;
			}

			return IsHorizontal
				? HorizontalSupplementaryView2.ReuseId
				: VerticalSupplementaryView2.ReuseId;
		}

		protected override CGRect DetermineEmptyViewFrame()
		{
			nfloat headerHeight = 0;
			var headerView = CollectionView.ViewWithTag(HeaderTag);

			if (headerView != null)
				headerHeight = headerView.Frame.Height;

			nfloat footerHeight = 0;
			var footerView = CollectionView.ViewWithTag(FooterTag);

			if (footerView != null)
				footerHeight = footerView.Frame.Height;

			return new CGRect(CollectionView.Frame.X, CollectionView.Frame.Y, CollectionView.Frame.Width,
				Math.Abs(CollectionView.Frame.Height - (headerHeight + footerHeight)));
		}

		public override void ViewWillLayoutSubviews()
		{
			base.ViewWillLayoutSubviews();

			if (_footerUIView != null)
			{
				var emptyView = CollectionView.ViewWithTag(EmptyTag);

				if (IsHorizontal)
				{
					if (_footerUIView.Frame.X != ItemsViewLayout.CollectionViewContentSize.Width ||
						_footerUIView.Frame.X < emptyView?.Frame.X)
						UpdateHeaderFooterPosition();
				}
				else
				{
					if (_footerUIView.Frame.Y != ItemsViewLayout.CollectionViewContentSize.Height ||
						_footerUIView.Frame.Y < (emptyView?.Frame.Y + emptyView?.Frame.Height))
						UpdateHeaderFooterPosition();
				}
			}
		}
	}
}