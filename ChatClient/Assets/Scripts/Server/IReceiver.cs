using System;
using System.Net.Sockets;

namespace Server
{
    public interface IReceiver : IDisposable
    {
        void StartReceive(Socket socket);
        event Action Disconnect;
    }
}