using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace StokYonetimMaui.Services;

public class DialogService : IDialogService
{
    public Task ShowAlertAsync(string title, string message, string cancel)
    {
        if (!IsSupportedPlatform() || Application.Current?.MainPage is null)
        {
            return Task.CompletedTask;
        }

        return MainThread.InvokeOnMainThreadAsync(() => Application.Current!.MainPage!.DisplayAlert(title, message, cancel));
    }

    public Task<bool> ShowConfirmationAsync(string title, string message, string accept, string cancel)
    {
        if (!IsSupportedPlatform() || Application.Current?.MainPage is null)
        {
            return Task.FromResult(false);
        }

        return MainThread.InvokeOnMainThreadAsync(
            () => Application.Current!.MainPage!.DisplayAlert(title, message, accept, cancel));
    }

    public Task<string?> ShowActionSheetAsync(string title, string cancel, string? destruction, params string[] buttons)
    {
        if (!IsSupportedPlatform() || Application.Current?.MainPage is null)
        {
            return Task.FromResult<string?>(null);
        }

        return MainThread.InvokeOnMainThreadAsync(
            () => Application.Current!.MainPage!.DisplayActionSheet(title, cancel, destruction, buttons));
    }

    public Task<string?> ShowPromptAsync(
        string title,
        string message,
        string accept = "Tamam",
        string cancel = "İptal",
        string? initialValue = null,
        int maxLength = -1,
        Keyboard? keyboard = null,
        string? placeholder = null)
    {
        if (!IsSupportedPlatform() || Application.Current?.MainPage is null)
        {
            return Task.FromResult<string?>(null);
        }

        return MainThread.InvokeOnMainThreadAsync(
            () => Application.Current!.MainPage!.DisplayPromptAsync(
                title,
                message,
                accept,
                cancel,
                placeholder,
                maxLength,
                keyboard,
                initialValue));
    }

    private static bool IsSupportedPlatform() =>
        OperatingSystem.IsAndroid() ||
        OperatingSystem.IsIOS() ||
        OperatingSystem.IsMacCatalyst() ||
        OperatingSystem.IsWindows();
}
