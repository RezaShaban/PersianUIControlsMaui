using MauiPersianToolkit.Localization;

namespace MauiPersianToolkit;

/// <summary>
/// XAML markup extension that resolves a toolkit chrome string for the active culture.
/// </summary>
/// <example>
/// <code>
/// Text="{mpt:Localize Today}"
/// Text="{mpt:Localize Key=SelectDate}"
/// </code>
/// </example>
[ContentProperty(nameof(Key))]
[AcceptEmptyServiceProvider]
public sealed class LocalizeExtension : IMarkupExtension<string>
{
    /// <summary>
    /// Localization key — use a <see cref="PersianToolkitStringId"/> constant name
    /// (e.g. <c>Today</c>, <c>Cancel</c>) or the full key string.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    public string ProvideValue(IServiceProvider serviceProvider)
    {
        if (string.IsNullOrWhiteSpace(Key))
            return string.Empty;

        // Allow Key="Today" (member name) as well as Key="Today" matching the id value.
        var id = ResolveId(Key.Trim());
        return PersianToolkitStrings.Get(id);
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) =>
        ProvideValue(serviceProvider);

    private static string ResolveId(string key)
    {
        // Prefer matching known constant field names so XAML can use {mpt:Localize Today}.
        var field = typeof(PersianToolkitStringId).GetField(key);
        if (field is not null && field.IsLiteral && !field.IsInitOnly && field.FieldType == typeof(string))
            return (string)field.GetRawConstantValue()!;

        return key;
    }
}
