using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using StokYonetimMaui.Models;
using StokYonetimMaui.Services;

namespace StokYonetimMaui.ViewModels;

public partial class CaseEntryViewModel : BaseViewModel
{
    private readonly IAppRepository _repository;
    private readonly IDialogService _dialogService;

    public ObservableCollection<CaseMaterial> Materials { get; } = new();

    [ObservableProperty]
    private DateTime _caseDate = DateTime.Today;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Hastane adı zorunludur.")]
    private string _hospitalName = string.Empty;

    [ObservableProperty]
    private string _doctorName = string.Empty;

    [ObservableProperty]
    private string _patientName = string.Empty;

    [ObservableProperty]
    private string? _notes;

    public CaseEntryViewModel(IAppRepository repository, IDialogService dialogService)
    {
        _repository = repository;
        _dialogService = dialogService;
        Title = "Vaka Girişi";
    }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        await LoadDefaultsAsync();
    }

    private Task LoadDefaultsAsync()
    {
        if (!Materials.Any())
        {
            Materials.Clear();
        }

        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task AddMaterialAsync()
    {
        var stock = await _repository.GetStockAsync();
        if (!stock.Any())
        {
            await _dialogService.ShowAlertAsync("Bilgi", "Stokta kayıtlı malzeme bulunmuyor.", "Tamam");
            return;
        }

        var options = stock.Select(s => $"{s.MaterialName} ({s.SerialLotNumber})").ToArray();
        var selected = await _dialogService.ShowActionSheetAsync("Malzeme Seç", "İptal", null, options);
        if (string.IsNullOrWhiteSpace(selected) || selected == "İptal")
        {
            return;
        }

        var item = stock.FirstOrDefault(s => $"{s.MaterialName} ({s.SerialLotNumber})" == selected);
        if (item is null)
        {
            return;
        }

        var quantityText = await _dialogService.ShowPromptAsync(
            "Adet",
            "Kullanılan adet",
            accept: "Tamam",
            cancel: "İptal",
            initialValue: "1",
            maxLength: 3,
            keyboard: Keyboard.Numeric);
        if (!int.TryParse(quantityText, out var quantity) || quantity <= 0)
        {
            return;
        }

        Materials.Add(new CaseMaterial
        {
            MaterialName = item.MaterialName,
            SerialLotNumber = item.SerialLotNumber,
            UbbCode = item.UbbCode,
            Quantity = Math.Min(quantity, item.Quantity)
        });
    }

    [RelayCommand]
    private void RemoveMaterial(CaseMaterial material)
    {
        if (Materials.Contains(material))
        {
            Materials.Remove(material);
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        await ExecuteBusyActionAsync(async () =>
        {
            ValidateAllProperties();
            if (HasErrors)
            {
                return;
            }

            if (!Materials.Any())
            {
                await _dialogService.ShowAlertAsync("Uyarı", "En az bir malzeme seçmelisiniz.", "Tamam");
                return;
            }

            var record = new CaseRecord
            {
                Date = CaseDate,
                HospitalName = HospitalName.Trim(),
                DoctorName = DoctorName.Trim(),
                PatientName = PatientName.Trim(),
                Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim(),
                Materials = Materials.ToList()
            };

            await _repository.SaveCaseAsync(record);
            await _repository.RemoveStockQuantitiesAsync(record.Materials);
            await _repository.AddHistoryAsync(new HistoryRecord
            {
                Type = HistoryRecordType.Case,
                Description = $"{record.HospitalName} için vaka kaydedildi",
                DetailsJson = JsonSerializer.Serialize(record)
            });

            await _dialogService.ShowAlertAsync("Başarılı", "Vaka kaydı oluşturuldu.", "Tamam");
            Materials.Clear();
            HospitalName = string.Empty;
            DoctorName = string.Empty;
            PatientName = string.Empty;
            Notes = string.Empty;
        });
    }

    [RelayCommand]
    private void ClearMaterials()
    {
        Materials.Clear();
    }
}
