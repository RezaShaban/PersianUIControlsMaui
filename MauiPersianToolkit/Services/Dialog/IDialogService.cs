using MauiPersianToolkit.Models;

namespace MauiPersianToolkit.Services.Dialog;

public interface IDialogService
{
    void Alert(string message, string title = "", MessageIcon icon = MessageIcon.ACCEPT, string? acceptText = null);
    void Alert(AlertConfig config);
    void Confirm(ConfirmConfig config);
    void ShowException(Exception ex);
    void CustomDialog(CustomDialogConfig config);
    void Prompt(PromptConfig config);
    void Toast(ToastConfig config);
    void Snackbar(SnackbarConfig config);

    /// <summary>Shows an alert and completes when it is closed.</summary>
    Task AlertAsync(AlertConfig config, CancellationToken cancellationToken = default);

    /// <summary>Shows an alert and completes when it is closed.</summary>
    Task AlertAsync(string message, string title = "", MessageIcon icon = MessageIcon.ACCEPT, string? acceptText = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Shows a confirm dialog. Returns <see langword="true"/> if accept was pressed;
    /// otherwise <see langword="false"/> (cancel or outside dismiss).
    /// </summary>
    Task<bool> ConfirmAsync(ConfirmConfig config, CancellationToken cancellationToken = default);

    /// <summary>
    /// Shows a prompt dialog and returns the typed result. Outside dismiss yields
    /// <see cref="PromptResult.IsOk"/> = <see langword="false"/>.
    /// </summary>
    Task<PromptResult> PromptAsync(PromptConfig config, CancellationToken cancellationToken = default);

    /// <summary>
    /// Shows a custom dialog. Returns <see langword="true"/> if accept was pressed.
    /// </summary>
    Task<bool> CustomDialogAsync(CustomDialogConfig config, CancellationToken cancellationToken = default);
}
