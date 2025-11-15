using Microsoft.Maui.Controls;

namespace StokYonetimMaui;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(Views.StockManagementPage), typeof(Views.StockManagementPage));
    }
}
