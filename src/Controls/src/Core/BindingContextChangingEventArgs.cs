#nullable disable

namespace Microsoft.Maui.Controls
{
    /// <summary>
    /// Provides data for a binding context that is about to change.
    /// </summary>
    public class BindingContextChangingEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingContextChangingEventArgs"/> class.
        /// </summary>
        /// <param name="oldValue">The current binding context.</param>
        /// <param name="newValue">The incoming binding context.</param>
        public BindingContextChangingEventArgs(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Gets the current binding context.
        /// </summary>
        public object OldValue { get; }

        /// <summary>
        /// Gets the incoming binding context.
        /// </summary>
        public object NewValue { get; }
    }
}