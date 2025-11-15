using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using StokYonetimMaui.Models;
using StokYonetimMaui.Services;

namespace StokYonetimMaui.ViewModels;

public partial class StockManagementViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IAppRepository _repository;

    [ObservableProperty]
    private string _id = Guid.NewGuid().ToString();

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Malzeme adı zorunludur.")]
    private string _materialName = string.Empty;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Seri/Lot numarası zorunludur.")]
    private string _serialLotNumber = string.Empty;

    [ObservableProperty]
    private string _ubbCode = string.Empty;

    [ObservableProperty]
    private DateTime _expiryDate = DateTime.Today;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(1, int.MaxValue, ErrorMessage = "Adet 1 veya daha büyük olmalıdır.")]
    private int _quantity = 1;

    [ObservableProperty]
    private DateTime _dateAdded = DateTime.Today;

    [ObservableProperty]
    private string? _from;

    [ObservableProperty]
    private string? _to;

    [ObservableProperty]
    private string? _materialCode;

    [ObservableProperty]
    private bool _isEditMode;

    public StockManagementViewModel(IAppRepository repository)
    {
        _repository = repository;
        Title = "Stok Yönetimi";
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("StockItemJson", out var jsonObj) && jsonObj is string json)
        {
            var item = JsonSerializer.Deserialize<StockItem>(json);
            if (item is null)
            {
                return;
            }

            IsEditMode = true;
            Id = item.Id;
            MaterialName = item.MaterialName;
            SerialLotNumber = item.SerialLotNumber;
            UbbCode = item.UbbCode;
            ExpiryDate = item.ExpiryDate;
            Quantity = item.Quantity;
            DateAdded = item.DateAdded;
            From = item.From;
            To = item.To;
            MaterialCode = item.MaterialCode;
        }
        else
        {
            ResetForm();
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

            if (await _repository.StockHasDuplicateAsync(MaterialName, SerialLotNumber, IsEditMode ? Id : null))
            {
                await Application.Current!.MainPage!.DisplayAlert("Uyarı", "Bu malzeme ve seri/lot numarası zaten kayıtlı.", "Tamam");
                return;
            }

            var item = new StockItem
            {
                Id = Id,
                MaterialName = MaterialName.Trim(),
                SerialLotNumber = SerialLotNumber.Trim(),
                UbbCode = UbbCode.Trim(),
                ExpiryDate = ExpiryDate,
                Quantity = Quantity,
                DateAdded = DateAdded,
                From = string.IsNullOrWhiteSpace(From) ? null : From.Trim(),
                To = string.IsNullOrWhiteSpace(To) ? null : To.Trim(),
                MaterialCode = string.IsNullOrWhiteSpace(MaterialCode) ? null : MaterialCode.Trim()
            };

            await _repository.SaveStockItemAsync(item);
            await _repository.AddHistoryAsync(new HistoryRecord
            {
                Type = IsEditMode ? HistoryRecordType.StockAdd : HistoryRecordType.StockAdd,
                Description = IsEditMode
                    ? $"{item.MaterialName} güncellendi"
                    : $"{item.MaterialName} stoğa eklendi",
                DetailsJson = JsonSerializer.Serialize(item)
            });

            await Shell.Current.GoToAsync("..", true);
        });
    }

    [RelayCommand]
    private Task CancelAsync()
    {
        ResetForm();
        return Shell.Current.GoToAsync("..", true);
    }

    private void ResetForm()
    {
        IsEditMode = false;
        Id = Guid.NewGuid().ToString();
        MaterialName = string.Empty;
        SerialLotNumber = string.Empty;
        UbbCode = string.Empty;
        ExpiryDate = DateTime.Today;
        Quantity = 1;
        DateAdded = DateTime.Today;
        From = null;
        To = null;
        MaterialCode = null;
    }
}
