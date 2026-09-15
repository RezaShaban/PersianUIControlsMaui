using MauiPersianToolkit.Enums;
using MauiPersianToolkit.Models;
using MauiPersianToolkit.Services.Calendar;
using MauiPersianToolkit.ViewModels;
using Xunit;

namespace MauiPersianToolkit.Test;

/// <summary>
/// Reference day: Monday 2026-09-14
/// = دوشنبه 1405/06/23 شمسی
/// = الإثنين 1448/04/02 قمری
/// = Monday September 14, 2026 میلادی
/// </summary>
public class WeekStartAndTodayTests
{
    private static readonly DateTime Today = new(2026, 9, 14);

    private readonly ICalendarService _persian = CalendarServiceFactory.GetService(CalendarType.Persian);
    private readonly ICalendarService _hijri = CalendarServiceFactory.GetService(CalendarType.Hijri);
    private readonly ICalendarService _gregorian = CalendarServiceFactory.GetService(CalendarType.Gregorian);

    [Fact]
    public void WeekStart_IsConfiguredPerCalendar()
    {
        Assert.Equal(DayOfWeek.Saturday, _persian.GetFirstDayOfWeek());
        Assert.Equal(DayOfWeek.Friday, _persian.GetLastDayOfWeek());

        Assert.Equal(DayOfWeek.Saturday, _hijri.GetFirstDayOfWeek());
        Assert.Equal(DayOfWeek.Friday, _hijri.GetLastDayOfWeek());

        Assert.Equal(DayOfWeek.Sunday, _gregorian.GetFirstDayOfWeek());
        Assert.Equal(DayOfWeek.Saturday, _gregorian.GetLastDayOfWeek());
    }

    [Fact]
    public void Today_ConvertsToExpectedCalendarDates()
    {
        Assert.Equal(DayOfWeek.Monday, Today.DayOfWeek);

        Assert.Equal("1405/06/23", _persian.ToCalendarDate(Today));
        Assert.Equal("1448/04/02", _hijri.ToCalendarDate(Today));
        Assert.Equal("2026/09/14", _gregorian.ToCalendarDate(Today));
    }

    [Fact]
    public void Today_AppearsUnderMondayColumn_InAllCalendars()
    {
        AssertMondayColumn(CalendarType.Persian, "د");
        AssertMondayColumn(CalendarType.Hijri, _hijri.GetDayOfWeekName(DayOfWeek.Monday));
        AssertMondayColumn(CalendarType.Gregorian, "Mo");
    }

    private static void AssertMondayColumn(CalendarType type, string mondayHeader)
    {
        var service = CalendarServiceFactory.GetService(type);
        var options = new CalendarOptions { CalendarType = type };
        // Force the reference day (CalendarType setter resets to "now")
        typeof(CalendarOptions)
            .GetProperty("SelectedPersianDate", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)!
            .SetValue(options, service.ToCalendarDate(Today));

        var vm = new DatePickerViewModel(options);

        Assert.Equal(mondayHeader, vm.DaysOfWeek[(int)(DayOfWeek.Monday - service.GetFirstDayOfWeek() + 7) % 7]);

        var todayCell = vm.DaysOfMonth.Single(d => d.GregorianDate.Date == Today.Date);
        var column = vm.DaysOfMonth.IndexOf(todayCell) % 7;
        var expectedColumn = ((int)DayOfWeek.Monday - (int)service.GetFirstDayOfWeek() + 7) % 7;

        Assert.Equal(expectedColumn, column);
        Assert.Equal(service.ToCalendarDate(Today), todayCell.PersianDate);
    }
}
