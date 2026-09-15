namespace MauiPersianToolkit.Services.Calendar;

/// <summary>
/// Interface for calendar conversion and calculation operations
/// Implements Strategy Pattern to support multiple calendar types
/// </summary>
public interface ICalendarService
{
    /// <summary>
    /// Converts gregorian DateTime to calendar string representation
    /// </summary>
    string ToCalendarDate(DateTime gregorianDate);

    /// <summary>
    /// Converts calendar string representation to gregorian DateTime
    /// </summary>
    DateTime ToGregorianDate(string calendarDate);

    /// <summary>
    /// Gets the beginning of the month for the given date
    /// </summary>
    string GetMonthBeginning(DateTime date);

    /// <summary>
    /// Gets the ending of the month for the given date
    /// </summary>
    string GetMonthEnding(DateTime date);

    /// <summary>
    /// Gets the day of week (0-6 where 0 is Sunday)
    /// </summary>
    DayOfWeek GetDayOfWeek(DateTime date);

    /// <summary>
    /// Gets the year component
    /// </summary>
    int GetYear(DateTime date);

    /// <summary>
    /// Gets the month component
    /// </summary>
    int GetMonth(DateTime date);

    /// <summary>
    /// Gets the day of month component
    /// </summary>
    int GetDayOfMonth(DateTime date);

    /// <summary>
    /// Gets the month name for the given month number
    /// </summary>
    string GetMonthName(int monthNumber);

    /// <summary>
    /// Gets the day of week name
    /// </summary>
    string GetDayOfWeekName(DayOfWeek dayOfWeek);

    /// <summary>
    /// Gets the first day of the week for this calendar's grid
    /// (e.g. Saturday for Persian/Hijri, Sunday for Gregorian).
    /// </summary>
    DayOfWeek GetFirstDayOfWeek();

    /// <summary>
    /// Gets the last / holiday day of the week
    /// (e.g. Friday for Persian/Hijri, Saturday for Gregorian).
    /// </summary>
    DayOfWeek GetLastDayOfWeek();

    /// <summary>
    /// Checks if a year is leap year
    /// </summary>
    bool IsLeapYear(int year);

    /// <summary>
    /// Gets days in month
    /// </summary>
    int GetDaysInMonth(int year, int month);

    /// <summary>
    /// Gets the count of months in a year
    /// </summary>
    int GetMonthsInYear();

    /// <summary>
    /// Gets all month names
    /// </summary>
    IEnumerable<string> GetAllMonthNames();
}
