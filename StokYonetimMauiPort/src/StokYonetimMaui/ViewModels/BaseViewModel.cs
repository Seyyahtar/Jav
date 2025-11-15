using CommunityToolkit.Mvvm.ComponentModel;

namespace StokYonetimMaui.ViewModels;

public partial class BaseViewModel : ObservableValidator
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _title = string.Empty;

    public virtual Task OnAppearingAsync() => Task.CompletedTask;

    protected async Task ExecuteBusyActionAsync(Func<Task> action)
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            await action();
        }
        finally
        {
            IsBusy = false;
        }
    }
}
