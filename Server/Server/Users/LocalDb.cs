using Server.ConnectionClient;

namespace Server.Users;

public class LocalDb : IUserDb
{
    private readonly List<IClient> _users = new();

    private IEnumerable<IClient> OnlineUsers => _users
        .Where(client => client.Data.Status == Status.Online)
        .ToArray();

    public event Action<IClient>? Added;

    public LocalDb()
    {
        Added += SendUsers;
        Added += NotifyUserLogin;
    }

    public void Add(IClient userClient)
    {
        if (userClient.Data.Status == Status.Online)
        {
            _users.Add(userClient);
            Added?.Invoke(userClient);
        }
    }

    public void SendEveryone(object data, ClientType type)
    {
        foreach (var user in OnlineUsers)
        {
            user.Send(data, type);
        }
    }

    private void NotifyUserLogin(IClient userClient)
    {
        foreach (var user in OnlineUsers.Where(client => client != userClient))
        {
            user.Send(userClient.Data, ClientType.User);
        }
    }

    public void SendMessageToEveryone(IMessage message)
    {
        foreach (var user in OnlineUsers)
        {
            user.Send(message, ClientType.Message);
        }
    }

    private void SendUsers(IClient client)
    {
        var usersData = _users
            .Select(item => item.Data)
            .ToArray();
        if(usersData.Length > 0)
        {
            client.Send(new Wrapper<IData>(usersData), ClientType.UserBatch);
        }
    }
}