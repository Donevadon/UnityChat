using Server.ConnectionClient;

namespace Server.Messages;

public class LocalDb : IMessageDb
{
    private const int StartCount = 20;
    private readonly List<Message> _messages = new();

    public event Action<IMessage>? Added;

    public void Add(IData data, string message)
    {
        var messageDto = new Message()
        {
            Color = data.Color,
            Nick = data.Nick,
            Text = message,
            Timestamp = DateTime.Now
        };
        _messages.Add(messageDto);
        Added?.Invoke(messageDto);
    }

    public void SendMessage(IClient client)
    {
        var messages = _messages
            .OrderBy(message => message.Timestamp)
            .Take(StartCount)
            .ToArray();

        if (messages.Length > 0)
        {
            client.Send(new Wrapper<Message>(messages), ClientType.MessageBatch);
        }
    }
}