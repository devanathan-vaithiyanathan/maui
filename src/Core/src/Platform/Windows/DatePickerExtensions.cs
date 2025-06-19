using System;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml.Controls;
using WBrush = Microsoft.UI.Xaml.Media.Brush;

namespace Microsoft.Maui.Platform
{
	public static class DatePickerExtensions
	{
		public static void UpdateDate(this CalendarDatePicker platformDatePicker, IDatePicker datePicker)
		{
			var date = datePicker.Date;
			platformDatePicker.UpdateDate(date);

			var format = datePicker.Format;
			var dateFormat = format.ToDateFormat();
			var updatedFormat = string.Empty;
			if (datePicker.CharacterSpacing > 0)
			{
				// If the format contains spaces and is a WinUI format, apply character spacing
				updatedFormat = ApplyCharacterSpacingToDateFormat(dateFormat, datePicker.CharacterSpacing);
			}

			platformDatePicker.DateFormat = updatedFormat == string.Empty ? dateFormat : updatedFormat;

			platformDatePicker.UpdateTextColor(datePicker);
		}

		public static void UpdateDate(this CalendarDatePicker platformDatePicker, DateTime dateTime)
		{
			platformDatePicker.Date = dateTime;
		}

		private static string ApplyCharacterSpacingToDateFormat(string dateFormat, double characterSpacing)
		{
			// Convert character spacing to a reasonable space multiplier
			// Since CalendarDatePicker uses format strings, we add spaces between separators
			if (characterSpacing <= 0)
				return dateFormat;

			// Calculate number of spaces to add based on character spacing value
			// Character spacing is typically a small decimal, so we scale it appropriately
			int spaceCount = Math.Max(1, (int)Math.Round(characterSpacing));
			string spacing = new string(' ', spaceCount);

			// Handle common date format separators
			var separators = new[] { "/", "-", ".", " " };
			foreach (var separator in separators)
			{

#pragma warning disable CA1307 // Specify StringComparison for clarity
				if (dateFormat.Contains(separator) || dateFormat == "d")
				{
					// Replace separators with separator + extra spacing
					dateFormat = dateFormat.Replace(separator, separator + spacing);
					break;
				}
			}

			// For WinUI CalendarDatePicker format patterns like "{month.integer(2)}/{day.integer(2)}/{year.full}"
			// Also add spacing after format tokens
			if (dateFormat.Contains('{') && dateFormat.Contains('}'))
			{
				dateFormat = dateFormat.Replace("}", "}" + spacing);
			}
#pragma warning restore CA1307 // Specify StringComparison for clarity


			return dateFormat.TrimEnd(); // Remove trailing spaces
		}

		public static void UpdateMinimumDate(this CalendarDatePicker platformDatePicker, IDatePicker datePicker)
		{
			platformDatePicker.MinDate = datePicker.MinimumDate;
		}

		public static void UpdateMaximumDate(this CalendarDatePicker platformDatePicker, IDatePicker datePicker)
		{
			platformDatePicker.MaxDate = datePicker.MaximumDate;
		}

		public static void UpdateCharacterSpacing(this CalendarDatePicker platformDatePicker, IDatePicker datePicker)
		{
			platformDatePicker.CharacterSpacing = datePicker.CharacterSpacing.ToEm();
		}

		public static void UpdateFont(this CalendarDatePicker platformDatePicker, IDatePicker datePicker, IFontManager fontManager) =>
			platformDatePicker.UpdateFont(datePicker.Font, fontManager);

		public static void UpdateTextColor(this CalendarDatePicker platformDatePicker, IDatePicker datePicker)
		{
			Color textColor = datePicker.TextColor;

			WBrush? brush = textColor?.ToPlatform();

			if (brush is null)
			{
				platformDatePicker.Resources.RemoveKeys(TextColorResourceKeys);
				platformDatePicker.ClearValue(CalendarDatePicker.ForegroundProperty);
			}
			else
			{
				platformDatePicker.Resources.SetValueForAllKey(TextColorResourceKeys, brush);
				platformDatePicker.Foreground = brush;
			}

			platformDatePicker.RefreshThemeResources();
		}

		// ResourceKeys controlling the foreground color of the CalendarDatePicker.
		// https://docs.microsoft.com/en-us/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.controls.calendardatepicker?view=windows-app-sdk-1.1
		static readonly string[] TextColorResourceKeys =
		{
			"CalendarDatePickerTextForeground",
			"CalendarDatePickerTextForegroundPointerOver",
			"CalendarDatePickerTextForegroundPressed",
			"CalendarDatePickerTextForegroundDisabled",
			"CalendarDatePickerTextForegroundSelected",

			// below resource keys are used for the calendar icon
			"CalendarDatePickerCalendarGlyphForeground",
			"CalendarDatePickerCalendarGlyphForegroundPointerOver",
			"CalendarDatePickerCalendarGlyphForegroundPressed",
			"CalendarDatePickerCalendarGlyphForegroundDisabled",
		};

		// TODO NET8 add to public API
		internal static void UpdateBackground(this CalendarDatePicker platformDatePicker, IDatePicker datePicker)
		{
			var brush = datePicker?.Background?.ToPlatform();

			if (brush is null)
			{
				platformDatePicker.Resources.RemoveKeys(BackgroundColorResourceKeys);
				platformDatePicker.ClearValue(CalendarDatePicker.BackgroundProperty);
			}
			else
			{
				platformDatePicker.Resources.SetValueForAllKey(BackgroundColorResourceKeys, brush);
				platformDatePicker.Background = brush;
			}

			platformDatePicker.RefreshThemeResources();
		}

		static readonly string[] BackgroundColorResourceKeys =
		{
			"CalendarDatePickerBackground",
			"CalendarDatePickerBackgroundPointerOver",
			"CalendarDatePickerBackgroundPressed",
			"CalendarDatePickerBackgroundDisabled",
			"CalendarDatePickerBackgroundFocused",
		};
	}
}
