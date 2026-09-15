using UIKit;

namespace MauiPersianToolkit.Platform;

/// <summary>
/// Locates the window the toolkit renders its popups and alerts into.
/// </summary>
internal static class PlatformWindow
{
    /// <summary>
    /// Finds the active key window, falling back to any window of a connected scene.
    /// </summary>
    public static UIWindow? GetKeyWindow()
    {
        var application = UIApplication.SharedApplication;

        if (!OperatingSystem.IsIOSVersionAtLeast(13) && !OperatingSystem.IsMacCatalystVersionAtLeast(13))
            return application.KeyWindow;

        var windows = application.ConnectedScenes
            .OfType<UIWindowScene>()
            .OrderByDescending(scene => scene.ActivationState == UISceneActivationState.ForegroundActive)
            .SelectMany(scene => scene.Windows)
            .ToList();

        return windows.FirstOrDefault(window => window.IsKeyWindow) ?? windows.FirstOrDefault();
    }

    /// <summary>
    /// Finds the topmost presented view controller, which is what a modal popup must be
    /// presented from.
    /// </summary>
    public static UIViewController? GetTopViewController()
    {
        var controller = GetKeyWindow()?.RootViewController;

        while (controller?.PresentedViewController is { } presented)
            controller = presented;

        return controller;
    }
}
