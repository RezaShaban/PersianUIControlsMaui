namespace MauiPersianToolkit.Localization;

/// <summary>
/// Resolves localized UI chrome strings for the toolkit.
/// </summary>
public interface IPersianToolkitLocalizer
{
    /// <summary>
    /// Culture name currently used for lookups (e.g. <c>fa</c>, <c>en</c>).
    /// </summary>
    string CultureName { get; }

    /// <summary>
    /// Returns the string for <paramref name="id"/> in the active culture,
    /// falling back to Persian, then to the key itself.
    /// </summary>
    string Get(string id);

    /// <summary>
    /// Same as <see cref="Get"/>.
    /// </summary>
    string this[string id] { get; }
}
