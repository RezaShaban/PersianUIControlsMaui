using System.Globalization;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using MauiPersianToolkit.Extensions;
using MauiPersianToolkit.Enums;

namespace MauiPersianToolkit.Services.Calendar;

/// <summary>
/// Persian (Jalali) calendar service implementation
/// </summary>
public class PersianCalendarService : ICalendarService
{
    private readonly PersianCalendar _calendar = new();

    public string ToCalendarDate(DateTime gregorianDate)
    {
        try
        {
            var year = _calendar.GetYear(gregorianDate);
            var month = _calendar.GetMonth(gregorianDate);
            var day = _calendar.GetDayOfMonth(gregorianDate);
            return $"{year}/{month.ToString().PadLeft(2, '0')}/{day.ToString().PadLeft(2, '0')}";
        }
        catch
        {
            return string.Empty;
        }
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

            return new DateTime(year, month, day, 0, 0, 0, _calendar);
        }
        catch
        {
            return DateTime.Now;
        }
    }

    public string GetMonthBeginning(DateTime date)
    {
        try
        {
            var year = _calendar.GetYear(date);
            var month = _calendar.GetMonth(date);
            return $"{year}/{month.ToString().PadLeft(2, '0')}/01";
        }
        catch
        {
            return string.Empty;
        }
    }

    public string GetMonthEnding(DateTime date)
    {
        try
        {
            var year = _calendar.GetYear(date);
            var month = _calendar.GetMonth(date);
            var daysInMonth = GetDaysInMonth(year, month);
            return $"{year}/{month.ToString().PadLeft(2, '0')}/{daysInMonth}";
        }
        catch
        {
            return string.Empty;
        }
    }

    public DayOfWeek GetDayOfWeek(DateTime date)
    {
        try
        {
            return _calendar.GetDayOfWeek(date);
        }
        catch
        {
            return DayOfWeek.Friday;
        }
    }

    public int GetYear(DateTime date)
    {
        try
        {
            return _calendar.GetYear(date);
        }
        catch
        {
            return 0;
        }
    }

    public int GetMonth(DateTime date)
    {
        try
        {
            return _calendar.GetMonth(date);
        }
        catch
        {
            return 0;
        }
    }

    public int GetDayOfMonth(DateTime date)
    {
        try
        {
            return _calendar.GetDayOfMonth(date);
        }
        catch
        {
            return 0;
        }
    }

    public string GetMonthName(int monthNumber)
    {
        try
        {
            return typeof(PersianMonthNames).GetDisplay(monthNumber - 1);
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
            // PersianDayOfWeek is ordered Sat=0..Fri=6; System.DayOfWeek is Sun=0..Sat=6.
            var persianIndex = ((int)dayOfWeek + 1) % 7;
            return typeof(PersianDayOfWeek).GetDisplay(persianIndex);
        }
        catch
        {
            return string.Empty;
        }
    }

    public DayOfWeek GetFirstDayOfWeek() => DayOfWeek.Saturday;

    public DayOfWeek GetLastDayOfWeek() => DayOfWeek.Friday;

    public bool IsLeapYear(int year)
    {
        try
        {
            return _calendar.IsLeapYear(year);
        }
        catch
        {
            return false;
        }
    }

    public int GetDaysInMonth(int year, int month)
    {
        try
        {
            return _calendar.GetDaysInMonth(year, month);
        }
        catch
        {
            return 0;
        }
    }

    public int GetMonthsInYear() => 12;

    public IEnumerable<string> GetAllMonthNames()
    {
        return typeof(PersianMonthNames)
            .GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
            .Select(field => field.GetCustomAttribute<DisplayAttribute>()?.Name ?? field.Name);
    }
}
