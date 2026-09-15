using System.ComponentModel.DataAnnotations;

namespace MauiPersianToolkit.Enums;

public enum HijriDayOfWeek
{
    [Display(Name = "أحد")]
    Sunday,
    [Display(Name = "إثن")]
    Monday,
    [Display(Name = "ثلا")]
    Tuesday,
    [Display(Name = "أرب")]
    Wednesday,
    [Display(Name = "خمي")]
    Thursday,
    [Display(Name = "جمع")]
    Friday,
    [Display(Name = "سبت")]
    Saturday,
}
