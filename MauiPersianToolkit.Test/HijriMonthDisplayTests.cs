using MauiPersianToolkit.Enums;
using MauiPersianToolkit.Models;
using MauiPersianToolkit.Services.Calendar;
using MauiPersianToolkit.ViewModels;
using Xunit;

namespace MauiPersianToolkit.Test;

public class HijriMonthDisplayTests
{
    private readonly ICalendarService _hijri = CalendarServiceFactory.GetService(CalendarType.Hijri);

    [Fact]
    public void Hijri_CurrentMonthDays_AreSequentialAndComplete()
    {
        var now = DateTime.Now;
        var options = new CalendarOptions { CalendarType = CalendarType.Hijri };
        var vm = new DatePickerViewModel(options);
        var current = vm.DaysOfMonth.Where(d => d.IsInCurrentMonth).ToList();
        var expectedDays = _hijri.GetDaysInMonth(_hijri.GetYear(now), _hijri.GetMonth(now));

        Assert.Equal(expectedDays, current.Count);
        for (var i = 0; i < current.Count; i++)
            Assert.Equal(i + 1, current[i].DayNum);
    }

    [Fact]
    public void Hijri_WeekHeaders_MatchSaturdayFirstGrid()
    {
        var options = new CalendarOptions { CalendarType = CalendarType.Hijri };
        var vm = new DatePickerViewModel(options);

        // Friday is last day of week → headers start on Saturday (سبت)
        Assert.Equal(DayOfWeek.Saturday, _hijri.GetFirstDayOfWeek());
        Assert.Equal(DayOfWeek.Friday, _hijri.GetLastDayOfWeek());
        Assert.Equal(_hijri.GetDayOfWeekName(DayOfWeek.Saturday), vm.DaysOfWeek[0]);
        Assert.Equal(_hijri.GetDayOfWeekName(DayOfWeek.Friday), vm.DaysOfWeek[6]);

        var first = vm.DaysOfMonth.First(d => d.IsInCurrentMonth && d.DayNum == 1);
        var index = vm.DaysOfMonth.IndexOf(first);
        var expectedOffset = ((int)_hijri.GetDayOfWeek(first.GregorianDate) - (int)_hijri.GetFirstDayOfWeek() + 7) % 7;
        Assert.Equal(expectedOffset, index);
    }

    [Fact]
    public void Hijri_IsInCurrentMonth_MatchesCalendarMonth()
    {
        var now = DateTime.Now;
        var month = _hijri.GetMonth(now);
        var year = _hijri.GetYear(now);
        var options = new CalendarOptions { CalendarType = CalendarType.Hijri };
        var vm = new DatePickerViewModel(options);

        Assert.All(vm.DaysOfMonth.Where(d => d.IsInCurrentMonth), d =>
        {
            Assert.Equal(month, _hijri.GetMonth(d.GregorianDate));
            Assert.Equal(year, _hijri.GetYear(d.GregorianDate));
        });

        Assert.All(vm.DaysOfMonth.Where(d => !d.IsInCurrentMonth), d =>
            Assert.False(_hijri.GetMonth(d.GregorianDate) == month && _hijri.GetYear(d.GregorianDate) == year));
    }
}
