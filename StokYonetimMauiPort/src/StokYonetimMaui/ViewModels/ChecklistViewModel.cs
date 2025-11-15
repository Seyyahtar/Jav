using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using StokYonetimMaui.Models;
using StokYonetimMaui.Services;

namespace StokYonetimMaui.ViewModels;

public partial class ChecklistViewModel : BaseViewModel
{
    private readonly IAppRepository _repository;

    [ObservableProperty]
    private ChecklistRecord? _activeChecklist;

    public ObservableCollection<ChecklistPatient> Patients { get; } = new();

    public ChecklistViewModel(IAppRepository repository)
    {
        _repository = repository;
        Title = "Kontrol Listesi";
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
            var checklist = await _repository.GetActiveChecklistAsync();
            if (checklist is null)
            {
                Patients.Clear();
                ActiveChecklist = null;
                return;
            }

            ActiveChecklist = checklist;
            Patients.Clear();
            foreach (var patient in checklist.Patients.OrderByDescending(p => p.Checked).ThenBy(p => p.Name))
            {
                Patients.Add(patient);
            }
        });
    }

    [RelayCommand]
    private async Task CreateChecklistAsync()
    {
        await ExecuteBusyActionAsync(async () =>
        {
            var title = await Application.Current!.MainPage!.DisplayPromptAsync("Kontrol Listesi", "Liste başlığı", initialValue: $"Plan {DateTime.Today:dd.MM.yyyy}");
            if (string.IsNullOrWhiteSpace(title))
            {
                return;
            }

            var checklist = new ChecklistRecord
            {
                Title = title.Trim(),
                CreatedDate = DateTime.Today,
                IsCompleted = false
            };

            await _repository.SaveChecklistAsync(checklist);
            ActiveChecklist = checklist;
            Patients.Clear();
        });
    }

    [RelayCommand]
    private async Task AddPatientAsync()
    {
        if (ActiveChecklist is null)
        {
            await Application.Current!.MainPage!.DisplayAlert("Bilgi", "Önce bir kontrol listesi oluşturun.", "Tamam");
            return;
        }

        var name = await Application.Current!.MainPage!.DisplayPromptAsync("Yeni Hasta", "Adı Soyadı");
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        var patient = new ChecklistPatient
        {
            Name = name.Trim()
        };

        ActiveChecklist.Patients.Add(patient);
        Patients.Insert(0, patient);
        await _repository.UpdateChecklistAsync(ActiveChecklist);
        await _repository.AddHistoryAsync(new HistoryRecord
        {
            Type = HistoryRecordType.Checklist,
            Description = $"{ActiveChecklist.Title} listesine yeni hasta eklendi",
            DetailsJson = JsonSerializer.Serialize(patient)
        });
    }

    [RelayCommand]
    private async Task ToggleCheckedAsync(ChecklistPatient patient)
    {
        if (ActiveChecklist is null)
        {
            return;
        }

        await _repository.UpdateChecklistAsync(ActiveChecklist);
        await LoadAsync();
    }

    [RelayCommand]
    private async Task CompleteChecklistAsync()
    {
        if (ActiveChecklist is null)
        {
            return;
        }

        ActiveChecklist.IsCompleted = true;
        ActiveChecklist.CompletedDate = DateTime.Today;
        await _repository.UpdateChecklistAsync(ActiveChecklist);
        ActiveChecklist = null;
        Patients.Clear();
    }
}
