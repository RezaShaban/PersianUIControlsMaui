using PersianUISamples.ViewModels;

namespace PersianUISamples.Pages;

public partial class HomePage : ContentPage
{
    public HomePage() : this(ServiceHelper.GetRequiredService<HomeViewModel>()) { }

    public HomePage(HomeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void OnCategoryTapped(object? sender, TappedEventArgs e)
    {
        if (sender is not BindableObject { BindingContext: CategoryCard card })
            return;

        await Shell.Current.GoToAsync($"//{card.Route}");
    }
}
