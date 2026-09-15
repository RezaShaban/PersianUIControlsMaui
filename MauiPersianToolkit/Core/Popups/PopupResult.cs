namespace MauiPersianToolkit.Core;

/// <summary>
/// Outcome of a popup display.
/// </summary>
public class PopupResult
{
    internal PopupResult(bool wasDismissedByTappingOutsideOfPopup)
    {
        WasDismissedByTappingOutsideOfPopup = wasDismissedByTappingOutsideOfPopup;
    }

    /// <summary>
    /// <see langword="true"/> when the popup closed because the user tapped outside
    /// of it rather than through an explicit close call.
    /// </summary>
    public bool WasDismissedByTappingOutsideOfPopup { get; }
}

/// <summary>
/// Outcome of a popup display that produced a value.
/// </summary>
/// <typeparam name="T">Type of the value produced by the popup.</typeparam>
public class PopupResult<T> : PopupResult
{
    internal PopupResult(T? result, bool wasDismissedByTappingOutsideOfPopup)
        : base(wasDismissedByTappingOutsideOfPopup)
    {
        Result = result;
    }

    /// <summary>
    /// Value the popup was closed with, or <see langword="default"/> when the popup
    /// was dismissed without producing one.
    /// </summary>
    public T? Result { get; }
}
