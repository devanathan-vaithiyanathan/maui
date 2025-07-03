using System;
using Android.Content.Res;
using Android.Text;
using Android.Views;
using Android.Widget;
using SearchView = AndroidX.AppCompat.Widget.SearchView;

namespace Microsoft.Maui.Platform
{
	public static class SearchViewExtensions
	{
		public static void UpdateText(this SearchView searchView, ISearchBar searchBar)
		{
			searchView.SetQuery(searchBar.Text, false);
		}

		public static void UpdatePlaceholder(this SearchView searchView, ISearchBar searchBar)
		{
			searchView.QueryHint = searchBar.Placeholder;
		}

		public static void UpdatePlaceholderColor(this SearchView searchView, ISearchBar searchBar, ColorStateList? defaultPlaceholderColor, EditText? editText = null)
		{
			editText ??= searchView.GetFirstChildOfType<EditText>();

			if (editText == null)
				return;

			var placeholderTextColor = searchBar.PlaceholderColor;

			if (placeholderTextColor == null)
			{
				editText.SetHintTextColor(defaultPlaceholderColor);
			}
			else
			{
				if (PlatformInterop.CreateEditTextColorStateList(editText.HintTextColors, placeholderTextColor.ToPlatform()) is ColorStateList c)
				{
					editText.SetHintTextColor(c);
				}
			}
		}

		public static void UpdateFont(this SearchView searchView, ISearchBar searchBar, IFontManager fontManager, EditText? editText = null)
		{
			editText ??= searchView.GetFirstChildOfType<EditText>();

			if (editText == null)
				return;

			editText.UpdateFont(searchBar, fontManager);
		}

		public static void UpdateVerticalTextAlignment(this SearchView searchView, ISearchBar searchBar)
		{
			searchView.UpdateVerticalTextAlignment(searchBar, null);
		}

		public static void UpdateVerticalTextAlignment(this SearchView searchView, ISearchBar searchBar, EditText? editText)
		{
			editText ??= searchView.GetFirstChildOfType<EditText>();

			if (editText == null)
				return;

			editText.UpdateVerticalAlignment(searchBar.VerticalTextAlignment, TextAlignment.Center.ToVerticalGravityFlags());
		}

		public static void UpdateMaxLength(this SearchView searchView, ISearchBar searchBar)
		{
			searchView.UpdateMaxLength(searchBar.MaxLength, null);
		}

		public static void UpdateMaxLength(this SearchView searchView, ISearchBar searchBar, EditText? editText)
		{
			searchView.UpdateMaxLength(searchBar.MaxLength, editText);
		}

		public static void UpdateMaxLength(this SearchView searchView, int maxLength, EditText? editText)
		{
			editText ??= searchView.GetFirstChildOfType<EditText>();
			editText?.SetLengthFilter(maxLength);

			var query = searchView.Query;
			var trimmedQuery = query.TrimToMaxLength(maxLength);

			if (query != trimmedQuery)
			{
				searchView.SetQuery(trimmedQuery, false);
			}
		}

		public static void UpdateIsReadOnly(this EditText editText, ISearchBar searchBar)
		{
			bool isReadOnly = !searchBar.IsReadOnly;

			editText.FocusableInTouchMode = isReadOnly;
			editText.Focusable = isReadOnly;
			editText.SetCursorVisible(isReadOnly);
		}

		public static void UpdateCancelButtonColor(this SearchView searchView, ISearchBar searchBar)
		{
			if (searchView.Resources == null)
				return;

			var searchCloseButtonIdentifier = Resource.Id.search_close_btn;

			if (searchCloseButtonIdentifier > 0)
			{
				var image = searchView.FindViewById<ImageView>(searchCloseButtonIdentifier);

				if (image != null && image.Drawable != null)
				{
					if (searchBar.CancelButtonColor != null)
						image.Drawable.SetColorFilter(searchBar.CancelButtonColor, FilterMode.SrcIn);
					else
						image.Drawable.ClearColorFilter();
				}
			}
		}

		public static void UpdateIsTextPredictionEnabled(this SearchView searchView, ISearchBar searchBar, EditText? editText = null)
		{
			editText ??= searchView.GetFirstChildOfType<EditText>();

			if (editText == null)
				return;

			if (searchBar.IsTextPredictionEnabled)
				editText.InputType |= InputTypes.TextFlagAutoCorrect;
			else
				editText.InputType &= ~InputTypes.TextFlagAutoCorrect;
		}

		public static void UpdateIsSpellCheckEnabled(this SearchView searchView, ISearchBar searchBar, EditText? editText = null)
		{
			editText ??= searchView.GetFirstChildOfType<EditText>();

			if (editText == null)
				return;

			if (!searchBar.IsSpellCheckEnabled)
				editText.InputType |= InputTypes.TextFlagNoSuggestions;
			else
				editText.InputType &= ~InputTypes.TextFlagNoSuggestions;
		}

		public static void UpdateIsEnabled(this SearchView searchView, ISearchBar searchBar, EditText? editText = null)
		{
			editText ??= searchView.GetFirstChildOfType<EditText>();

			if (editText == null)
				return;

			if (editText != null)
			{
				editText.Enabled = searchBar.IsEnabled;
			}
		}

		public static void UpdateKeyboard(this SearchView searchView, ISearchBar searchBar)
		{
			searchView.SetInputType(searchBar);
		}

		internal static void SetInputType(this SearchView searchView, ISearchBar searchBar, EditText? editText = null)
		{
			editText ??= searchView.GetFirstChildOfType<EditText>();

			if (editText == null)
				return;

			editText.SetInputType(searchBar);
		}

		internal static void UpdateFlowDirection(this SearchView searchView, ISearchBar searchBar, EditText? editText = null)
		{
			editText ??= searchView.GetFirstChildOfType<EditText>();

			// Update the SearchView itself
			searchView.UpdateFlowDirection((IView)searchBar);

			// Update the internal EditText if available
			editText?.UpdateFlowDirection((IView)searchBar);

			// Update icon positioning based on flow direction
			UpdateSearchViewIcons(searchView, searchBar);
		}

		private static void UpdateSearchViewIcons(SearchView searchView, ISearchBar searchBar)
		{
			var flowDirection = ((IView)searchBar).FlowDirection;

			// Handle search (magnifying glass) icon positioning
			UpdateSearchIcon(searchView, flowDirection);

			// Handle close/clear button positioning  
			UpdateCloseButton(searchView, flowDirection);
		}

		private static void UpdateSearchIcon(SearchView searchView, FlowDirection flowDirection)
		{
			// Try to find the search icon using common resource identifiers
			// Note: SearchView's internal structure may vary across Android versions
			try
			{
				// Try to find search icon by traversing the view hierarchy
				var searchIconView = FindSearchIcon(searchView);
				if (searchIconView != null)
				{
					// Update the search icon's layout direction and positioning
					if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBeanMr1)
					{
						var layoutDirection = flowDirection == FlowDirection.RightToLeft
							? Android.Views.LayoutDirection.Rtl
							: Android.Views.LayoutDirection.Ltr;

						searchIconView.LayoutDirection = layoutDirection;
					}
				}
			}
			catch
			{
				// Ignore errors - icon positioning is not critical for basic functionality
			}
		}

		private static ImageView? FindSearchIcon(SearchView searchView)
		{
			// SearchView typically contains an ImageView for the search icon
			// We'll search through the view hierarchy to find it
			return FindImageViewByContentDescription(searchView, "Search") ??
				   FindImageViewByDrawableType(searchView);
		}

		private static ImageView? FindImageViewByContentDescription(ViewGroup parent, string description)
		{
			for (int i = 0; i < parent.ChildCount; i++)
			{
				var child = parent.GetChildAt(i);
				if (child is ImageView imageView &&
					imageView.ContentDescription?.ToString()?.Contains(description, StringComparison.OrdinalIgnoreCase) == true)
				{
					return imageView;
				}
				else if (child is ViewGroup childGroup)
				{
					var result = FindImageViewByContentDescription(childGroup, description);
					if (result != null)
						return result;
				}
			}
			return null;
		}

		private static ImageView? FindImageViewByDrawableType(ViewGroup parent)
		{
			// Look for ImageViews that are likely to be the search icon
			// (typically the first ImageView in a SearchView that's not the close button)
			for (int i = 0; i < parent.ChildCount; i++)
			{
				var child = parent.GetChildAt(i);
				if (child is ImageView imageView && imageView.Id != Resource.Id.search_close_btn)
				{
					return imageView;
				}
				else if (child is ViewGroup childGroup)
				{
					var result = FindImageViewByDrawableType(childGroup);
					if (result != null)
						return result;
				}
			}
			return null;
		}

		private static void UpdateCloseButton(SearchView searchView, FlowDirection flowDirection)
		{
			// Handle close/clear button positioning
			var searchCloseButtonIdentifier = Resource.Id.search_close_btn;
			if (searchCloseButtonIdentifier > 0)
			{
				var closeButton = searchView.FindViewById<ImageView>(searchCloseButtonIdentifier);
				if (closeButton != null)
				{
					// Update the close button's layout direction
					if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBeanMr1)
					{
						var layoutDirection = flowDirection == FlowDirection.RightToLeft
							? Android.Views.LayoutDirection.Rtl
							: Android.Views.LayoutDirection.Ltr;

						closeButton.LayoutDirection = layoutDirection;
					}
				}
			}
		}
	}
}