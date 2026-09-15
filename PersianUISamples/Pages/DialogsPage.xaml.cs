using PersianUISamples.ViewModels;

namespace PersianUISamples.Pages;

public partial class DialogsPage : ContentPage
{
    public DialogsPage() : this(ServiceHelper.GetRequiredService<DialogsGalleryViewModel>()) { }

    public DialogsPage(DialogsGalleryViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
