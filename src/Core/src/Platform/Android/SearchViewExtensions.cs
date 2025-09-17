using System;
using Android.Content.Res;
using Android.Text;
using Android.Views;
using Android.Widget;
using SearchView = AndroidX.AppCompat.Widget.SearchView;
using ATextDirection = Android.Views.TextDirection;

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
			// Get the internal EditText first
			editText ??= searchView.GetFirstChildOfType<EditText>();
			
			// Resolve the effective FlowDirection for the SearchBar
			// This handles both direct assignments and inherited values from parent elements
			var effectiveFlowDirection = GetEffectiveFlowDirection(searchBar);
			
			void UpdateFlowDirectionForViews(EditText et)
			{
				// Apply the resolved FlowDirection to both SearchView and EditText
				// We can't rely on inheritance for SearchBar because we're applying FlowDirection 
				// to the inner EditText, so we need to handle MatchParent scenarios manually
				switch (effectiveFlowDirection)
				{
					case FlowDirection.RightToLeft:
						searchView.LayoutDirection = Android.Views.LayoutDirection.Rtl;
						et.LayoutDirection = Android.Views.LayoutDirection.Rtl;
#pragma warning disable CA1416 // Introduced in API 23
						et.TextDirection = ATextDirection.FirstStrongRtl;
#pragma warning restore CA1416
						break;
					case FlowDirection.LeftToRight:
						searchView.LayoutDirection = Android.Views.LayoutDirection.Ltr;
						et.LayoutDirection = Android.Views.LayoutDirection.Ltr;
#pragma warning disable CA1416 // Introduced in API 23
						et.TextDirection = ATextDirection.FirstStrongLtr;
#pragma warning restore CA1416
						break;
					default: // MatchParent or unspecified - use system default (LTR)
						searchView.LayoutDirection = Android.Views.LayoutDirection.Ltr;
						et.LayoutDirection = Android.Views.LayoutDirection.Ltr;
#pragma warning disable CA1416 // Introduced in API 23
						et.TextDirection = ATextDirection.FirstStrongLtr;
#pragma warning restore CA1416
						break;
				}
			}
			
			if (editText != null)
			{
				UpdateFlowDirectionForViews(editText);
			}
			else
			{
				// If EditText isn't available yet, post a delayed update
				// This can happen during initialization when the SearchView hierarchy isn't fully built
				searchView.Post(() =>
				{
					var delayedEditText = searchView.GetFirstChildOfType<EditText>();
					if (delayedEditText != null)
					{
						UpdateFlowDirectionForViews(delayedEditText);
					}
				});
			}
		}

		private static FlowDirection GetEffectiveFlowDirection(ISearchBar searchBar)
		{
			// If SearchBar has an explicit FlowDirection, use it
			if (searchBar.FlowDirection != FlowDirection.MatchParent)
				return searchBar.FlowDirection;

			// For MatchParent, traverse up the parent chain to find the effective FlowDirection
			var currentView = searchBar.Parent;
			while (currentView != null)
			{
				if (currentView is IView view && view.FlowDirection != FlowDirection.MatchParent)
					return view.FlowDirection;
				currentView = currentView.Parent;
			}

			// Default to LeftToRight if no explicit FlowDirection is found in the hierarchy
			return FlowDirection.LeftToRight;
		}
	}
}