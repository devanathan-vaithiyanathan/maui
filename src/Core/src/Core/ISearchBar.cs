using System.ComponentModel;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui
{
	/// <summary>
	/// Represents a View used to initiating a search.
	/// </summary>
	public interface ISearchBar : IView, ITextInput, ITextAlignment
	{
		/// <summary>
		/// Gets the color of the cancel button.
		/// </summary>
		Color CancelButtonColor { get; }

		/// <summary>
		/// Gets the color of the Search icon.
		/// </summary>
		Color SearchIconColor { get; }

		/// <summary>
		/// Gets an enumeration value that controls the appearance of the return button.
		/// </summary>
		ReturnType ReturnType { get; }

		/// <summary>
		/// Notify when the user presses the Search button.
		/// </summary>
		void SearchButtonPressed();
	}

	/// <summary>
	/// Provides optional control over the cancel and clear buttons of an <see cref="ISearchBar"/>.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface ISearchBarCancelButtonVisibility
	{
		/// <summary>
		/// Gets whether the platform's cancel and clear buttons can be shown.
		/// </summary>
		/// <remarks>
		/// When <see langword="true"/>, the platform's usual visibility behavior applies.
		/// Search bars that do not implement this interface retain their existing behavior.
		/// </remarks>
		bool ShowCancelButton { get; }
	}

	internal static class SearchBarVisibilityExtensions
	{
		internal static bool AllowsCancelButton(this ISearchBar searchBar) =>
			searchBar is not ISearchBarCancelButtonVisibility { ShowCancelButton: false };
	}
}