using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StokYonetimMaui.Services;

namespace StokYonetimMaui.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authenticationService;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    private string _username = string.Empty;

    public LoginViewModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
        Title = "Giriş Yap";
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        await ExecuteBusyActionAsync(async () =>
        {
            ValidateAllProperties();
            if (HasErrors)
            {
                return;
            }

            await _authenticationService.LoginAsync(Username.Trim());
        });
    }
}
