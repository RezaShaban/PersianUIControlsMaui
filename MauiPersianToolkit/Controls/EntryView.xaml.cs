using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiPersianToolkit.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class EntryView : PersianInputBase
{
    #region Field's
    Color _color;
    #endregion

    #region Propertie's
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(EntryView), default(string), BindingMode.TwoWay);
    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }
    public static readonly BindableProperty AppendTextProperty = BindableProperty.Create(nameof(AppendText), typeof(string), typeof(EntryView), default(string), BindingMode.OneWay);
    public string AppendText
    {
        get { return (string)GetValue(AppendTextProperty); }
        set { SetValue(AppendTextProperty, value); }
    }
    public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(EntryView), TextAlignment.Start, BindingMode.TwoWay);
    public TextAlignment HorizontalTextAlignment
    {
        get { return (TextAlignment)GetValue(HorizontalTextAlignmentProperty); }
        set { SetValue(HorizontalTextAlignmentProperty, value); }
    }
    public static readonly BindableProperty EntryFlowDirectionProperty = BindableProperty.Create(nameof(EntryFlowDirection), typeof(FlowDirection), typeof(EntryView), FlowDirection.RightToLeft, BindingMode.TwoWay);
    public FlowDirection EntryFlowDirection
    {
        get { return (FlowDirection)GetValue(EntryFlowDirectionProperty); }
        set { SetValue(EntryFlowDirectionProperty, value); }
    }
    public static readonly BindableProperty ErrorMessageColorProperty = BindableProperty.Create(nameof(ErrorMessageColor), typeof(Color), typeof(EntryView), Colors.OrangeRed, BindingMode.OneWay);
    public Color ErrorMessageColor
    {
        get { return (Color)GetValue(ErrorMessageColorProperty); }
        set { SetValue(ErrorMessageColorProperty, value); }
    }
    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(EntryView), default(bool), BindingMode.TwoWay);
    public bool IsPassword
    {
        get { return (bool)GetValue(IsPasswordProperty); }
        set { SetValue(IsPasswordProperty, value); }
    }
    public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(nameof(ReturnType), typeof(ReturnType), typeof(EntryView), default(ReturnType), BindingMode.TwoWay);
    public ReturnType ReturnType
    {
        get { return (ReturnType)GetValue(ReturnTypeProperty); }
        set { SetValue(ReturnTypeProperty, value); }
    }
    public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(nameof(ReturnCommand), typeof(ICommand), typeof(EntryView), default(ICommand), BindingMode.TwoWay);
    public ICommand ReturnCommand
    {
        get { return (ICommand)GetValue(ReturnCommandProperty); }
        set { SetValue(ReturnCommandProperty, value); }
    }
    public static readonly BindableProperty TextChangedCommandProperty = BindableProperty.Create(nameof(TextChangedCommand), typeof(ICommand), typeof(EntryView), default(ICommand), BindingMode.TwoWay);
    public ICommand TextChangedCommand
    {
        get { return (ICommand)GetValue(TextChangedCommandProperty); }
        set { SetValue(TextChangedCommandProperty, value); }
    }
    public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(EntryView), default(Keyboard), BindingMode.TwoWay);
    public Keyboard Keyboard
    {
        get { return (Keyboard)GetValue(KeyboardProperty); }
        set { SetValue(KeyboardProperty, value); }
    }
    #endregion

    public event EventHandler<TextChangedEventArgs> TextChanged;

    public EntryView()
    {
        InitializeComponent();
        AttachInputChrome(outline, entry);

        if (_color is null)
            _color = PlaceHolderColor;
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == AppendTextProperty.PropertyName)
            append.Text = AppendText;
    }

    private void entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        TextChangedCommand?.Execute(e.NewTextValue);
        TextChanged?.Invoke(sender, e);
    }
}