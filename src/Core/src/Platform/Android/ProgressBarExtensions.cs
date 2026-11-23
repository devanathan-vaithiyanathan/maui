using System.Runtime.CompilerServices;
using Android.Graphics.Drawables;
using Android.Views;
using Google.Android.Material.ProgressIndicator;
using Microsoft.Maui.Graphics;
using AProgressBar = Android.Widget.ProgressBar;
using ColorFilter = Android.Graphics.ColorFilter;
using PorterDuff = Android.Graphics.PorterDuff;
using AView = Android.Views.View;

namespace Microsoft.Maui.Platform
{
	public static class ProgressBarExtensions
	{
		static readonly ConditionalWeakTable<AProgressBar, ProgressColorOverride> s_progressColorOverrides = new();

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
			if (progress.ProgressColor is not { } color)
			{
				platformProgressBar.ResetProgressColor();
				return;
			}

			if (!s_progressColorOverrides.TryGetValue(platformProgressBar, out var colorOverride))
			{
				colorOverride = new ProgressColorOverride(platformProgressBar);
				s_progressColorOverrides.Add(platformProgressBar, colorOverride);
			}

			colorOverride.Update(color);
		}

		internal static void ResetProgressColor(this AProgressBar platformProgressBar)
		{
			if (s_progressColorOverrides.TryGetValue(platformProgressBar, out var colorOverride))
			{
				colorOverride.Stop();
				s_progressColorOverrides.Remove(platformProgressBar);
			}
		}

		sealed class ProgressColorOverride : Java.Lang.Object, ViewTreeObserver.IOnPreDrawListener, AView.IOnAttachStateChangeListener
		{
			readonly AProgressBar _platformProgressBar;
			ViewTreeObserver? _viewTreeObserver;
			Drawable? _sourceDrawable;
			Drawable? _drawable;
			ColorFilter? _originalColorFilter;
			Color? _color;
			Color? _appliedColor;

			public ProgressColorOverride(AProgressBar platformProgressBar)
			{
				_platformProgressBar = platformProgressBar;
				platformProgressBar.AddOnAttachStateChangeListener(this);
				if (platformProgressBar.IsAttachedToWindow)
					OnViewAttachedToWindow(platformProgressBar);
			}

			public void Update(Color color)
			{
				_color = color;
				ApplyColor();
				_platformProgressBar.Invalidate();
			}

			public bool OnPreDraw()
			{
				ApplyColor();
				return true;
			}

			public void OnViewAttachedToWindow(AView view)
			{
				_viewTreeObserver = view.ViewTreeObserver;
				_viewTreeObserver.AddOnPreDrawListener(this);
				ApplyColor();
			}

			public void OnViewDetachedFromWindow(AView view)
			{
				if (_viewTreeObserver?.IsAlive == true)
					_viewTreeObserver.RemoveOnPreDrawListener(this);
				_viewTreeObserver = null;
			}

			void ApplyColor()
			{
				var drawable = _platformProgressBar.Indeterminate
					? _platformProgressBar.IndeterminateDrawable
					: _platformProgressBar.ProgressDrawable;

				if (!_platformProgressBar.Indeterminate && drawable is LayerDrawable layers)
					drawable = layers.FindDrawableByLayerId(global::Android.Resource.Id.Progress) ?? drawable;

				if (!ReferenceEquals(drawable, _sourceDrawable))
				{
					RestoreDrawable();
					_sourceDrawable = drawable;
					_drawable = drawable?.Mutate();
					_originalColorFilter = _drawable?.ColorFilter;
				}

				if (_drawable is null)
					return;

				if (_color is null)
				{
					RestoreDrawable();
					return;
				}

				if (!Equals(_color, _appliedColor))
				{
					_drawable.SetColorFilter(_color.ToPlatform(), PorterDuff.Mode.SrcIn!);
					_appliedColor = _color;
				}
			}

			void RestoreDrawable()
			{
				_drawable?.SetColorFilter(_originalColorFilter);
				_drawable = null;
				_sourceDrawable = null;
				_originalColorFilter = null;
				_appliedColor = null;
			}

			public void Stop()
			{
				_platformProgressBar.RemoveOnAttachStateChangeListener(this);
				if (_viewTreeObserver?.IsAlive == true)
					_viewTreeObserver.RemoveOnPreDrawListener(this);
				_viewTreeObserver = null;
				ApplyColor();
				RestoreDrawable();
				_platformProgressBar.Invalidate();
				Dispose();
			}
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