using NetCoreServer;
using System.Net.Sockets;
using System.Net;
using UtilsLib;

namespace Steam3Server.Servers
{
    public class TCPSession : TcpSession
    {
        public TCPSession(TcpServer server) : base(server) { }

        protected override void OnConnected()
        {
            Console.WriteLine($"TCP session with Id {Id} connected!");
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"TCP session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            var server = (TCPServerBase)Server;
            string message = BitConverter.ToString(buffer[..(int)size]);
            Logger.PWLog("Incoming: " + message, $"{server.ServerName}.OnReceived");
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"TCP session caught an error with code {error}");
        }
    }

    public class TCPServerBase : TcpServer
    {
        public string ServerName;
        public TCPServerBase(string servername, IPAddress address, int port) : base(address, port)
        {
            ServerName = servername;
        }

        protected override void OnStarted()
        {
            Logger.PWLog($"{ServerName} Server Started!", $"{ServerName}.OnStarted");
        }
        protected override TcpSession CreateSession() { return new TCPSession(this); }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"TCP server caught an error with code {error}");
        }
    }
}
