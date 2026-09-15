using MauiPersianToolkit.Enums;

namespace MauiPersianToolkit.Services.Calendar;

/// <summary>
/// Resolves the <see cref="ICalendarService"/> backing a given <see cref="CalendarType"/>.
/// </summary>
/// <remarks>
/// Inject this instead of calling <see cref="CalendarServiceFactory"/> directly when the
/// calling code should be substitutable in tests.
/// </remarks>
public interface ICalendarServiceResolver
{
    /// <summary>
    /// Gets the service for <paramref name="calendarType"/>.
    /// </summary>
    /// <exception cref="NotSupportedException">No service is registered for the type.</exception>
    ICalendarService Resolve(CalendarType calendarType);
}

/// <summary>
/// Resolves calendar services through <see cref="CalendarServiceFactory"/>, so a service
/// replaced with <see cref="CalendarServiceFactory.RegisterService"/> is picked up here too.
/// </summary>
internal sealed class CalendarServiceResolver : ICalendarServiceResolver
{
    public ICalendarService Resolve(CalendarType calendarType) =>
        CalendarServiceFactory.GetService(calendarType);
}
