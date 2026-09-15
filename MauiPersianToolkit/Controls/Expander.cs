namespace MauiPersianToolkit.Controls;

/// <summary>
/// A header that toggles the visibility of its content when tapped.
/// </summary>
/// <remarks>
/// Derives from <see cref="Grid"/> rather than <see cref="ContentView"/> so that
/// <see cref="Content"/> is a property this control owns. Shadowing
/// <see cref="ContentView.Content"/> would let a binding replace the internal layout and
/// take the header with it.
/// </remarks>
[ContentProperty(nameof(Content))]
public class Expander : Grid
{
    /// <summary>Backing store for <see cref="Header"/>.</summary>
    public static readonly BindableProperty HeaderProperty = BindableProperty.Create(
        nameof(Header), typeof(View), typeof(Expander), propertyChanged: OnHeaderChanged);

    /// <summary>Backing store for <see cref="Content"/>.</summary>
    public static readonly BindableProperty ContentProperty = BindableProperty.Create(
        nameof(Content), typeof(View), typeof(Expander), propertyChanged: OnContentChanged);

    /// <summary>Backing store for <see cref="IsExpanded"/>.</summary>
    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(
        nameof(IsExpanded), typeof(bool), typeof(Expander), true,
        BindingMode.TwoWay, propertyChanged: OnIsExpandedChanged);

    /// <summary>
    /// Raised when the expander is expanded or collapsed.
    /// </summary>
    public event EventHandler<ExpandedChangedEventArgs>? ExpandedChanged;

    public Expander()
    {
        RowDefinitions =
        [
            new RowDefinition(GridLength.Auto),
            new RowDefinition(GridLength.Auto)
        ];
    }

    /// <summary>
    /// View shown above the content and used as the toggle target.
    /// </summary>
    public View? Header
    {
        get => (View?)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Collapsible body of the expander.
    /// </summary>
    public View? Content
    {
        get => (View?)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    /// Whether the content is currently visible. Defaults to <see langword="true"/>.
    /// </summary>
    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    private static void OnHeaderChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var expander = (Expander)bindable;
        expander.Swap((View?)oldValue, (View?)newValue, row: 0);

        if (newValue is View header)
        {
            header.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => expander.IsExpanded = !expander.IsExpanded)
            });
        }
    }

    private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var expander = (Expander)bindable;
        expander.Swap((View?)oldValue, (View?)newValue, row: 1);
        expander.ApplyExpandedState();
    }

    private static void OnIsExpandedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var expander = (Expander)bindable;
        expander.ApplyExpandedState();
        expander.ExpandedChanged?.Invoke(expander, new ExpandedChangedEventArgs((bool)newValue));
    }

    private void Swap(View? oldView, View? newView, int row)
    {
        if (oldView is not null)
            Children.Remove(oldView);

        if (newView is null)
            return;

        SetRow((BindableObject)newView, row);
        Children.Add(newView);
    }

    private void ApplyExpandedState()
    {
        if (Content is { } content)
            content.IsVisible = IsExpanded;
    }
}

/// <summary>
/// Carries the new expansion state of an <see cref="Expander"/>.
/// </summary>
public class ExpandedChangedEventArgs(bool isExpanded) : EventArgs
{
    /// <summary>
    /// Whether the expander is now expanded.
    /// </summary>
    public bool IsExpanded { get; } = isExpanded;
}
