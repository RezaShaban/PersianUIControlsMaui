using System.Collections.ObjectModel;

namespace MauiPersianToolkit.Extensions;

/// <summary>
/// Collection helpers used by the toolkit's view models.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Materializes <paramref name="source"/> into an <see cref="ObservableCollection{T}"/>.
    /// </summary>
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return new ObservableCollection<T>(source);
    }
}
