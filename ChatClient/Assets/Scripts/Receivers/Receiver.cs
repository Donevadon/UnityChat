using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Server;
using UnityEngine;
using UserComponent.MessageSystem;
using UserComponent.UserSystem;

namespace Receivers
{
    public class Receiver : IReceiver, IUserReceived, IMessageReceived
    {
        private Socket _socket;
        private Thread _receiveThread;
        
        public event Action<string> UserReceived;
        public event Action<string> BatchUserReceived;
        public event Action<string> UserLogout;
        public event Action<string> MessageReceived;
        public event Action<string> BatchMessageReceived;
        public event Action Disconnect;
        

        private readonly Action<string>[] _responseActions;
        

        public Receiver()
        {
            _responseActions = new Action<string>[]
            {
                _ => Disconnect?.Invoke(),
                j => MessageReceived?.Invoke(j),
                j => UserReceived?.Invoke(j),
                j => BatchUserReceived?.Invoke(j),
                j => BatchMessageReceived?.Invoke(j),
                j => UserLogout?.Invoke(j)
            };
        }

        public void StartReceive(Socket socket)
        {
            _socket = socket;
            _receiveThread = new Thread(Start)
            {
                IsBackground = true
            };
            _receiveThread.Start();
        }
        
        private void Start()
        {
            bool connected;
            do
            {
                connected = Receive();
            } while (connected);
        }
        
        private bool Receive()
        {
            if (TryGetBinaryMessage(out var buffer))
            {
                ReadBinaryPackage(buffer);
            }
            else
            {
                Disconnect?.Invoke();
            }

            return _socket.Connected;
        }

        private bool TryGetBinaryMessage(out byte[] buffer)
        {
            var list = new List<byte>();
            int bytes;

            do
            {
                var data = new byte[256];
                bytes = _socket.Receive(data);

                list.AddRange(data.Take(bytes));
            } while (_socket.Available > 0);

            buffer = list.ToArray();
            return bytes != 0;
        }

        private void ReadBinaryPackage(byte[] buffer)
        {
            using var stream = new MemoryStream(buffer);
            using var reader = new BinaryReader(stream);

            while (reader.PeekChar() > -1)
            {
                var type = reader.ReadByte();
                var json = reader.ReadString();
                _responseActions[type].Invoke(json);
                Debug.Log($"Received: {json}");
            }
        }

        public void Dispose()
        {
            _socket?.Dispose();
            _receiveThread?.Abort();
        }
    }
}