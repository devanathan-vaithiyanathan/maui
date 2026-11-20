using System.Runtime.CompilerServices;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml.Controls;

namespace Microsoft.Maui.Platform
{
	public static class ProgressBarExtensions
	{
		static readonly ConditionalWeakTable<ProgressBar, object> s_progressColorOverrides = new();
		static readonly object s_progressColorOverride = new();

		public static void UpdateProgress(this ProgressBar platformProgressBar, IProgress progress)
		{
			platformProgressBar.Value = progress.Progress;
		}

		public static void UpdateProgressColor(this ProgressBar platformProgressBar, IProgress progress)
		{
			Color progressColor = progress.ProgressColor;
			if (progressColor is not null)
			{
				platformProgressBar.Foreground = progressColor.ToPlatform();
				s_progressColorOverrides.GetValue(platformProgressBar, _ => s_progressColorOverride);
			}
			else if (s_progressColorOverrides.TryGetValue(platformProgressBar, out _))
			{
				platformProgressBar.ClearValue(ProgressBar.ForegroundProperty);
				s_progressColorOverrides.Remove(platformProgressBar);
			}
		}
	}
}