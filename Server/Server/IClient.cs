using Server.ConnectionClient;

namespace Server;

public interface IClient
{
    void Send(object obj, ClientType type);
    event Action<IData, string> MessageReceived;
    event Action<IClient> Disconnected;
    IData Data { get;}
}