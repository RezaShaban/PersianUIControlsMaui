using MauiPersianToolkit.Enums;
using MauiPersianToolkit.Extensions;
using System.Globalization;

namespace MauiPersianToolkit.Services.Calendar;

/// <summary>
/// Gregorian calendar service implementation
/// </summary>
public class GregorianCalendarService : ICalendarService
{
    private readonly GregorianCalendar _calendar = new();

    public string ToCalendarDate(DateTime gregorianDate)
    {
        var year = gregorianDate.Year;
        var month = gregorianDate.Month;
        var day = gregorianDate.Day;
        return $"{year}/{month.ToString().PadLeft(2, '0')}/{day.ToString().PadLeft(2, '0')}";
    }

    public DateTime ToGregorianDate(string calendarDate)
    {
        try
        {
            if (string.IsNullOrEmpty(calendarDate) || !calendarDate.Contains("/"))
                return DateTime.Now;

            var parts = calendarDate.Split('/');
            if (parts.Length != 3)
                return DateTime.Now;

            var year = int.Parse(parts[0]);
            var month = int.Parse(parts[1]);
            var day = int.Parse(parts[2]);

            return new DateTime(year, month, day);
        }
        catch
        {
            return DateTime.Now;
        }
    }

    public string GetMonthBeginning(DateTime date)
    {
        var year = date.Year;
        var month = date.Month;
        return $"{year}/{month.ToString().PadLeft(2, '0')}/01";
    }

    public string GetMonthEnding(DateTime date)
    {
        var year = date.Year;
        var month = date.Month;
        var daysInMonth = GetDaysInMonth(year, month);
        return $"{year}/{month.ToString().PadLeft(2, '0')}/{daysInMonth}";
    }

    public DayOfWeek GetDayOfWeek(DateTime date) => date.DayOfWeek;

    public int GetYear(DateTime date) => date.Year;

    public int GetMonth(DateTime date) => date.Month;

    public int GetDayOfMonth(DateTime date) => date.Day;

    public string GetMonthName(int monthNumber)
    {
        try
        {
            var cultureInfo = new CultureInfo("en-US");
            return cultureInfo.DateTimeFormat.GetMonthName(monthNumber);
        }
        catch
        {
            return string.Empty;
        }
    }

    public string GetDayOfWeekName(DayOfWeek dayOfWeek)
    {
        try
        {
            var cultureInfo = new CultureInfo("en-US");
            return cultureInfo.DateTimeFormat.GetDayName(dayOfWeek).Substring(0, 2);
            //return typeof(DayOfWeek).GetDisplay((int)dayOfWeek);
        }
        catch
        {
            return string.Empty;
        }
    }

    public DayOfWeek GetFirstDayOfWeek() => DayOfWeek.Sunday;

    public DayOfWeek GetLastDayOfWeek() => DayOfWeek.Saturday;

    public bool IsLeapYear(int year)
    {
        return _calendar.IsLeapYear(year);
    }

    public int GetDaysInMonth(int year, int month)
    {
        return _calendar.GetDaysInMonth(year, month);
    }

    public int GetMonthsInYear() => 12;

    public IEnumerable<string> GetAllMonthNames()
    {
        var cultureInfo = new CultureInfo("en-US");
        return Enumerable.Range(1, 12)
            .Select(month => cultureInfo.DateTimeFormat.GetMonthName(month));
    }
}
