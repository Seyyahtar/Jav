using StokYonetimMaui.Models;

namespace StokYonetimMaui.Services;

public interface IAuthenticationService
{
    Task<User?> GetCurrentUserAsync();
    Task<User> LoginAsync(string username);
    Task LogoutAsync();
}
