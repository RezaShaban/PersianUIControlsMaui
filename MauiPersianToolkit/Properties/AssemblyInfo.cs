using MauiPersianToolkit;

// A single XAML namespace for every public type in the toolkit. Consumers declare
// xmlns:mpt="http://schemas.mauipersiantoolkit.com/2026/toolkit" once, which also leaves
// the toolkit free to move types between CLR namespaces without breaking their markup.
[assembly: XmlnsDefinition(PersianToolkit.XamlNamespace, "MauiPersianToolkit")]
[assembly: XmlnsDefinition(PersianToolkit.XamlNamespace, "MauiPersianToolkit.Behaviors")]
[assembly: XmlnsDefinition(PersianToolkit.XamlNamespace, "MauiPersianToolkit.Controls")]
[assembly: XmlnsDefinition(PersianToolkit.XamlNamespace, "MauiPersianToolkit.Converters")]
[assembly: XmlnsDefinition(PersianToolkit.XamlNamespace, "MauiPersianToolkit.Core")]
[assembly: XmlnsDefinition(PersianToolkit.XamlNamespace, "MauiPersianToolkit.Dialogs")]
[assembly: XmlnsDefinition(PersianToolkit.XamlNamespace, "MauiPersianToolkit.Enums")]
[assembly: XmlnsDefinition(PersianToolkit.XamlNamespace, "MauiPersianToolkit.Localization")]
[assembly: XmlnsDefinition(PersianToolkit.XamlNamespace, "MauiPersianToolkit.Models")]
[assembly: XmlnsDefinition(PersianToolkit.XamlNamespace, "MauiPersianToolkit.ViewModels")]
[assembly: Microsoft.Maui.Controls.XmlnsPrefix(PersianToolkit.XamlNamespace, "mpt")]
