using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StokYonetimMaui.Models;
using StokYonetimMaui.Services;

namespace StokYonetimMaui.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly IAppRepository _repository;
    private readonly IAuthenticationService _authenticationService;

    public ObservableCollection<StockItem> ExpiringSoon { get; } = new();

    [ObservableProperty]
    private string _welcomeText = string.Empty;

    [ObservableProperty]
    private int _stockCount;

    [ObservableProperty]
    private int _caseCount;

    [ObservableProperty]
    private int _checklistCount;

    public HomeViewModel(IAppRepository repository, IAuthenticationService authenticationService)
    {
        _repository = repository;
        _authenticationService = authenticationService;
        Title = "Ana Sayfa";
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
            var user = await _authenticationService.GetCurrentUserAsync();
            WelcomeText = user is null
                ? "Hoş geldiniz"
                : $"Hoş geldiniz, {user.Username}!";

            var stock = await _repository.GetStockAsync();
            var cases = await _repository.GetCasesAsync();
            var checklists = await _repository.GetChecklistsAsync();

            StockCount = stock.Sum(s => s.Quantity);
            CaseCount = cases.Count;
            ChecklistCount = checklists.Count;

            ExpiringSoon.Clear();
            foreach (var item in stock.Where(s => s.ExpiryDate <= DateTime.Today.AddMonths(1)).OrderBy(s => s.ExpiryDate).Take(5))
            {
                ExpiringSoon.Add(item);
            }
        });
    }
}
