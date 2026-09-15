namespace MauiPersianToolkit.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ContainerView : ContentView
{
    #region Propertie's
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(ContainerView), default(string), BindingMode.TwoWay);
    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }
    public static readonly BindableProperty LeftTitleProperty = BindableProperty.Create(nameof(LeftTitle), typeof(string), typeof(ContainerView), default(string), BindingMode.TwoWay);
    public string LeftTitle
    {
        get { return (string)GetValue(LeftTitleProperty); }
        set { SetValue(LeftTitleProperty, value); }
    }
    public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(string), typeof(ContainerView), "\uf104", BindingMode.TwoWay);
    public string Icon
    {
        get { return (string)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }
    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(ContainerView), true, BindingMode.TwoWay);
    public bool IsExpanded
    {
        get { return (bool)GetValue(IsExpandedProperty); }
        set { SetValue(IsExpandedProperty, value); }
    }
    public static readonly BindableProperty HasShadowProperty = BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(ContainerView), true, BindingMode.TwoWay);
    public bool HasShadow
    {
        get { return (bool)GetValue(HasShadowProperty); }
        set { SetValue(HasShadowProperty, value); }
    }
    public static readonly BindableProperty ContentsProperty = BindableProperty.Create(nameof(Contents), typeof(View), typeof(ContainerView), default(View), BindingMode.TwoWay);
    public View Contents
    {
        get { return (View)GetValue(ContentsProperty); }
        set { SetValue(ContentsProperty, value); }
    }
    #endregion

    public ContainerView()
    {
        PersianTheme.SeedControlResources(Resources);
        InitializeComponent();
    }
}