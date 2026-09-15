using System.Collections.ObjectModel;
using PersianUISamples.Localization;

namespace PersianUISamples.Models;

public class PickerItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}

public class TreeNodeModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int? ParentId { get; set; }
}

/// <summary>Localized demo datasets rebuilt for the active gallery language.</summary>
public static class SampleData
{
    public static ObservableCollection<PickerItem> CreateCities() =>
    [
        new() { Id = 1, Title = DemoStrings.CityTehran },
        new() { Id = 2, Title = DemoStrings.CityMashhad },
        new() { Id = 3, Title = DemoStrings.CityIsfahan },
        new() { Id = 4, Title = DemoStrings.CityShiraz },
        new() { Id = 5, Title = DemoStrings.CityTabriz },
        new() { Id = 6, Title = DemoStrings.CityAhvaz },
        new() { Id = 7, Title = DemoStrings.CityKaraj },
        new() { Id = 8, Title = DemoStrings.CityQom },
        new() { Id = 9, Title = DemoStrings.CityKerman },
        new() { Id = 10, Title = DemoStrings.CityRasht },
        new() { Id = 11, Title = DemoStrings.CityYazd },
        new() { Id = 12, Title = DemoStrings.CityHamedan },
    ];

    public static ObservableCollection<PickerItem> CreatePickerItems() =>
    [
        new() { Id = 1, Title = DemoStrings.Option1, Icon = "\uf027" },
        new() { Id = 2, Title = DemoStrings.Option2, Icon = "\uf037" },
        new() { Id = 3, Title = DemoStrings.Option3, Icon = "\uf047" },
        new() { Id = 4, Title = DemoStrings.Option4, Icon = "\uf057" },
    ];

    public static ObservableCollection<TreeNodeModel> CreateTreeItems() =>
    [
        new() { Id = 1, Title = DemoStrings.TreeL11, ParentId = null },
        new() { Id = 2, Title = DemoStrings.TreeL21, ParentId = 1 },
        new() { Id = 3, Title = DemoStrings.TreeL22, ParentId = 1 },
        new() { Id = 4, Title = DemoStrings.TreeL12, ParentId = null },
        new() { Id = 5, Title = DemoStrings.TreeL21b, ParentId = 4 },
        new() { Id = 6, Title = DemoStrings.TreeL31, ParentId = 5 },
        new() { Id = 7, Title = DemoStrings.TreeL32, ParentId = 5 },
        new() { Id = 8, Title = DemoStrings.TreeL41, ParentId = 6 },
    ];
}
