using MauiPersianToolkit.Enums;
using MauiPersianToolkit.Services.Calendar;

namespace MauiPersianToolkit;

/// <summary>
/// Extension methods for calendar operations
/// Note: These methods use Persian calendar by default for backward compatibility
/// For other calendar types, use ICalendarService directly
/// </summary>
public static class CalendarExtensions
{
    private static readonly ICalendarService _defaultCalendarService = CalendarServiceFactory.GetService(CalendarType.Persian);

    /// <summary>
    /// Converts gregorian DateTime to Persian date string
    /// </summary>
    public static string ToPersianDate(this DateTime date)
    {
        return _defaultCalendarService.ToCalendarDate(date);
    }

    /// <summary>
    /// Converts gregorian DateTime to Persian date time string
    /// </summary>
    public static string ToPersianDateTime(this DateTime date)
    {
        var calendarDate = _defaultCalendarService.ToCalendarDate(date);
        return $"{calendarDate} {date.Hour.ToString().PadLeft(2, '0')}:{date.Minute.ToString().PadLeft(2, '0')}";
    }

    /// <summary>
    /// Converts Persian date string to gregorian DateTime
    /// </summary>
    public static DateTime ToDateTime(this string persianDate)
    {
        return _defaultCalendarService.ToGregorianDate(persianDate);
    }

    /// <summary>
    /// Gets the beginning of Persian month
    /// </summary>
    public static string GetPersianBeginningMonth(this DateTime date)
    {
        return _defaultCalendarService.GetMonthBeginning(date);
    }

    /// <summary>
    /// Gets the ending of Persian month
    /// </summary>
    public static string GetPersianEndingMonth(this DateTime date)
    {
        return _defaultCalendarService.GetMonthEnding(date);
    }

    /// <summary>
    /// Gets day of week in Persian calendar
    /// </summary>
    public static DayOfWeek GetPersianDay(this DateTime date)
    {
        return _defaultCalendarService.GetDayOfWeek(date);
    }

    /// <summary>
    /// Gets year in Persian calendar
    /// </summary>
    public static int GetPersianYear(this DateTime date)
    {
        return _defaultCalendarService.GetYear(date);
    }

    /// <summary>
    /// Gets month in Persian calendar
    /// </summary>
    public static int GetPersianMonth(this DateTime date)
    {
        return _defaultCalendarService.GetMonth(date);
    }

    /// <summary>
    /// Gets day of month in Persian calendar
    /// </summary>
    public static int GetPersianDayOfMonth(this DateTime date)
    {
        return _defaultCalendarService.GetDayOfMonth(date);
    }

    /// <summary>
    /// Converts date using specified calendar type
    /// </summary>
    public static string ToCalendarDate(this DateTime date, CalendarType calendarType)
    {
        var service = CalendarServiceFactory.GetService(calendarType);
        return service.ToCalendarDate(date);
    }

    /// <summary>
    /// Converts calendar string using specified calendar type
    /// </summary>
    public static DateTime ToDateTime(this string calendarDate, CalendarType calendarType)
    {
        var service = CalendarServiceFactory.GetService(calendarType);
        return service.ToGregorianDate(calendarDate);
    }

    /// <summary>
    /// Gets day of week using specified calendar type
    /// </summary>
    public static DayOfWeek GetDayOfWeek(this DateTime date, CalendarType calendarType)
    {
        var service = CalendarServiceFactory.GetService(calendarType);
        return service.GetDayOfWeek(date);
    }
}
