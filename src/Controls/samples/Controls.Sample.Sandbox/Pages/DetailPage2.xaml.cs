namespace Maui.Controls.Sample.Pages;

public partial class DetailPage2 : ContentPage
{
    public DetailPage2()
    {
        InitializeComponent();
    }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        if (Application.Current?.Windows[0].Page is FlyoutPage flyoutPage)
        {
            var oldNavPage = flyoutPage.Detail as NavigationPage;
            var oldPage = oldNavPage?.CurrentPage;
            var weakNav = oldNavPage != null ? new WeakReference(oldNavPage) : null;
            var weakPage = oldPage != null ? new WeakReference(oldPage) : null;

            // Extract type names as strings before nulling strong refs.
            // If oldNavPage/oldPage are captured directly in the closure they will
            // never be GC'd and WeakReference.IsAlive will always return true,
            // making the check a false positive every time.
            string? navTypeName = oldNavPage?.GetType().Name;
            string? pageTypeName = oldPage?.GetType().Name;

            flyoutPage.Detail = new NavigationPage(new DetailPage1());

            // Null out strong refs so the closure doesn't keep the objects alive.
            oldNavPage = null;
            oldPage = null;

            Dispatcher.Dispatch(async () =>
            {
                await Task.Delay(500);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                bool navLeaked = weakNav?.IsAlive == true;
                bool pageLeaked = weakPage?.IsAlive == true;

                if (navLeaked || pageLeaked)
                {
                    var leakedNames = string.Join(", ",
                        new[] { navLeaked ? navTypeName : null, pageLeaked ? pageTypeName : null }
                            .Where(n => n != null));
                    Console.WriteLine($"[LEAK] Still alive after GC: {leakedNames}");
                    var currentPage = Application.Current?.Windows[0].Page;
                    //await currentPage!.DisplayAlertAsync("🚨 Leak Detected", $"{leakedNames} was not collected!", "OK");
                }
                else
                {
                    Console.WriteLine("[OK] Old pages were collected.");
                }
            });
        }
    }
}