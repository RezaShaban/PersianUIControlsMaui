using System.Collections;
using System.Reflection;
using MauiPersianToolkit.Enums;

namespace MauiPersianToolkit.Helpers;

/// <summary>Pure filtering used by <see cref="Controls.AutoCompleteView"/> (also unit-tested).</summary>
public static class AutoCompleteFilter
{
    public static IReadOnlyList<object> Filter(
        IEnumerable? source,
        string? query,
        string? displayProperty,
        AutoCompleteFilterMode mode,
        int maxResults,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase,
        Func<object, string, bool>? customFilter = null)
    {
        if (source is null || maxResults <= 0)
            return Array.Empty<object>();

        var q = (query ?? string.Empty).Trim();
        if (q.Length == 0)
            return Array.Empty<object>();

        PropertyInfo? displayInfo = null;
        var results = new List<object>(Math.Min(maxResults, 16));

        foreach (var item in source)
        {
            if (item is null)
                continue;

            var text = ResolveDisplay(item, displayProperty, ref displayInfo);
            if (string.IsNullOrEmpty(text))
                continue;

            var match = customFilter is not null
                ? customFilter(item, q)
                : mode switch
                {
                    AutoCompleteFilterMode.StartsWith => text.StartsWith(q, comparison),
                    _ => text.Contains(q, comparison)
                };

            if (!match)
                continue;

            results.Add(item);
            if (results.Count >= maxResults)
                break;
        }

        return results;
    }

    public static string ResolveDisplay(object item, string? displayProperty, ref PropertyInfo? cached)
    {
        if (item is string s)
            return s;

        if (string.IsNullOrWhiteSpace(displayProperty))
            return item.ToString() ?? string.Empty;

        if (cached is null
            || cached.Name != displayProperty
            || (cached.DeclaringType is not null && !cached.DeclaringType.IsInstanceOfType(item)))
        {
            cached = item.GetType().GetProperty(displayProperty,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
        }

        if (cached is null)
            return item.ToString() ?? string.Empty;

        return cached.GetValue(item)?.ToString() ?? string.Empty;
    }

    public static IReadOnlyList<string> ToDisplayList(
        IEnumerable? source,
        string? displayProperty)
    {
        if (source is null)
            return Array.Empty<string>();

        PropertyInfo? info = null;
        var list = new List<string>();
        foreach (var item in source)
        {
            if (item is null)
                continue;
            list.Add(ResolveDisplay(item, displayProperty, ref info));
        }

        return list;
    }
}
