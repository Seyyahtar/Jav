using StokYonetimMaui.ViewModels;

namespace StokYonetimMaui.Views;

public partial class ChecklistPage : ContentPage
{
    private readonly ChecklistViewModel _viewModel;

    public ChecklistPage(ChecklistViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnAppearingAsync();
    }
}
