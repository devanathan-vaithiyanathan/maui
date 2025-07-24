using System;

namespace Microsoft.Maui.Controls
{
	/// <summary>
	/// Provides data for key events.
	/// </summary>
	public class KeyEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KeyEventArgs"/> class.
		/// </summary>
		/// <param name="key">The key that was pressed.</param>
		public KeyEventArgs(string key)
		{
			Key = key;
		}

		/// <summary>
		/// Gets the key that was pressed.
		/// </summary>
		public string Key { get; }
	}
}