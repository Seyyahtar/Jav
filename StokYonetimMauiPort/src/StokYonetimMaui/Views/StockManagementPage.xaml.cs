using StokYonetimMaui.ViewModels;

namespace StokYonetimMaui.Views;

public partial class StockManagementPage : ContentPage, IQueryAttributable
{
    private readonly StockManagementViewModel _viewModel;

    public StockManagementPage(StockManagementViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _viewModel.ApplyQueryAttributes(query);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnAppearingAsync();
    }
}
