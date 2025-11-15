using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using StokYonetimMaui.Models;
using StokYonetimMaui.Services;

namespace StokYonetimMaui.ViewModels;

public partial class HistoryViewModel : BaseViewModel
{
    private readonly IAppRepository _repository;

    public ObservableCollection<HistoryRecord> Records { get; } = new();

    public HistoryViewModel(IAppRepository repository)
    {
        _repository = repository;
        Title = "Geçmiş";
    }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        await LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        await ExecuteBusyActionAsync(async () =>
        {
            var history = await _repository.GetHistoryAsync();
            Records.Clear();
            foreach (var record in history)
            {
                Records.Add(record);
            }
        });
    }

    [RelayCommand]
    private async Task RemoveAsync(HistoryRecord record)
    {
        var confirm = await Application.Current!.MainPage!.DisplayAlert("Onay", "Kaydı silmek istediğinize emin misiniz?", "Evet", "Hayır");
        if (!confirm)
        {
            return;
        }

        await _repository.RemoveHistoryAsync(record.Id);
        Records.Remove(record);
    }

    [RelayCommand]
    private Task ShowDetailsAsync(HistoryRecord record)
    {
        string details = string.Empty;
        if (!string.IsNullOrWhiteSpace(record.DetailsJson))
        {
            try
            {
                var parsed = JsonSerializer.Deserialize<object>(record.DetailsJson);
                details = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });
            }
            catch
            {
                details = record.DetailsJson;
            }
        }

        return Application.Current!.MainPage!.DisplayAlert(record.Description, details, "Kapat");
    }
}
