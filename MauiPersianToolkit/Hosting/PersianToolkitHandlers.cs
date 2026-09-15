using MauiPersianToolkit.Controls;

namespace MauiPersianToolkit.Hosting;

/// <summary>
/// Registers the handlers of every toolkit control that renders through a native surface.
/// </summary>
public static class PersianToolkitHandlers
{
    /// <summary>
    /// Adds the toolkit's handlers to the application's handler collection. Adding a new
    /// natively-rendered control is a single line here.
    /// </summary>
    public static IMauiHandlersCollection AddPersianToolkitHandlers(this IMauiHandlersCollection handlers)
    {
        handlers.AddHandler<PersianAutoSuggest, Handlers.PersianAutoSuggestHandler>();
        // Popup is deliberately absent. It is a ContentView, so it renders through the
        // built-in ContentViewHandler when placed in a page (DatePickerView is used that
        // way). Its PopupHandler is an ElementHandler rather than an IViewHandler and is
        // created on demand by PopupPresenter; registering it here would make MAUI hand it
        // back for in-tree popups and throw "Handler must be of type IViewHandler".
        return handlers;
    }
}
