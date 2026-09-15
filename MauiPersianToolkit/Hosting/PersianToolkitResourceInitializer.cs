namespace MauiPersianToolkit.Hosting;

/// <summary>
/// Ensures toolkit theme tokens are present and applies
/// <see cref="PersianToolkitOptions.Theme"/> overrides as soon as an
/// <see cref="Application"/> exists.
/// </summary>
/// <remarks>
/// <see cref="IMauiInitializeService"/> often runs during <c>MauiApp.Build()</c> before
/// <see cref="Application.Current"/> is set, so this type also implements
/// <see cref="IMauiInitializeScopedService"/> (first window) as a second chance.
/// Controls additionally call <see cref="PersianTheme.EnsureApplicationStyles"/> before
/// loading XAML so zero-config consumers always get defaults.
/// </remarks>
internal sealed class PersianToolkitResourceInitializer : IMauiInitializeService, IMauiInitializeScopedService
{
    public void Initialize(IServiceProvider services) => PersianTheme.EnsureApplicationStyles();
}
