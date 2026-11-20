using System.Runtime.CompilerServices;
using Android.Graphics.Drawables;
using Google.Android.Material.ProgressIndicator;
using Microsoft.Maui.Graphics;
using AProgressBar = Android.Widget.ProgressBar;
using ColorFilter = Android.Graphics.ColorFilter;
using PorterDuff = Android.Graphics.PorterDuff;

namespace Microsoft.Maui.Platform
{
	public static class ProgressBarExtensions
	{
		static readonly ConditionalWeakTable<Drawable, ProgressColorState> s_progressColorStates = new();

		public const int Maximum = 10000;

		public static void UpdateProgress(this AProgressBar platformProgressBar, IProgress progress)
		{
			platformProgressBar.Progress = (int)(progress.Progress * Maximum);
		}

		public static void UpdateProgressColor(this AProgressBar platformProgressBar, IProgress progress)
		{
			if (platformProgressBar is LinearProgressIndicator materialProgressBar)
			{
				materialProgressBar.UpdateLinearProgressIndicatorColor(progress);
			}
			else
			{
				platformProgressBar.UpdateProgressBarColor(progress);
			}
		}

		static void UpdateProgressBarColor(this AProgressBar platformProgressBar, IProgress progress)
		{
			var drawable = platformProgressBar.Indeterminate
				? platformProgressBar.IndeterminateDrawable
				: platformProgressBar.ProgressDrawable;

			if (!platformProgressBar.Indeterminate && drawable is LayerDrawable layers)
			{
				drawable = layers.FindDrawableByLayerId(global::Android.Resource.Id.Progress) ?? drawable;
			}

			if (drawable is null)
				return;

			if (progress.ProgressColor is { } color)
			{
				drawable.Mutate();
				if (!s_progressColorStates.TryGetValue(drawable, out _))
					s_progressColorStates.Add(drawable, new ProgressColorState(drawable.ColorFilter));

				// An explicit filter takes precedence over tint without replacing the widget's
				// ColorStateList or the drawable's own stateful tint.
				drawable.SetColorFilter(color.ToPlatform(), PorterDuff.Mode.SrcIn!);
			}
			else if (s_progressColorStates.TryGetValue(drawable, out var state))
			{
				drawable.SetColorFilter(state.ColorFilter);
				s_progressColorStates.Remove(drawable);
			}
		}

		sealed class ProgressColorState(ColorFilter? colorFilter)
		{
			public ColorFilter? ColorFilter { get; } = colorFilter;
		}

		static void UpdateLinearProgressIndicatorColor(this LinearProgressIndicator materialProgressBar, IProgress progress)
		{
			Color color = progress.ProgressColor;

			if (color is null)
			{
				// Reset to theme default by passing empty array - Material3's setIndicatorColor() 
				// automatically resolves this to theme's colorPrimary when length == 0
				materialProgressBar.SetIndicatorColor([]);
			}
			else
			{
				var colorArray = new int[] { color.ToPlatform() };
				materialProgressBar.SetIndicatorColor(colorArray);
			}
		}
	}
}