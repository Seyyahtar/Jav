namespace StokYonetimMaui.Services;

using Microsoft.Maui.Controls;

public interface IDialogService
{
    Task ShowAlertAsync(string title, string message, string cancel);

    Task<bool> ShowConfirmationAsync(string title, string message, string accept, string cancel);

    Task<string?> ShowActionSheetAsync(string title, string cancel, string? destruction, params string[] buttons);

    Task<string?> ShowPromptAsync(
        string title,
        string message,
        string accept = "Tamam",
        string cancel = "İptal",
        string? initialValue = null,
        int maxLength = -1,
        Keyboard? keyboard = null,
        string? placeholder = null);
}
