using System.Runtime.CompilerServices;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml.Controls;

namespace Microsoft.Maui.Platform
{
	public static class ProgressBarExtensions
	{
		static readonly ConditionalWeakTable<ProgressBar, StrongBox<object>> s_originalForegrounds = new();

		public static void UpdateProgress(this ProgressBar platformProgressBar, IProgress progress)
		{
			platformProgressBar.Value = progress.Progress;
		}

		public static void UpdateProgressColor(this ProgressBar platformProgressBar, IProgress progress)
		{
			Color progressColor = progress.ProgressColor;
			if (progressColor is not null)
			{
				if (!s_originalForegrounds.TryGetValue(platformProgressBar, out _))
					s_originalForegrounds.Add(platformProgressBar, new(platformProgressBar.ReadLocalValue(ProgressBar.ForegroundProperty)));

				platformProgressBar.Foreground = progressColor.ToPlatform();
			}
			else if (s_originalForegrounds.TryGetValue(platformProgressBar, out var originalForeground))
			{
				if (originalForeground.Value == Microsoft.UI.Xaml.DependencyProperty.UnsetValue)
					platformProgressBar.ClearValue(ProgressBar.ForegroundProperty);
				else
					platformProgressBar.SetValue(ProgressBar.ForegroundProperty, originalForeground.Value);

				s_originalForegrounds.Remove(platformProgressBar);
			}
		}
	}
}