using PersianUISamples.ViewModels;

namespace PersianUISamples.Pages;

public partial class LayoutPage : ContentPage
{
    public LayoutPage() : this(ServiceHelper.GetRequiredService<LayoutGalleryViewModel>()) { }

    public LayoutPage(LayoutGalleryViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
