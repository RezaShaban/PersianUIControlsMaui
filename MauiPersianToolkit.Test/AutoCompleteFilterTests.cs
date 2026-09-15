using MauiPersianToolkit.Enums;
using MauiPersianToolkit.Helpers;
using MauiPersianToolkit.Localization;
using Xunit;

namespace MauiPersianToolkit.Test;

public class AutoCompleteFilterTests
{
    private sealed record City(int Id, string Title);

    private static readonly City[] Cities =
    [
        new(1, "تهران"),
        new(2, "مشهد"),
        new(3, "اصفهان"),
        new(4, "شیراز"),
        new(5, "تبریز"),
    ];

    [Fact]
    public void Contains_ReturnsMatchingCities()
    {
        var result = AutoCompleteFilter.Filter(Cities, "شه", "Title", AutoCompleteFilterMode.Contains, 10);

        Assert.Contains(result, x => ((City)x).Title == "مشهد");
        Assert.DoesNotContain(result, x => ((City)x).Title == "اصفهان");
    }

    [Fact]
    public void StartsWith_RespectsPrefix()
    {
        var result = AutoCompleteFilter.Filter(Cities, "ته", "Title", AutoCompleteFilterMode.StartsWith, 10);

        Assert.Single(result);
        Assert.Equal("تهران", ((City)result[0]).Title);
    }

    [Fact]
    public void MaxResults_IsHonored()
    {
        var many = Enumerable.Range(1, 20).Select(i => new City(i, $"شهر{i}")).ToArray();
        var result = AutoCompleteFilter.Filter(many, "شهر", "Title", AutoCompleteFilterMode.StartsWith, 5);
        Assert.Equal(5, result.Count);
    }

    [Fact]
    public void CustomFilter_OverridesMode()
    {
        var result = AutoCompleteFilter.Filter(
            Cities, "x", "Title", AutoCompleteFilterMode.StartsWith, 10,
            customFilter: (item, _) => ((City)item).Id == 3);

        Assert.Single(result);
        Assert.Equal("اصفهان", ((City)result[0]).Title);
    }

    [Fact]
    public void Localization_NoResults_Exists()
    {
        var fa = PersianToolkitLocalizer.CreateDefault();
        fa.SetCulture("fa");
        Assert.Equal("نتیجه‌ای یافت نشد", fa[PersianToolkitStringId.NoResults]);
    }
}
