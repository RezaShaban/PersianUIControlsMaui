using System.ComponentModel;
using Microsoft.Maui.Controls.Shapes;

namespace MauiPersianToolkit.Controls;

/// <summary>
/// Shared surface of the toolkit's data-entry controls: a floating placeholder, a leading
/// glyph, and inline validation feedback.
/// </summary>
/// <remarks>
/// Derive from this rather than from <see cref="ContentView"/> so a new input control picks
/// up the common bindable properties, and so styles targeting the base apply to all of them.
/// Native <see cref="Entry"/>/<see cref="Editor"/> stay as the real input; this base only
/// skins chrome and forwards focus/semantics.
/// </remarks>
public abstract class PersianInputBase : ContentView
{
    /// <summary>Visual state names applied to input chrome.</summary>
    public static class InputVisualState
    {
        public const string Normal = nameof(Normal);
        public const string Focused = nameof(Focused);
        public const string Disabled = nameof(Disabled);
        public const string Error = nameof(Error);
    }

    private Shape? _outline;
    private VisualElement? _nativeInput;
    private Color? _idlePlaceholderColor;
    private bool _nativeFocused;
    private string _visualState = InputVisualState.Normal;

    protected PersianInputBase()
    {
        // Library XAML StaticResource cannot reliably see Application MergedDictionaries.
        PersianTheme.SeedControlResources(Resources);
        PropertyChanged += OnBasePropertyChanged;
    }

    /// <summary>Backing store for <see cref="PlaceHolder"/>.</summary>
    public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create(
        nameof(PlaceHolder), typeof(string), typeof(PersianInputBase), default(string), BindingMode.TwoWay);

    /// <summary>Backing store for <see cref="PlaceHolderColor"/>.</summary>
    public static readonly BindableProperty PlaceHolderColorProperty = BindableProperty.Create(
        nameof(PlaceHolderColor), typeof(Color), typeof(PersianInputBase), null, BindingMode.TwoWay,
        defaultValueCreator: _ => ThemeColors.Muted);

    /// <summary>Backing store for <see cref="ActivePlaceHolderColor"/>.</summary>
    public static readonly BindableProperty ActivePlaceHolderColorProperty = BindableProperty.Create(
        nameof(ActivePlaceHolderColor), typeof(Color), typeof(PersianInputBase), null, BindingMode.TwoWay,
        defaultValueCreator: _ => ThemeColors.Accent);

    /// <summary>Backing store for <see cref="TextColor"/>.</summary>
    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
        nameof(TextColor), typeof(Color), typeof(PersianInputBase), null, BindingMode.TwoWay,
        defaultValueCreator: _ => ThemeColors.OnSurface);

    /// <summary>Backing store for <see cref="Icon"/>.</summary>
    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon), typeof(string), typeof(PersianInputBase), string.Empty, BindingMode.TwoWay);

    /// <summary>Backing store for <see cref="ErrorMessage"/>.</summary>
    public static readonly BindableProperty ErrorMessageProperty = BindableProperty.Create(
        nameof(ErrorMessage), typeof(string), typeof(PersianInputBase), default(string), BindingMode.TwoWay);

    /// <summary>Backing store for <see cref="IsValid"/>.</summary>
    /// <remarks>
    /// Historical naming: when <see langword="true"/> the validation message is shown (error state).
    /// </remarks>
    public static readonly BindableProperty IsValidProperty = BindableProperty.Create(
        nameof(IsValid), typeof(bool), typeof(PersianInputBase), default(bool), BindingMode.TwoWay);

    /// <summary>
    /// Label shown above the input while it is empty.
    /// </summary>
    public string PlaceHolder
    {
        get => (string)GetValue(PlaceHolderProperty);
        set => SetValue(PlaceHolderProperty, value);
    }

    /// <summary>
    /// Color of the placeholder while the input is not focused.
    /// </summary>
    public Color PlaceHolderColor
    {
        get => (Color)GetValue(PlaceHolderColorProperty);
        set => SetValue(PlaceHolderColorProperty, value);
    }

    /// <summary>
    /// Color of the placeholder while the input is focused.
    /// </summary>
    public Color ActivePlaceHolderColor
    {
        get => (Color)GetValue(ActivePlaceHolderColorProperty);
        set => SetValue(ActivePlaceHolderColorProperty, value);
    }

    /// <summary>
    /// Color of the value the control displays.
    /// </summary>
    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    /// <summary>
    /// Leading glyph, expressed as a character of the toolkit's icon font.
    /// </summary>
    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Validation message shown beneath the input.
    /// </summary>
    public string ErrorMessage
    {
        get => (string)GetValue(ErrorMessageProperty);
        set => SetValue(ErrorMessageProperty, value);
    }

    /// <summary>
    /// When <see langword="true"/>, the validation message is visible (error chrome).
    /// </summary>
    public bool IsValid
    {
        get => (bool)GetValue(IsValidProperty);
        set => SetValue(IsValidProperty, value);
    }

    /// <summary>
    /// Current chrome visual state (<see cref="InputVisualState"/>).
    /// </summary>
    public string CurrentInputVisualState => _visualState;

    /// <summary>
    /// Forwards focus to the native <see cref="Entry"/>/<see cref="Editor"/> when attached.
    /// </summary>
    public bool FocusNativeInput()
    {
        if (_nativeInput is null)
            return false;

        return _nativeInput.Focus();
    }

    /// <summary>
    /// Wires the rounded outline and optional native text surface so focus/error/disabled
    /// states update stroke without replacing platform handlers.
    /// </summary>
    protected void AttachInputChrome(Shape outline, VisualElement? nativeInput = null)
    {
        ArgumentNullException.ThrowIfNull(outline);

        if (_nativeInput is not null)
        {
            _nativeInput.Focused -= OnNativeFocused;
            _nativeInput.Unfocused -= OnNativeUnfocused;
        }

        _outline = outline;
        _nativeInput = nativeInput;

        if (_nativeInput is not null)
        {
            _nativeInput.Focused += OnNativeFocused;
            _nativeInput.Unfocused += OnNativeUnfocused;
        }

        UpdateSemantics();
        UpdateVisualState();
    }

    private void OnBasePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(PlaceHolder) or nameof(ErrorMessage) or nameof(IsValid))
            UpdateSemantics();

        if (e.PropertyName is nameof(IsEnabled) or nameof(IsValid)
            or nameof(PlaceHolderColor) or nameof(ActivePlaceHolderColor))
            UpdateVisualState();

        if (e.PropertyName == nameof(IsValid) && IsValid)
            FocusNativeInput();
    }

    private void OnNativeFocused(object? sender, FocusEventArgs e)
    {
        _nativeFocused = true;
        _idlePlaceholderColor ??= PlaceHolderColor;
        PlaceHolderColor = ActivePlaceHolderColor;
        UpdateVisualState();
    }

    private void OnNativeUnfocused(object? sender, FocusEventArgs e)
    {
        _nativeFocused = false;
        if (_idlePlaceholderColor is not null)
            PlaceHolderColor = _idlePlaceholderColor;
        UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        var next = ResolveVisualState();
        _visualState = next;
        VisualStateManager.GoToState(this, next);
        ApplyOutlineStroke(next);
    }

    private string ResolveVisualState()
    {
        if (!IsEnabled)
            return InputVisualState.Disabled;
        // IsValid == true means "show validation error" (legacy API).
        if (IsValid)
            return InputVisualState.Error;
        if (_nativeFocused)
            return InputVisualState.Focused;
        return InputVisualState.Normal;
    }

    private void ApplyOutlineStroke(string state)
    {
        if (_outline is null)
            return;

        var color = state switch
        {
            InputVisualState.Error => ThemeColors.Cancel,
            InputVisualState.Focused => ActivePlaceHolderColor ?? ThemeColors.Accent,
            InputVisualState.Disabled => ThemeColors.Disabled,
            _ => ThemeColors.Outline
        };

        _outline.Stroke = new SolidColorBrush(color);
        _outline.StrokeThickness = state is InputVisualState.Focused or InputVisualState.Error ? 1.5 : 1;
    }

    private void UpdateSemantics()
    {
        var description = PlaceHolder;
        if (IsValid && !string.IsNullOrWhiteSpace(ErrorMessage))
            description = string.IsNullOrWhiteSpace(description)
                ? ErrorMessage
                : $"{description}. {ErrorMessage}";

        if (!string.IsNullOrWhiteSpace(description))
            SemanticProperties.SetDescription(this, description);

        if (_nativeInput is not null && !string.IsNullOrWhiteSpace(PlaceHolder))
            SemanticProperties.SetDescription(_nativeInput, PlaceHolder);
    }
}
