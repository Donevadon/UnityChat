using System;

namespace UserComponent.UserSystem
{
    public interface IUserReceived
    {
        event Action<string> UserReceived;
        event Action<string> BatchUserReceived;
        event Action<string> UserLogout;
    }
}