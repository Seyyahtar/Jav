using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using StokYonetimMaui.Services;

namespace StokYonetimMaui.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IAppRepository _repository;
    private readonly IExcelExportService _excelExportService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private DateTime? _loginDate;

    public SettingsViewModel(IAuthenticationService authenticationService, IAppRepository repository, IExcelExportService excelExportService, IDialogService dialogService)
    {
        _authenticationService = authenticationService;
        _repository = repository;
        _excelExportService = excelExportService;
        _dialogService = dialogService;
        Title = "Ayarlar";
    }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        await LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        var user = await _authenticationService.GetCurrentUserAsync();
        Username = user?.Username ?? string.Empty;
        LoginDate = user?.LoginDate;
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _authenticationService.LogoutAsync();
    }

    [RelayCommand]
    private async Task ExportStockAsync()
    {
        var stock = await _repository.GetStockAsync();
        var path = await _excelExportService.ExportStockAsync(stock, $"stok_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        await _dialogService.ShowAlertAsync("Excel Aktarımı", $"Dosya kaydedildi: {path}", "Tamam");
    }
}
