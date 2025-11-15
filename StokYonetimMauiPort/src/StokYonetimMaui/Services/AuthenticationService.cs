using CommunityToolkit.Mvvm.Messaging;
using StokYonetimMaui.Models;
using StokYonetimMaui.ViewModels.Messages;

namespace StokYonetimMaui.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAppRepository _repository;

    public AuthenticationService(IAppRepository repository)
    {
        _repository = repository;
    }

    public Task<User?> GetCurrentUserAsync() => _repository.GetUserAsync();

    public async Task<User> LoginAsync(string username)
    {
        var user = new User
        {
            Username = username,
            LoginDate = DateTime.Now
        };

        await _repository.SaveUserAsync(user);
        WeakReferenceMessenger.Default.Send(new UserSessionChangedMessage(true));
        return user;
    }

    public async Task LogoutAsync()
    {
        await _repository.ClearUserAsync();
        WeakReferenceMessenger.Default.Send(new UserSessionChangedMessage(false));
    }
}
