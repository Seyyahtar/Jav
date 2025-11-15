using StokYonetimMaui.ViewModels;

namespace StokYonetimMaui.Views;

public partial class CaseEntryPage : ContentPage
{
    private readonly CaseEntryViewModel _viewModel;

    public CaseEntryPage(CaseEntryViewModel viewModel)
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
