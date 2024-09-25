using NetCoreServer;
using System.Net.Sockets;
using UtilsLib;

namespace Steam3Server.Servers
{
    public class SSLSession : SslSession
    {
        public SSLSession(SslServer server) : base(server) { }

        protected override void OnConnected()
        {
            Console.WriteLine($"SSL session with Id {Id} connected!");
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"SSL session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            Logger.PWLog("OnReceived");
            var server = (SSLServerBase)Server;
            string message = BitConverter.ToString(buffer[..(int)size]);
            Logger.PWLog("Incoming: " + message, $"{server.ServerName}.OnReceived");
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"SSL session caught an error with code {error}");
        }
    }

    public class SSLServerBase : SslServer
    {
        public string ServerName;
        public SSLServerBase(string servername,SslContext context, string address, int port) : base(context, address, port) 
        {
            ServerName = servername;
        }

        protected override void OnStarted()
        {
            Logger.PWLog($"{ServerName} Server Started!", $"{ServerName}.OnStarted");
        }

        protected override SslSession CreateSession() { return new SSLSession(this); }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"{ServerName} server caught an error with code {error}");
        }
    }
}
