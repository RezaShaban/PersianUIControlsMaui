namespace MauiPersianToolkit.Core;

/// <summary>
/// How long a toast stays on screen.
/// </summary>
public enum ToastDuration
{
    /// <summary>Roughly two seconds.</summary>
    Short = 0,

    /// <summary>Roughly three and a half seconds.</summary>
    Long = 1
}

/// <summary>
/// Maps <see cref="ToastDuration"/> onto concrete timings for platforms that need them.
/// </summary>
public static class ToastDurationExtensions
{
    /// <summary>
    /// Converts the duration into the time span used by platforms without a native
    /// short/long notion.
    /// </summary>
    public static TimeSpan ToTimeSpan(this ToastDuration duration) => duration switch
    {
        ToastDuration.Short => TimeSpan.FromSeconds(2),
        ToastDuration.Long => TimeSpan.FromSeconds(3.5),
        _ => throw new NotSupportedException($"Toast duration '{duration}' is not supported.")
    };
}
