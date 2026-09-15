using MauiPersianToolkit.Localization;
using Xunit;

namespace MauiPersianToolkit.Test;

public class LocalizationTests
{
    [Fact]
    public void PersianCatalog_ReturnsPersianChrome()
    {
        var localizer = PersianToolkitLocalizer.CreateDefault();
        localizer.SetCulture("fa");

        Assert.Equal("ثبت", localizer[PersianToolkitStringId.Accept]);
        Assert.Equal("انصراف", localizer[PersianToolkitStringId.Cancel]);
        Assert.Equal("امروز", localizer[PersianToolkitStringId.Today]);
        Assert.Equal("باشه", localizer[PersianToolkitStringId.Ok]);
    }

    [Fact]
    public void EnglishCatalog_ReturnsEnglishChrome()
    {
        var localizer = PersianToolkitLocalizer.CreateDefault();
        localizer.SetCulture("en-US");

        Assert.Equal("Save", localizer[PersianToolkitStringId.Accept]);
        Assert.Equal("Cancel", localizer[PersianToolkitStringId.Cancel]);
        Assert.Equal("Today", localizer[PersianToolkitStringId.Today]);
        Assert.Equal("OK", localizer[PersianToolkitStringId.Ok]);
    }

    [Fact]
    public void Override_WinsOverCatalog()
    {
        var localizer = PersianToolkitLocalizer.CreateDefault();
        localizer.SetCulture("fa");
        localizer.SetOverride(PersianToolkitStringId.Accept, "تایید نهایی");

        Assert.Equal("تایید نهایی", localizer[PersianToolkitStringId.Accept]);
    }

    [Fact]
    public void AdditionalCatalog_IsResolvable()
    {
        var localizer = PersianToolkitLocalizer.CreateDefault();
        localizer.RegisterCatalog("ar", new Dictionary<string, string>
        {
            [PersianToolkitStringId.Today] = "اليوم",
            [PersianToolkitStringId.Cancel] = "إلغاء",
        });
        localizer.SetCulture("ar");

        Assert.Equal("اليوم", localizer[PersianToolkitStringId.Today]);
        Assert.Equal("إلغاء", localizer[PersianToolkitStringId.Cancel]);
        // Missing key falls back to Persian catalog
        Assert.Equal("ثبت", localizer[PersianToolkitStringId.Accept]);
    }
}
