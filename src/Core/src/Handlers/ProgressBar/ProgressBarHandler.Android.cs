using static Android.Resource;
using AndroidProgressBar = Android.Widget.ProgressBar;

namespace Microsoft.Maui.Handlers
{
	public partial class ProgressBarHandler : ViewHandler<IProgress, AndroidProgressBar>
	{
		AndroidProgressBar? _progressColorPlatformView;
		bool _hasAppliedProgressColor;

		protected override AndroidProgressBar CreatePlatformView()
		{
			return new AndroidProgressBar(Context, null, Attribute.ProgressBarStyleHorizontal)
			{
				Indeterminate = false,
				Max = ProgressBarExtensions.Maximum
			};
		}

		public static void MapProgress(IProgressBarHandler handler, IProgress progress)
		{
			handler.PlatformView?.UpdateProgress(progress);
		}

		public static void MapProgressColor(IProgressBarHandler handler, IProgress progress)
		{
			if (handler is ProgressBarHandler progressBarHandler)
			{
				progressBarHandler.UpdateProgressColor(progress);
			}
			else
			{
				handler.PlatformView?.UpdateProgressColor(progress);
			}
		}

		void UpdateProgressColor(IProgress progress)
		{
			var platformView = PlatformView;
			if (platformView is null)
			{
				return;
			}

			if (!ReferenceEquals(_progressColorPlatformView, platformView))
			{
				_progressColorPlatformView = platformView;
				_hasAppliedProgressColor = false;
			}

			if (progress.ProgressColor is null && !_hasAppliedProgressColor)
			{
				return;
			}

			platformView.UpdateProgressColor(progress);
			_hasAppliedProgressColor = progress.ProgressColor is not null;
		}
	}
}