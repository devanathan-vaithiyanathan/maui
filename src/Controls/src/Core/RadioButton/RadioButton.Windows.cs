using Microsoft.Maui.Controls.Internals;
using Microsoft.UI.Xaml;
using Windows.Foundation;

namespace Microsoft.Maui.Controls
{
	public partial class RadioButton
	{
		public static void MapContent(RadioButtonHandler handler, RadioButton radioButton)
			=> MapContent((IRadioButtonHandler)handler, radioButton);

		public static void MapContent(IRadioButtonHandler handler, RadioButton radioButton)
		{
			if (radioButton.ResolveControlTemplate() != null)
			{
				handler.PlatformView.Style =
					UI.Xaml.Application.Current.Resources["RadioButtonControlStyle"] as UI.Xaml.Style;
			}
			else
			{
				handler.PlatformView.ClearValue(FrameworkElement.StyleProperty);
			}

			RadioButtonHandler.MapContent(handler, radioButton);

			// Apply TextTransform if needed
			if (radioButton.TextTransform is TextTransform.Lowercase or TextTransform.Uppercase)
			{
				var contentString = radioButton.Content?.ToString();
				if (!string.IsNullOrEmpty(contentString))
				{
					if (handler.PlatformView is Microsoft.UI.Xaml.Controls.RadioButton platformRadioButton)
					{
						var transformedText = TextTransformUtilities.GetTransformedText(contentString, radioButton.TextTransform);
						platformRadioButton.Content = transformedText;
					}
				}
			}
		}
	}
}
