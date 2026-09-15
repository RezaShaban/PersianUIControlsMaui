using MauiPersianToolkit.Controls;
using MauiPersianToolkit.Localization;
using Xunit;

namespace MauiPersianToolkit.Test;

public class TimePickerTests
{
    [Theory]
    [InlineData(14, 37, 22, 5, false, 14, 35, 0)]
    [InlineData(14, 37, 22, 5, true, 14, 35, 22)]
    [InlineData(9, 2, 0, 15, false, 9, 0, 0)]
    [InlineData(23, 59, 59, 1, true, 23, 59, 59)]
    public void ClampTime_SnapsMinutesAndOptionalSeconds(
        int h, int m, int s, int interval, bool showSeconds,
        int eh, int em, int es)
    {
        var result = TimePickerView.ClampTime(new TimeSpan(h, m, s), interval, showSeconds);

        Assert.Equal(eh, result.Hours);
        Assert.Equal(em, result.Minutes);
        Assert.Equal(es, result.Seconds);
    }

    [Fact]
    public void Localization_SelectTimeAndNow_ExistInBothCatalogs()
    {
        var fa = PersianToolkitLocalizer.CreateDefault();
        fa.SetCulture("fa");
        Assert.Equal("انتخاب زمان", fa[PersianToolkitStringId.SelectTime]);
        Assert.Equal("الان", fa[PersianToolkitStringId.Now]);

        var en = PersianToolkitLocalizer.CreateDefault();
        en.SetCulture("en");
        Assert.Equal("Select time", en[PersianToolkitStringId.SelectTime]);
        Assert.Equal("Now", en[PersianToolkitStringId.Now]);
    }
}
