using System;
using System.Net.Sockets;
using System.Text;
using JetBrains.Annotations;
using MonoBehaviorClient;
using UnityEngine;

namespace Server
{
    public class ConnectionServer : IConnection
    {
        private const string IP = "127.0.0.1";
        private const int Port = 5000;
        private readonly IMonoBehaviorAsync<Action> _async;
        private readonly IConnectionWindow _connectionWindow;
        private readonly IReceiver _receiver;
        private readonly Socket _socket;

        public ConnectionServer([NotNull] IConnectionWindow connectionWindow, [NotNull] IReceiver receiver,
            IMonoBehaviorAsync<Action> async)
        {
            _connectionWindow = connectionWindow ?? throw new ArgumentNullException(nameof(connectionWindow));
            _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
            _async = async;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            _connectionWindow.OnLogin += LoginHandler;
            _connectionWindow.Open();
        }

        public void SendMessage(string inputText)
        {
            Send(inputText);
        }

        public void Dispose()
        {
            _receiver?.Dispose();
            _socket?.Dispose();
            _connectionWindow.OnLogin -= LoginHandler;
        }

        private async void LoginHandler(string nick, string color)
        {
            try
            {
                _connectionWindow.OnLogin -= LoginHandler;
                _connectionWindow.Close();
                await _socket.ConnectAsync(IP, Port)
                    .ContinueWith(_ => Send(new NewUser(nick, color)));
                _receiver.StartReceive(_socket);
                _receiver.Disconnect += OnDisconnect;
            }
            catch (Exception e)
            {
                OnDisconnect();
            }
        }

        private void Send<T>(T user)
        {
            var json = JsonUtility.ToJson(user);
            Send(json);
        }

        private void OnDisconnect()
        {
            _connectionWindow.OnLogin += LoginHandler;
            _socket.Disconnect(true);
            _async.Add(() => _connectionWindow.Open());
        }

        private void Send(string str)
        {
            try
            {
                var data = Encoding.Unicode.GetBytes(str);
                _socket.Send(data);
            }
            catch (SocketException e)
            {
                OnDisconnect();
            }
        }
    }
}