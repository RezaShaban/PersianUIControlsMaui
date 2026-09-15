using System.ComponentModel;
using System.Windows.Input;

namespace MauiPersianToolkit.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ButtonView : ContentView
{
    public ButtonView()
    {
        PersianTheme.SeedControlResources(Resources);
        InitializeComponent();
        btn.Clicked += Button_Clicked;
    }

    #region Propertie's
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ButtonView), default(string), BindingMode.TwoWay);
    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ButtonView), 13d, BindingMode.TwoWay);
    [TypeConverter(typeof(FontSizeConverter))]
    public double FontSize
    {
        get { return (double)GetValue(FontSizeProperty); }
        set { SetValue(FontSizeProperty, value); }
    }

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ButtonView), default(ICommand), BindingMode.TwoWay);
    public ICommand Command
    {
        get { return (ICommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }
    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ButtonView), default(object), BindingMode.TwoWay);
    public object CommandParameter
    {
        get { return (object)GetValue(CommandParameterProperty); }
        set { SetValue(CommandParameterProperty, value); }
    }
    public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(string), typeof(ButtonView), default(string), BindingMode.TwoWay);
    public string Icon
    {
        get { return (string)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }
    public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(ButtonView), default(bool), BindingMode.TwoWay);
    public bool IsBusy
    {
        get { return (bool)GetValue(IsBusyProperty); }
        set { SetValue(IsBusyProperty, value); }
    }
    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(ButtonView), default(bool), BindingMode.TwoWay);
    public bool IsExpanded
    {
        get { return (bool)GetValue(IsExpandedProperty); }
        set { SetValue(IsExpandedProperty, value); }
    }
    public static readonly BindableProperty BtnBgColorProperty = BindableProperty.Create(nameof(BtnBgColor), typeof(Color), typeof(ButtonView), default(Color), BindingMode.TwoWay);
    public Color BtnBgColor
    {
        get { return (Color)GetValue(BtnBgColorProperty); }
        set { SetValue(BtnBgColorProperty, value); }
    }
    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ButtonView), default(Color), BindingMode.TwoWay);
    public Color TextColor
    {
        get { return (Color)GetValue(TextColorProperty); }
        set { SetValue(TextColorProperty, value); }
    }
    public static readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(ButtonView), default(Color), BindingMode.TwoWay);
    public Color IconColor
    {
        get { return (Color)GetValue(IconColorProperty); }
        set { SetValue(IconColorProperty, value); }
    }
    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(ButtonView), default(int), BindingMode.TwoWay);
    public int CornerRadius
    {
        get { return (int)GetValue(CornerRadiusProperty); }
        set { SetValue(CornerRadiusProperty, value); }
    }
    public static readonly BindableProperty BtnMarginProperty = BindableProperty.Create(nameof(BtnMargin), typeof(Thickness), typeof(ButtonView), default(Thickness), BindingMode.TwoWay);
    public Thickness BtnMargin
    {
        get { return (Thickness)GetValue(BtnMarginProperty); }
        set { SetValue(BtnMarginProperty, value); }
    }
    #endregion

    #region Event's
    public event EventHandler Clicked;
    #endregion

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (Clicked != null)
            Clicked.Invoke(this, e);
        else
            IsExpanded = !IsExpanded;
    }
}