using System;
using System.Linq;
using System.Threading.Tasks;
using Android.Text;
using Android.Text.Method;
using Android.Widget;
using Microsoft.Maui.DeviceTests.Stubs;
using Xunit;
using AColor = Android.Graphics.Color;
using SearchView = AndroidX.AppCompat.Widget.SearchView;

namespace Microsoft.Maui.DeviceTests
{
	public partial class SearchBarHandlerTests
	{
		[Fact(DisplayName = "PlaceholderColor Initializes Correctly")]
		public async Task PlaceholderColorInitializesCorrectly()
		{
			var searchBar = new SearchBarStub()
			{
				Placeholder = "Test",
				PlaceholderColor = Colors.Yellow
			};

			await ValidatePropertyInitValue(searchBar, () => searchBar.PlaceholderColor, GetNativePlaceholderColor, searchBar.PlaceholderColor);
		}

		[Fact(DisplayName = "Horizontal TextAlignment Initializes Correctly")]
		public async Task HorizontalTextAlignmentInitializesCorrectly()
		{
			var xplatHorizontalTextAlignment = TextAlignment.End;

			var searchBarStub = new SearchBarStub()
			{
				Text = "Test",
				HorizontalTextAlignment = xplatHorizontalTextAlignment
			};

			Android.Views.TextAlignment expectedValue = Android.Views.TextAlignment.ViewEnd;

			var values = await GetValueAsync(searchBarStub, (handler) =>
			{
				return new
				{
					ViewValue = searchBarStub.HorizontalTextAlignment,
					PlatformViewValue = GetNativeHorizontalTextAlignment(handler)
				};
			});

			Assert.Equal(xplatHorizontalTextAlignment, values.ViewValue);
			values.PlatformViewValue.AssertHasFlag(expectedValue);
		}

		[Fact(DisplayName = "Vertical TextAlignment Initializes Correctly")]
		public async Task VerticalTextAlignmentInitializesCorrectly()
		{
			var xplatVerticalTextAlignment = TextAlignment.End;

			var searchBarStub = new SearchBarStub()
			{
				Text = "Test",
				VerticalTextAlignment = xplatVerticalTextAlignment
			};

			Android.Views.GravityFlags expectedValue = Android.Views.GravityFlags.Bottom;

			var values = await GetValueAsync(searchBarStub, (handler) =>
			{
				return new
				{
					ViewValue = searchBarStub.VerticalTextAlignment,
					PlatformViewValue = GetNativeVerticalTextAlignment(handler)
				};
			});

			Assert.Equal(xplatVerticalTextAlignment, values.ViewValue);
			values.PlatformViewValue.AssertHasFlag(expectedValue);
		}

		[Fact(DisplayName = "CharacterSpacing Initializes Correctly")]
		public async Task CharacterSpacingInitializesCorrectly()
		{
			var xplatCharacterSpacing = 4;

			var searchBar = new SearchBarStub()
			{
				CharacterSpacing = xplatCharacterSpacing,
				Text = "Test"
			};

			float expectedValue = searchBar.CharacterSpacing.ToEm();

			var values = await GetValueAsync(searchBar, (handler) =>
			{
				return new
				{
					ViewValue = searchBar.CharacterSpacing,
					PlatformViewValue = GetNativeCharacterSpacing(handler)
				};
			});

			Assert.Equal(xplatCharacterSpacing, values.ViewValue);
			Assert.Equal(expectedValue, values.PlatformViewValue, EmCoefficientPrecision);
		}

		[Fact]
		public async Task SearchViewHasEditTextChild()
		{
			await InvokeOnMainThreadAsync(() =>
			{
				var view = new SearchView(MauiContext.Context);

				var editText = view.GetFirstChildOfType<EditText>();

				Assert.NotNull(editText);
			});
		}

		[Fact]
		public async Task SearchBarTakesFullWidthByDefault()
		{
			await InvokeOnMainThreadAsync(async () =>
			{
				var layout = new LayoutStub()
				{
					Width = 500
				};

				var searchbar = new SearchBarStub
				{
					Text = "My Search Term",
				};

				layout.Add(searchbar);

				var layoutHandler = CreateHandler(layout);
				await layoutHandler.PlatformView.AttachAndRun(() =>
				{
					var layoutSize = layout.Measure(double.PositiveInfinity, double.PositiveInfinity);
					var rect = new Rect(0, 0, layoutSize.Width, layoutSize.Height);
					var searchbarSize = searchbar.Measure(rect.Width, rect.Height);

					Assert.Equal(layoutSize.Width, searchbarSize.Width);
				});
			});
		}

		double GetInputFieldHeight(SearchBarHandler searchBarHandler)
		{
			var control = GetNativeSearchBar(searchBarHandler);
			var editText = control.GetChildrenOfType<EditText>().FirstOrDefault();
			return MauiContext.Context.FromPixels((double)editText.MeasuredHeight);
		}


		static SearchView GetNativeSearchBar(SearchBarHandler searchBarHandler) =>
			searchBarHandler.PlatformView;

		string GetNativeText(SearchBarHandler searchBarHandler) =>
			GetNativeSearchBar(searchBarHandler).Query;

		static void SetNativeText(SearchBarHandler searchBarHandler, string text) =>
			GetNativeSearchBar(searchBarHandler).SetQuery(text, false);

		static int GetCursorStartPosition(SearchBarHandler searchBarHandler)
		{
			var control = GetNativeSearchBar(searchBarHandler);
			var editText = control.GetChildrenOfType<EditText>().FirstOrDefault();
			return editText.SelectionStart;
		}

		static void UpdateCursorStartPosition(SearchBarHandler searchBarHandler, int position)
		{
			var control = GetNativeSearchBar(searchBarHandler);
			var editText = control.GetChildrenOfType<EditText>().FirstOrDefault();
			editText.SetSelection(position);
		}

		Color GetNativeTextColor(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().FirstOrDefault();

			if (editText != null)
			{
				int currentTextColorInt = editText.CurrentTextColor;
				AColor currentTextColor = new AColor(currentTextColorInt);
				return currentTextColor.ToColor();
			}

			return Colors.Transparent;
		}

		Android.Views.TextAlignment GetNativeHorizontalTextAlignment(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().First();
			return editText.TextAlignment;
		}

		Android.Views.GravityFlags GetNativeVerticalTextAlignment(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().First();
			return editText.Gravity;
		}

		string GetNativePlaceholder(SearchBarHandler searchBarHandler) =>
			GetNativeSearchBar(searchBarHandler).QueryHint;

		Color GetNativePlaceholderColor(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().FirstOrDefault();

			if (editText != null)
			{
				int currentHintTextColor = editText.CurrentHintTextColor;
				AColor currentPlaceholderColorr = new AColor(currentHintTextColor);
				return currentPlaceholderColorr.ToColor();
			}

			return Colors.Transparent;
		}

		double GetNativeCharacterSpacing(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().FirstOrDefault();

			if (editText != null)
			{
				return editText.LetterSpacing;
			}

			return -1;
		}

		Android.Views.TextAlignment GetNativeTextAlignment(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().First();
			return editText.TextAlignment;
		}

		bool GetNativeIsReadOnly(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().First();

			if (editText is null)
				return false;

			return !editText.Focusable && !editText.FocusableInTouchMode;
		}

		bool GetNativeIsNumericKeyboard(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().First();

			if (editText is null)
				return false;

			var inputTypes = editText.InputType;

			return editText.KeyListener is NumberKeyListener
				&& (inputTypes.HasFlag(InputTypes.NumberFlagDecimal) && inputTypes.HasFlag(InputTypes.ClassNumber) && inputTypes.HasFlag(InputTypes.NumberFlagSigned));
		}

		bool GetNativeIsChatKeyboard(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().First();

			if (editText is null)
				return false;

			var inputTypes = editText.InputType;

			return inputTypes.HasFlag(InputTypes.ClassText) && inputTypes.HasFlag(InputTypes.TextFlagCapSentences) && inputTypes.HasFlag(InputTypes.TextFlagAutoComplete);
		}

		bool GetNativeIsEmailKeyboard(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().First();

			if (editText is null)
				return false;

			var inputTypes = editText.InputType;

			return (inputTypes.HasFlag(InputTypes.ClassText) && inputTypes.HasFlag(InputTypes.TextVariationEmailAddress));
		}

		bool GetNativeIsTelephoneKeyboard(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().First();

			if (editText is null)
				return false;

			var inputTypes = editText.InputType;

			return inputTypes.HasFlag(InputTypes.ClassPhone);
		}

		bool GetNativeIsUrlKeyboard(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().First();

			if (editText is null)
				return false;

			var inputTypes = editText.InputType;

			return inputTypes.HasFlag(InputTypes.ClassText) && inputTypes.HasFlag(InputTypes.TextVariationUri);
		}

		bool GetNativeIsTextKeyboard(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().First();

			if (editText is null)
				return false;

			var inputTypes = editText.InputType;

			return inputTypes.HasFlag(InputTypes.ClassText) && inputTypes.HasFlag(InputTypes.TextFlagCapSentences) && inputTypes.HasFlag(InputTypes.TextFlagAutoComplete);
		}

		bool GetNativeIsTextPredictionEnabled(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().First();

			if (editText is null)
				return false;

			var inputTypes = editText.InputType;

			return inputTypes.HasFlag(InputTypes.TextFlagAutoCorrect);
		}

		bool GetNativeIsSpellCheckEnabled(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().First();

			if (editText is null)
				return false;

			var inputTypes = editText.InputType;

			return !inputTypes.HasFlag(InputTypes.TextFlagNoSuggestions);
		}

		Android.Views.LayoutDirection GetNativeFlowDirection(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			return searchView.LayoutDirection;
		}

		Android.Views.LayoutDirection GetNativeEditTextFlowDirection(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().FirstOrDefault();
			return editText?.LayoutDirection ?? Android.Views.LayoutDirection.Inherit;
		}

		Android.Views.TextDirection GetNativeEditTextTextDirection(SearchBarHandler searchBarHandler)
		{
			var searchView = GetNativeSearchBar(searchBarHandler);
			var editText = searchView.GetChildrenOfType<EditText>().FirstOrDefault();
#pragma warning disable CA1416 // Introduced in API 23
			return editText?.TextDirection ?? Android.Views.TextDirection.Inherit;
#pragma warning restore CA1416
		}

		[Fact(DisplayName = "FlowDirection Initializes Correctly")]
		public async Task FlowDirectionInitializesCorrectly()
		{
			var searchBarStub = new SearchBarStub()
			{
				Text = "Test",
				FlowDirection = FlowDirection.RightToLeft
			};

			var values = await GetValueAsync(searchBarStub, (handler) =>
			{
				return new
				{
					ViewValue = searchBarStub.FlowDirection,
					PlatformViewValue = GetNativeFlowDirection(handler),
					EditTextFlowDirection = GetNativeEditTextFlowDirection(handler)
				};
			});

			Assert.Equal(FlowDirection.RightToLeft, values.ViewValue);
			Assert.Equal(Android.Views.LayoutDirection.Rtl, values.PlatformViewValue);
			// EditText should have the resolved FlowDirection applied directly (not inherit)
			Assert.Equal(Android.Views.LayoutDirection.Rtl, values.EditTextFlowDirection);
		}

		[Fact(DisplayName = "FlowDirection Updates Correctly")]
		public async Task FlowDirectionUpdatesCorrectly()
		{
			var searchBarStub = new SearchBarStub()
			{
				Text = "Test",
				FlowDirection = FlowDirection.LeftToRight
			};

			await ValidatePropertyUpdatesValue(
				searchBarStub,
				nameof(IView.FlowDirection),
				GetNativeFlowDirection,
				FlowDirection.RightToLeft,
				Android.Views.LayoutDirection.Rtl);

			// Also verify EditText flow direction updates
			var handler = CreateHandler(searchBarStub);
			await InvokeOnMainThreadAsync(() =>
			{
				searchBarStub.FlowDirection = FlowDirection.RightToLeft;
				var editTextDirection = GetNativeEditTextFlowDirection(handler);
				// EditText should have the resolved FlowDirection applied directly (not inherit)
				Assert.Equal(Android.Views.LayoutDirection.Rtl, editTextDirection);
			});
		}

		[Fact(DisplayName = "FlowDirection Inherits From Parent Correctly")]
		public async Task FlowDirectionInheritsFromParentCorrectly()
		{
			await InvokeOnMainThreadAsync(() =>
			{
				// Create a SearchBar with default FlowDirection (MatchParent)
				var searchBarStub = new SearchBarStub()
				{
					Text = "Test",
					FlowDirection = FlowDirection.MatchParent
				};

				// Create a parent layout with RTL FlowDirection
				var layoutStub = new LayoutStub()
				{
					FlowDirection = FlowDirection.RightToLeft
				};
				layoutStub.Add(searchBarStub);

				// Create handlers and set up the hierarchy
				var layoutHandler = CreateHandler(layoutStub);
				var searchBarHandler = CreateHandler(searchBarStub);

				// Simulate the parent-child relationship in the platform view hierarchy
				if (layoutHandler.PlatformView is Android.Views.ViewGroup parentView)
				{
					parentView.LayoutDirection = Android.Views.LayoutDirection.Rtl;
					parentView.AddView(searchBarHandler.PlatformView);
				}

				// Verify that SearchBar resolves and applies the RTL direction from parent
				var searchView = GetNativeSearchBar(searchBarHandler);
				var editText = searchView.GetChildrenOfType<EditText>().FirstOrDefault();

				// Both SearchView and EditText should have RTL resolved from parent chain
				Assert.Equal(Android.Views.LayoutDirection.Rtl, searchView.LayoutDirection);
				Assert.Equal(Android.Views.LayoutDirection.Rtl, editText?.LayoutDirection);
			});
		}

		[Fact(DisplayName = "FlowDirection Text Direction Set Correctly")]
		public async Task FlowDirectionTextDirectionSetCorrectly()
		{
			var searchBarStub = new SearchBarStub()
			{
				Text = "Test",
				FlowDirection = FlowDirection.RightToLeft
			};

			var values = await GetValueAsync(searchBarStub, (handler) =>
			{
				return new
				{
					ViewValue = searchBarStub.FlowDirection,
					PlatformViewValue = GetNativeFlowDirection(handler),
					EditTextFlowDirection = GetNativeEditTextFlowDirection(handler),
					EditTextTextDirection = GetNativeEditTextTextDirection(handler)
				};
			});

			Assert.Equal(FlowDirection.RightToLeft, values.ViewValue);
			Assert.Equal(Android.Views.LayoutDirection.Rtl, values.PlatformViewValue);
			Assert.Equal(Android.Views.LayoutDirection.Rtl, values.EditTextFlowDirection);
#pragma warning disable CA1416 // Introduced in API 23
			Assert.Equal(Android.Views.TextDirection.FirstStrongRtl, values.EditTextTextDirection);
#pragma warning restore CA1416
		}


	}
}