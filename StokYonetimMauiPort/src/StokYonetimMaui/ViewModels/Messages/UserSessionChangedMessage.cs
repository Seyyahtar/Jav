using CommunityToolkit.Mvvm.Messaging.Messages;

namespace StokYonetimMaui.ViewModels.Messages;

public class UserSessionChangedMessage : ValueChangedMessage<bool>
{
    public UserSessionChangedMessage(bool value) : base(value)
    {
    }

    public bool IsLoggedIn => Value;
}
