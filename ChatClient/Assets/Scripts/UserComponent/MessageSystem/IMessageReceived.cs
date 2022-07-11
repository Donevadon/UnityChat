using System;

namespace UserComponent.MessageSystem
{
    public interface IMessageReceived
    {
        event Action<string> MessageReceived;
        event Action<string> BatchMessageReceived;
    }
}