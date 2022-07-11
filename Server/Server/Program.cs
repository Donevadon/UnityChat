using System.Net;
using System.Net.Sockets;
using Server.ConnectionClient;

namespace Server;

internal class Program
{
    private const string Ip = "127.0.0.1";
    private const int Port = 5000;
    private static Thread? _serverThread;
    private static readonly bool _working = true;
    private static readonly CancellationTokenSource Source = new();

    private static void Main(string[] args)
    {
        _serverThread = new Thread(StartServer)
        {
            IsBackground = true
        };
        _serverThread.Start();
        while (_working)
        {
            var command = Console.ReadLine();
        }

        Source.Cancel();
    }

    private static void StartServer()
    {
        Console.WriteLine("Server started");
        var token = Source.Token;
        var ipAddress = IPAddress.Parse(Ip);
        var endPoint = new IPEndPoint(ipAddress, Port);
        var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        var server = new Server(new Messages.LocalDb(), new Users.LocalDb());
        socket.Bind(endPoint);
        socket.Listen(100);
        Console.WriteLine("Listen: " + endPoint);
        while (!token.IsCancellationRequested)
        {
            var user = socket.Accept();
            Console.WriteLine("User accepted");
            server.AddClient(new UserClient(user));
        }
        Console.WriteLine("Server stopped");
    }
}