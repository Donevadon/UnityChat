using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Server.ConnectionClient;

public class UserClient : IClient, IDisposable
{
    private readonly CancellationTokenSource _cancellation = new();
    private readonly Socket _socket;
    private UserData _data;
    
    public event Action<IData, string> MessageReceived;
    public event Action<IClient>? Disconnected;
    private Status IsConnect => _socket.Connected ? Status.Online : Status.Offline;
    public IData Data
    {
        get
        {
            _data ??= ReceiveUserDataAndStartThreadReceive();
            _data.Status = IsConnect;
            return _data;
        }
    }

    public UserClient(Socket socket)
    {
        _socket = socket;
        Disconnected += c => Console.WriteLine($"{nameof(Disconnected)}: " + c.Data.Nick);
        MessageReceived += (d, m) => Console.WriteLine($"{nameof(MessageReceived)}: {d.Nick}: {m}");
    }
    
    public void Send(object message, ClientType type)
    {
        using var stream = new MemoryStream();
        var writer = new BinaryWriter(stream);
        var reader = new BinaryReader(stream);
        writer.Write((byte) type);
        var json = JsonSerializer.Serialize(message);
        writer.Write(json);
        stream.Seek(0, SeekOrigin.Begin);
        var bytes = reader.ReadBytes((int) stream.Length);
        TrySendMessage(bytes);
    }

    private void TrySendMessage(byte[] bytes)
    {
        try
        {
            _socket.Send(bytes);
            Console.WriteLine("Sended: " + bytes.Length);
        }
        catch (SocketException e)
        {
            Disconnect();
        }
    }

    public void Dispose()
    {
        _socket.Dispose();
        _cancellation.Cancel();
    }

    private UserData ReceiveUserDataAndStartThreadReceive()
    {
        var connected = Receive(out UserData data);
        if (connected)
        {
            var receiveThread = new Thread(ReceiveMessage)
            {
                IsBackground = true
            };
            receiveThread.Start();
        }
        else
        {
            Disconnect();
        }

        return data;
    }
    
    private bool Receive<T>(out T result) where T : class
    {
        result = default!;
        var connected = Receive(out var json);
        if (connected)
        {
            Console.WriteLine($"Received: {json}");
            result = JsonSerializer.Deserialize<T>(json) ?? throw new ArgumentNullException();
        }

        return connected;
    }

    private bool Receive(out string result)
    {
        var builder = new StringBuilder();
        var data = new byte[256];

        do
        {
            var bytes = _socket.Receive(data);
            if (bytes != 0)
            {
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
        } while (_socket.Available > 0);

        result = builder.ToString();
        return _socket.Connected;
    }

    private void ReceiveMessage()
    {
        var token = _cancellation.Token;
        var timeout = 0;
        while (!token.IsCancellationRequested)
        {
            var connected = Receive(out var message);
            if (!string.IsNullOrWhiteSpace(message))
            {
                timeout = 0;
                MessageReceived?.Invoke(Data, message);
            }
            else if (!connected || timeout >= 20)
            {
                Disconnect();
            }
            else
            {
                timeout++;
            }
        }
    }

    private void Disconnect()
    {
        _socket.Disconnect(true);
        _cancellation.Cancel();
        Disconnected?.Invoke(this);
    }
}