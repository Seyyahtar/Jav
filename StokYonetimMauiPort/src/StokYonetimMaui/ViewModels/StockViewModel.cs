using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using StokYonetimMaui.Models;
using StokYonetimMaui.Services;
using StokYonetimMaui.Views;

namespace StokYonetimMaui.ViewModels;

public partial class StockViewModel : BaseViewModel
{
    private readonly IAppRepository _repository;
    private readonly IExcelExportService _excelExportService;
    private readonly IDialogService _dialogService;

    public ObservableCollection<StockItem> Items { get; } = new();

    [ObservableProperty]
    private StockItem? _selectedItem;

    public StockViewModel(IAppRepository repository, IExcelExportService excelExportService, IDialogService dialogService)
    {
        _repository = repository;
        _excelExportService = excelExportService;
        _dialogService = dialogService;
        Title = "Stok";
    }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        await LoadAsync();
    }

    [RelayCommand]
    private Task LoadAsync() => ExecuteBusyActionAsync(RefreshDataAsync);

    private async Task RefreshDataAsync()
    {
        var items = await _repository.GetStockAsync();
        Items.Clear();
        foreach (var item in items)
        {
            Items.Add(item);
        }
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        await Shell.Current.GoToAsync(nameof(StockManagementPage));
    }

    [RelayCommand]
    private async Task EditAsync(StockItem item)
    {
        var parameters = new Dictionary<string, object>
        {
            ["StockItemJson"] = JsonSerializer.Serialize(item)
        };
        await Shell.Current.GoToAsync(nameof(StockManagementPage), parameters);
    }

    [RelayCommand]
    private async Task DeleteAsync(StockItem item)
    {
        if (item is null)
        {
            return;
        }

        await ExecuteBusyActionAsync(async () =>
        {
            await _repository.DeleteStockItemAsync(item.Id);
            await _repository.AddHistoryAsync(new HistoryRecord
            {
                Type = HistoryRecordType.StockDelete,
                Description = $"{item.MaterialName} - {item.SerialLotNumber} stoktan kaldırıldı.",
                DetailsJson = JsonSerializer.Serialize(item)
            });
            await RefreshDataAsync();
        });
    }

    [RelayCommand]
    private async Task ExportAsync()
    {
        await ExecuteBusyActionAsync(async () =>
        {
            var path = await _excelExportService.ExportStockAsync(Items, $"stok_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
            await _dialogService.ShowAlertAsync("Excel Aktarımı", $"Dosya kaydedildi: {path}", "Tamam");
        });
    }
}
