namespace MauiPersianToolkit.Localization;

/// <summary>
/// Entry point for toolkit UI chrome strings. Controls and dialogs resolve text through here.
/// </summary>
public static class PersianToolkitStrings
{
    private static IPersianToolkitLocalizer _current = PersianToolkitLocalizer.CreateDefault();

    /// <summary>
    /// Active localizer. Replace to plug in your own translations.
    /// </summary>
    public static IPersianToolkitLocalizer Current
    {
        get => _current;
        set => _current = value ?? throw new ArgumentNullException(nameof(value));
    }

    public static string Accept => Current[PersianToolkitStringId.Accept];
    public static string Cancel => Current[PersianToolkitStringId.Cancel];
    public static string Confirm => Current[PersianToolkitStringId.Confirm];
    public static string Ok => Current[PersianToolkitStringId.Ok];
    public static string SystemErrorTitle => Current[PersianToolkitStringId.SystemErrorTitle];
    public static string SelectDate => Current[PersianToolkitStringId.SelectDate];
    public static string SelectTime => Current[PersianToolkitStringId.SelectTime];
    public static string Now => Current[PersianToolkitStringId.Now];
    public static string Hour => Current[PersianToolkitStringId.Hour];
    public static string Minute => Current[PersianToolkitStringId.Minute];
    public static string Second => Current[PersianToolkitStringId.Second];
    public static string Am => Current[PersianToolkitStringId.Am];
    public static string Pm => Current[PersianToolkitStringId.Pm];
    public static string Today => Current[PersianToolkitStringId.Today];
    public static string NoResults => Current[PersianToolkitStringId.NoResults];
    public static string TypeToSearch => Current[PersianToolkitStringId.TypeToSearch];

    /// <summary>
    /// Convenience alias for <see cref="Current"/>.<see cref="IPersianToolkitLocalizer.Get"/>.
    /// </summary>
    public static string Get(string id) => Current.Get(id);

    /// <summary>
    /// Switches the UI culture when the active localizer is the built-in
    /// <see cref="PersianToolkitLocalizer"/>.
    /// </summary>
    public static void SetCulture(string cultureName)
    {
        if (Current is PersianToolkitLocalizer localizer)
            localizer.SetCulture(cultureName);
        else
            throw new InvalidOperationException(
                "SetCulture requires the built-in PersianToolkitLocalizer. Replace Current or call SetCulture on your custom localizer.");
    }
}
