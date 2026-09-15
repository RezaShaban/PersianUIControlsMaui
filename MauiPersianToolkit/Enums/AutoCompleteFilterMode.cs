namespace MauiPersianToolkit.Enums;

/// <summary>How <see cref="Controls.AutoCompleteView"/> matches typed text against items.</summary>
public enum AutoCompleteFilterMode
{
    /// <summary>Item text must start with the query.</summary>
    StartsWith = 0,

    /// <summary>Item text must contain the query anywhere.</summary>
    Contains = 1
}
