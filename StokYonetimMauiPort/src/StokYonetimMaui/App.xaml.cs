using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using StokYonetimMaui.Services;
using StokYonetimMaui.ViewModels.Messages;

namespace StokYonetimMaui;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IAuthenticationService _authenticationService;

    public App(IServiceProvider serviceProvider, IAuthenticationService authenticationService)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        _authenticationService = authenticationService;

        WeakReferenceMessenger.Default.Register<UserSessionChangedMessage>(this, OnUserSessionChanged);

        MainThread.BeginInvokeOnMainThread(async () => await InitializeMainPageAsync());
    }

    private async Task InitializeMainPageAsync()
    {
        var user = await _authenticationService.GetCurrentUserAsync();
        if (user is null)
        {
            MainPage = new NavigationPage(_serviceProvider.GetRequiredService<Views.LoginPage>());
        }
        else
        {
            MainPage = _serviceProvider.GetRequiredService<AppShell>();
        }
    }

    private void OnUserSessionChanged(object recipient, UserSessionChangedMessage message)
    {
        if (message.IsLoggedIn)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                MainPage = _serviceProvider.GetRequiredService<AppShell>();
            });
        }
        else
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                MainPage = new NavigationPage(_serviceProvider.GetRequiredService<Views.LoginPage>());
            });
        }
    }
}
