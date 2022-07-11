using System;

namespace MonoBehaviorClient
{
    internal interface IConnection: IDisposable
    {
        void Connect();
        void SendMessage(string inputText);
    }
}