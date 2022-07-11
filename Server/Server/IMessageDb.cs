namespace Server;

public interface IMessageDb
{
    void Add(IData data, string message);
    void SendMessage(IClient client);
    event Action<IMessage> Added;
}