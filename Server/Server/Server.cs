using Server.ConnectionClient;

namespace Server;

public class Server
{
    private readonly IMessageDb _messageDb;
    private readonly IUserDb _userDb;

    public Server(IMessageDb messageDb, IUserDb userDb)
    {
        _messageDb = messageDb ?? throw new ArgumentNullException(nameof(messageDb));
        _userDb = userDb ?? throw new ArgumentNullException(nameof(userDb));
        _messageDb.Added += _userDb.SendMessageToEveryone;
        _userDb.Added += _messageDb.SendMessage;
    }

    public void AddClient(IClient userClient)
    {
        userClient.MessageReceived += _messageDb.Add;
        userClient.Disconnected += ClientOnDisconnected;
        _userDb.Add(userClient);
    }

    private void ClientOnDisconnected(IClient userClient)
    {
        _userDb.SendEveryone(userClient.Data, ClientType.UserLogout);
        userClient.MessageReceived += _messageDb.Add;
        userClient.Disconnected += ClientOnDisconnected;
    }
}