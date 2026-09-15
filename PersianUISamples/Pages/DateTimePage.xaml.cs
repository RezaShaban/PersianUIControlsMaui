using PersianUISamples.ViewModels;

namespace PersianUISamples.Pages;

public partial class DateTimePage : ContentPage
{
    public DateTimePage() : this(ServiceHelper.GetRequiredService<DateTimeGalleryViewModel>()) { }

    public DateTimePage(DateTimeGalleryViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
