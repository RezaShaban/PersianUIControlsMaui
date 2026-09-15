using PersianUISamples.ViewModels;

namespace PersianUISamples.Pages;

public partial class InputsPage : ContentPage
{
    public InputsPage() : this(ServiceHelper.GetRequiredService<InputsViewModel>()) { }

    public InputsPage(InputsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
