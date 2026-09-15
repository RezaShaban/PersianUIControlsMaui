namespace PersianUISamples;

internal static class ServiceHelper
{
    public static T GetRequiredService<T>() where T : notnull
    {
        var services = GetServices()
            ?? throw new InvalidOperationException("MAUI services are not available yet.");
        return services.GetRequiredService<T>();
    }

    public static T? TryGetService<T>() where T : class
    {
        var services = GetServices();
        return services?.GetService<T>();
    }

    private static IServiceProvider? GetServices() =>
        Application.Current?.Handler?.MauiContext?.Services
        ?? IPlatformApplication.Current?.Services;
}
