using MauiPersianToolkit.Enums;
using MauiPersianToolkit.Extensions;
using System.Globalization;

namespace MauiPersianToolkit.Services.Calendar;

/// <summary>
/// Hijri (Islamic) calendar service implementation
/// </summary>
public class HijriCalendarService : ICalendarService
{
    private readonly HijriCalendar _calendar = new() { HijriAdjustment = 0 };

    // Islamic month names (short labels for the month picker)
    private static readonly string[] HijriMonthNames =
    [
        "محرم",
        "صفر",
        "ر.الأول",
        "ر.الثاني",
        "ج.الأولى",
        "ج.الثانية",
        "رجب",
        "شعبان",
        "رمضان",
        "شوال",
        "ذ.القعدة",
        "ذ.الحجة"
    ];

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

            return _calendar.ToDateTime(year, month, day, 0, 0, 0, 0).Date;
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
            if (monthNumber < 1 || monthNumber > HijriMonthNames.Length)
                return string.Empty;
            return HijriMonthNames[monthNumber - 1];
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
            return typeof(HijriDayOfWeek).GetDisplay((int)dayOfWeek);
        }
        catch
        {
            return string.Empty;
        }
    }

    // Iranian-style week: Saturday → Friday (same as Persian).
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

    public IEnumerable<string> GetAllMonthNames() => HijriMonthNames;
}
