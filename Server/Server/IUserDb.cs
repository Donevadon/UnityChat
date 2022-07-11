using Server.ConnectionClient;

namespace Server;

public interface IUserDb
{
    void Add(IClient userClient);
    void SendEveryone(object data, ClientType userLogout);
    void SendMessageToEveryone(IMessage message);
    event Action<IClient> Added;
}