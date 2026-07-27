using System;

namespace Microsoft.Maui.Controls
{
    /// <summary>Provides data for the <see cref="Switch.Toggling"/> event.</summary>
    public class SwitchTogglingEventArgs : EventArgs
    {
        /// <summary>Creates a new <see cref="SwitchTogglingEventArgs"/> with the specified value.</summary>
        /// <param name="value">The new toggle state being requested.</param>
        public SwitchTogglingEventArgs(bool value)
        {
            Value = value;
        }

        /// <summary>Gets the new toggle state being requested.</summary>
        public bool Value { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the toggle operation should be canceled.
        /// Set to <see langword="true"/> to prevent the <see cref="Switch"/> from changing state.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
