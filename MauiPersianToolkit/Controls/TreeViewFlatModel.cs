using MauiPersianToolkit.Enums;
using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;

namespace MauiPersianToolkit.Controls;

/// <summary>
/// Pure flatten/expand model for <see cref="TreeView"/> — no UI dispatcher required.
/// </summary>
public sealed class TreeViewFlatModel
{
    private static readonly object NullParentKey = new();

    private readonly List<TreeViewItem> _allNodes = [];
    private readonly Dictionary<object, List<TreeViewItem>> _childrenByParent = new();
    private Type? _itemType;
    private PropertyInfo? _keyProp;
    private PropertyInfo? _parentProp;
    private PropertyInfo? _displayProp;

    public ObservableCollection<TreeViewItem> VisibleRows { get; } = [];

    public void Rebuild(
        IList? itemsSource,
        string? keyProperty,
        string? parentProperty,
        string? displayProperty,
        TreeViewSelectionMode selectionMode,
        IList? selectedItems)
    {
        VisibleRows.Clear();
        _allNodes.Clear();
        _childrenByParent.Clear();
        _itemType = null;
        _keyProp = _parentProp = _displayProp = null;

        if (itemsSource is null || string.IsNullOrWhiteSpace(keyProperty))
            return;

        var selectedKeys = new HashSet<object>();
        if (selectedItems is not null)
        {
            foreach (var selected in selectedItems.OfType<object>())
            {
                var key = ReadKey(selected, keyProperty, parentProperty, displayProperty);
                if (key is not null)
                    selectedKeys.Add(key);
            }
        }

        foreach (var raw in itemsSource.OfType<object>())
        {
            EnsureAccessors(raw, keyProperty!, parentProperty, displayProperty);
            var id = _keyProp?.GetValue(raw);
            if (id is null)
                continue;

            var parentId = _parentProp?.GetValue(raw);
            var node = new TreeViewItem
            {
                Id = id,
                ParentId = parentId,
                OriginalItem = raw,
                Title = _displayProp?.GetValue(raw)?.ToString() ?? raw.ToString() ?? string.Empty,
                SelectionMode = selectionMode,
                IsSelected = selectedKeys.Contains(id),
                Depth = 0
            };
            _allNodes.Add(node);

            var parentKey = parentId ?? NullParentKey;
            if (!_childrenByParent.TryGetValue(parentKey, out var list))
            {
                list = [];
                _childrenByParent[parentKey] = list;
            }

            list.Add(node);
        }

        foreach (var node in _allNodes)
        {
            node.HasChildren = _childrenByParent.TryGetValue(node.Id, out var kids) && kids.Count > 0;
            node.ChildItems = node.HasChildren ? _childrenByParent[node.Id] : [];
        }

        if (_childrenByParent.TryGetValue(NullParentKey, out var roots))
        {
            foreach (var root in roots)
            {
                root.Depth = 0;
                VisibleRows.Add(root);
            }
        }
    }

    public void ToggleExpand(TreeViewItem item)
    {
        if (!item.HasChildren)
            return;

        item.IsExpanded = !item.IsExpanded;
        if (item.IsExpanded)
            InsertChildren(item);
        else
            RemoveDescendants(item);
    }

    private void InsertChildren(TreeViewItem parent)
    {
        if (!_childrenByParent.TryGetValue(parent.Id, out var children) || children.Count == 0)
            return;

        var index = VisibleRows.IndexOf(parent);
        if (index < 0)
            return;

        var insertAt = index + 1;
        foreach (var child in children)
        {
            child.Depth = parent.Depth + 1;
            child.IsExpanded = false;
            VisibleRows.Insert(insertAt++, child);
        }
    }

    private void RemoveDescendants(TreeViewItem parent)
    {
        var index = VisibleRows.IndexOf(parent);
        if (index < 0)
            return;

        var removeFrom = index + 1;
        while (removeFrom < VisibleRows.Count && VisibleRows[removeFrom].Depth > parent.Depth)
        {
            VisibleRows[removeFrom].IsExpanded = false;
            VisibleRows.RemoveAt(removeFrom);
        }
    }

    private object? ReadKey(object item, string keyProperty, string? parentProperty, string? displayProperty)
    {
        EnsureAccessors(item, keyProperty, parentProperty, displayProperty);
        return _keyProp?.GetValue(item);
    }

    private void EnsureAccessors(object sample, string keyProperty, string? parentProperty, string? displayProperty)
    {
        var type = sample.GetType();
        if (_itemType == type && _keyProp is not null)
            return;

        _itemType = type;
        _keyProp = type.GetProperty(keyProperty);
        _parentProp = string.IsNullOrWhiteSpace(parentProperty) ? null : type.GetProperty(parentProperty);
        _displayProp = string.IsNullOrWhiteSpace(displayProperty) ? null : type.GetProperty(displayProperty);
    }
}
