#if USE_MEMORY_TOOLKIT
using MemoryToolkit.Maui;
#endif
namespace Maui.Controls.Sample;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp
			.CreateBuilder()
#if __ANDROID__ || __IOS__
			.UseMauiMaps()
#endif
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("Dokdo-Regular.ttf", "Dokdo");
				fonts.AddFont("LobsterTwo-Regular.ttf", "Lobster Two");
				fonts.AddFont("LobsterTwo-Bold.ttf", "Lobster Two Bold");
				fonts.AddFont("LobsterTwo-Italic.ttf", "Lobster Two Italic");
				fonts.AddFont("LobsterTwo-BoldItalic.ttf", "Lobster Two BoldItalic");
				fonts.AddFont("ionicons.ttf", "Ionicons");
				fonts.AddFont("SegoeUI.ttf", "Segoe UI");
				fonts.AddFont("SegoeUI-Bold.ttf", "Segoe UI Bold");
				fonts.AddFont("SegoeUI-Italic.ttf", "Segoe UI Italic");
				fonts.AddFont("SegoeUI-Bold-Italic.ttf", "Segoe UI Bold Italic");
			});

#if DEBUG && USE_MEMORY_TOOLKIT
		// Console.WriteLine("SANDBOX: UseLeakDetection registered");
		// builder.UseLeakDetection(collectionTarget =>
		// {
		// 	// This callback will run any time a leak is detected.
		// 	Console.WriteLine($"SANDBOX: 💦 LEAK DETECTED — {collectionTarget.Name} is a zombie!");
		// 	var mainPage = Application.Current?.Windows[0].Page;
		// 	mainPage.DisplayAlertAsync("💦Leak Detected💦", $"❗🧟❗{collectionTarget.Name} is a zombie!", "OK");
		// });
#endif

		return builder.Build();
	}
}
