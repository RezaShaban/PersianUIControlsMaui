using PersianUISamples.ViewModels;

namespace PersianUISamples.Pages;

public partial class PickersPage : ContentPage
{
    public PickersPage() : this(ServiceHelper.GetRequiredService<PickersGalleryViewModel>()) { }

    public PickersPage(PickersGalleryViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
