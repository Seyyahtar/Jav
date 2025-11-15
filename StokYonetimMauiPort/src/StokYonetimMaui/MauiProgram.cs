using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using StokYonetimMaui.Services;
using StokYonetimMaui.ViewModels;
using StokYonetimMaui.Views;

namespace StokYonetimMaui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                // Varsayılan sistem fontlarını kullanıyoruz.
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<App>();
        builder.Services.AddSingleton<AppShell>();

        builder.Services.AddSingleton<IFileSystemService, FileSystemService>();
        builder.Services.AddSingleton<IDatabaseInitializer, SqliteDatabaseInitializer>();
        builder.Services.AddSingleton<IAppRepository, AppRepository>();
        builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
        builder.Services.AddSingleton<IExcelExportService, ClosedXmlExcelExportService>();

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<LoginViewModel>();

        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<HomeViewModel>();

        builder.Services.AddSingleton<StockPage>();
        builder.Services.AddSingleton<StockViewModel>();

        builder.Services.AddTransient<StockManagementPage>();
        builder.Services.AddTransient<StockManagementViewModel>();

        builder.Services.AddSingleton<CaseEntryPage>();
        builder.Services.AddSingleton<CaseEntryViewModel>();

        builder.Services.AddSingleton<ChecklistPage>();
        builder.Services.AddSingleton<ChecklistViewModel>();

        builder.Services.AddSingleton<HistoryPage>();
        builder.Services.AddSingleton<HistoryViewModel>();

        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddSingleton<SettingsViewModel>();

        return builder.Build();
    }
}
