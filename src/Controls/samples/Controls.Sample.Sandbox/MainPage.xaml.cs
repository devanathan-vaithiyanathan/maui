using System.Collections;
using System.Reflection;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Storage;

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	FileResult? _selectedFile = null;
	string? _selectedFilePath = null;

	public MainPage()
	{
		InitializeComponent();
	}

	async void OnResetMaxLengthClicked(object? sender, EventArgs e)
	{
		MaxLengthEntry.Text = "ABCD";
		MaxLengthEntry.CursorPosition = 2;
		MaxLengthEntry.SelectionLength = 0;
		MaxLengthStatus.Text = "Paste 123456 at the cursor";
		//await MaxLengthEntry.FocusAsync();
	}

	void OnMaxLengthTextChanged(object? sender, TextChangedEventArgs e)
	{
		if (e.NewTextValue == "12345")
			MaxLengthStatus.Text = "REPRODUCED: existing text was discarded";
		else if (e.NewTextValue != "ABCD")
			MaxLengthStatus.Text = $"Result: {e.NewTextValue}";
	}

	void OnIndicatorReconnectClicked(object? sender, EventArgs e)
	{
#if IOS || MACCATALYST
		var indicator = new IndicatorView { Count = 3, Position = 0 };
		using var pageControl = new Microsoft.Maui.Platform.MauiPageControl { Pages = 3 };
		pageControl.SetIndicatorView(indicator);
		pageControl.SetIndicatorView(null);
		pageControl.SetIndicatorView(indicator);
		pageControl.CurrentPage = 1;
		pageControl.SendActionForControlEvents(UIKit.UIControlEvent.ValueChanged);

		IndicatorStatus.Text = indicator.Position == 0
			? "REPRODUCED: Position stayed at 0 after reconnect"
			: $"NOT REPRODUCED: Position changed to {indicator.Position}";
#else
		IndicatorStatus.Text = "Run on iOS or Mac Catalyst";
#endif
	}

	async void OnPermissionLeakClicked(object? sender, EventArgs e)
	{
#if ANDROID
		var before = GetPendingPermissionCount();
		try
		{
			await Task.Run(async () => await new PermissionProbe().RunAsync());
		}
		catch (PermissionException)
		{
		}

		var after = GetPendingPermissionCount();
		PermissionStatus.Text = after > before
			? $"REPRODUCED: pending requests grew from {before} to {after}"
			: $"NOT REPRODUCED: pending requests stayed at {after}";
#else
		await Task.CompletedTask;
		PermissionStatus.Text = "Run on Android";
#endif
	}

	async void OnOpenFooterReproductionClicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(ShellFooterReproductionPage));
	}

	async void OnContactsStressClicked(object? sender, EventArgs e)
	{
#if ANDROID
		// var permission = await Permissions.RequestAsync<Permissions.ContactsRead>();
		// if (permission != PermissionStatus.Granted)
		// {
		// 	ContactsStatus.Text = $"Contacts permission: {permission}";
		// 	return;
		// }

		// ContactsStatus.Text = "Running...";
		// for (var iteration = 0; iteration < 500; iteration++)
		// {
		// 	var contacts = await Contacts.GetAllAsync();
		// 	_ = contacts.FirstOrDefault();
		// }

		// ContactsStatus.Text = "Completed 500; inspect logcat for CursorWindow warnings";
#else
		await Task.CompletedTask;
		ContactsStatus.Text = "Run on Android";
#endif
	}

	async void OnPickFileClicked(object? sender, EventArgs e)
	{
#if IOS || MACCATALYST
		_selectedFile = await FilePicker.Default.PickAsync();
		_selectedFilePath = _selectedFile?.FullPath;
		FileStatus.Text = _selectedFile is null ? "Selection cancelled" : $"Selected: {_selectedFile.FileName}";
#else
		await Task.CompletedTask;
		FileStatus.Text = "Run on iOS or Mac Catalyst";
#endif
	}

	async void OnForceFileFailuresClicked(object? sender, EventArgs e)
	{
#if IOS || MACCATALYST
		if (_selectedFile is null || _selectedFilePath is null)
		{
			FileStatus.Text = "Pick a file first";
			return;
		}

		var fullPathProperty = typeof(FileResult).BaseType?.GetProperty(nameof(FileResult.FullPath));
		fullPathProperty?.SetValue(_selectedFile, Path.Combine(FileSystem.CacheDirectory, "missing", Guid.NewGuid().ToString()));

		var failures = 0;
		for (var iteration = 0; iteration < 100; iteration++)
		{
			try
			{
				await using var stream = await _selectedFile.OpenReadAsync();
			}
			catch
			{
				failures++;
			}
		}

		fullPathProperty?.SetValue(_selectedFile, _selectedFilePath);
		FileStatus.Text = $"REPRODUCED exception path {failures}/100 times; each start lacks a stop";
#else
		await Task.CompletedTask;
		FileStatus.Text = "Run on iOS or Mac Catalyst";
#endif
	}

#if ANDROID
	static int GetPendingPermissionCount()
	{
		var requests = typeof(Permissions.BasePlatformPermission)
			.GetField("requests", BindingFlags.Static | BindingFlags.NonPublic)?
			.GetValue(null) as ICollection;
		return requests?.Count ?? -1;
	}

	sealed class PermissionProbe : Permissions.BasePlatformPermission
	{
		public Task RunAsync() => DoRequest([Android.Manifest.Permission.Camera]);
	}
#endif

}