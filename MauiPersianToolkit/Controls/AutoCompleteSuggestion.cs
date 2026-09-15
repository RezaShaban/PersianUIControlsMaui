namespace MauiPersianToolkit.Controls;

/// <summary>One row in the managed AutoComplete dropdown.</summary>
public sealed class AutoCompleteSuggestion
{
    public AutoCompleteSuggestion(object item, string display)
    {
        Item = item;
        Display = display;
    }

    public object Item { get; }
    public string Display { get; }
    public override string ToString() => Display;
}
