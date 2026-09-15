using PersianUISamples.ViewModels;

namespace PersianUISamples.Pages;

public partial class AutoCompletePage : ContentPage
{
    public AutoCompletePage() : this(ServiceHelper.GetRequiredService<AutoCompleteGalleryViewModel>()) { }

    public AutoCompletePage(AutoCompleteGalleryViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
