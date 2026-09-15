using PersianUISamples.ViewModels;

namespace PersianUISamples.Pages;

public partial class TreePage : ContentPage
{
    public TreePage() : this(ServiceHelper.GetRequiredService<TreeGalleryViewModel>()) { }

    public TreePage(TreeGalleryViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
