using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Microsoft.Maui;
using AndroidX.Activity;
using System;
using System.Collections.Generic;
using System.Text;
using Android.OS;

namespace Maui.Controls.Sample.Platform
{
	[Activity(
		Theme = "@style/Maui.SplashTheme",
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode)]
	[IntentFilter(
		new[] { Microsoft.Maui.ApplicationModel.Platform.Intent.ActionAppAction },
		Categories = new[] { Android.Content.Intent.CategoryDefault })]
	[Register("com.microsoft.maui.sandbox.MainActivity")]
	public class MainActivity : MauiAppCompatActivity
	{
		protected override void OnCreate(Bundle? savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			OnBackPressedDispatcher.AddCallback(this, new OnBackPress(OnBackPressedDispatcher));
		}
	}

	public class OnBackPress : OnBackPressedCallback
	{
		readonly OnBackPressedDispatcher _dispatcher;

		public OnBackPress(OnBackPressedDispatcher dispatcher) : base(true)
		{
			_dispatcher = dispatcher;
		}

		public override void HandleOnBackPressed()
		{
			//Add a breakpoint here or review console.
			Console.WriteLine("On back pressed.");

			// Disable this callback while forwarding so it does not invoke itself again.
			Enabled = false;
			try
			{
				_dispatcher.OnBackPressed();
			}
			finally
			{
				Enabled = true;
			}
		}
	}
}