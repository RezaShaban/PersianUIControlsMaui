using MauiPersianToolkit.Enums;
using MauiPersianToolkit.ViewModels;

namespace MauiPersianToolkit.Models;

public class DayOfMonth : ObservableObject
{
    public PersianDayOfWeek DayOfWeek { get => field; set => SetProperty(ref field, value); }
    public int DayNum { get => field; set => SetProperty(ref field, value); }
    public bool IsInCurrentMonth { get => field; set => SetProperty(ref field, value); }
    public bool CanSelect { get => field; set => SetProperty(ref field, value); }
    public bool IsSelected { get => field; set => SetProperty(ref field, value); }
    public string PersianDate { get => field; set => SetProperty(ref field, value); }
    internal int PersianDateNo { get => field; set => SetProperty(ref field, value); }
    public DateTime GregorianDate { get => field; set => SetProperty(ref field, value); }
    public bool IsHoliday { get => field; set => SetProperty(ref field, value); }
    public bool IsToday { get => field; set => SetProperty(ref field, value); }
    public bool IsInRange { get => field; set => SetProperty(ref field, value); }
}
