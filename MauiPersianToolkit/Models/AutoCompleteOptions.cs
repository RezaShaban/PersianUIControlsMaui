using MauiPersianToolkit.Enums;

namespace MauiPersianToolkit.Models;

/// <summary>
/// Tunables for <see cref="Controls.AutoCompleteView"/>. Bind or mutate the instance;
/// the control reacts without recreating chrome.
/// </summary>
public sealed class AutoCompleteOptions
{
    /// <summary>Minimum typed characters before suggestions appear. Default 1.</summary>
    public int MinimumPrefixLength { get; set; } = 1;

    /// <summary>Hard cap on visible suggestions. Default 8.</summary>
    public int MaxSuggestions { get; set; } = 8;

    /// <summary>Max height of the managed dropdown panel (DIP). Default 220.</summary>
    public double MaxDropdownHeight { get; set; } = 220;

    /// <summary>StartsWith vs Contains matching.</summary>
    public AutoCompleteFilterMode FilterMode { get; set; } = AutoCompleteFilterMode.Contains;

    /// <summary>Case-insensitive by default.</summary>
    public bool IgnoreCase { get; set; } = true;

    /// <summary>
    /// When <see langword="true"/>, use platform suggest UI
    /// (Android <c>AutoCompleteTextView</c>, Windows <c>AutoSuggestBox</c>).
    /// iOS still uses the managed dropdown. Default <see langword="true"/>.
    /// </summary>
    public bool UseNativeSuggestions { get; set; } = true;

    /// <summary>Show the clear (×) affordance when text is non-empty.</summary>
    public bool ShowClearButton { get; set; } = true;

    /// <summary>Open the dropdown when the field gains focus (if prefix length allows).</summary>
    public bool OpenOnFocus { get; set; } = true;

    /// <summary>Close suggestions after a selection.</summary>
    public bool CloseOnSelect { get; set; } = true;

    /// <summary>Debounce for filter passes (ms). 0 = immediate. Default 120.</summary>
    public int FilterDebounceMs { get; set; } = 120;

    /// <summary>Placeholder shown inside the native field (optional; falls back to control PlaceHolder).</summary>
    public string? FieldPlaceholder { get; set; }

    /// <summary>Empty-state caption when the query has no matches. Null uses localized NoResults.</summary>
    public string? EmptyText { get; set; }

    /// <summary>Custom predicate; when set, <see cref="FilterMode"/> is ignored.</summary>
    public Func<object, string, bool>? CustomFilter { get; set; }

    /// <summary>Optional async suggestion provider (e.g. remote search). Receives the query.</summary>
    public Func<string, CancellationToken, Task<IReadOnlyList<object>>>? SuggestionProvider { get; set; }
}
