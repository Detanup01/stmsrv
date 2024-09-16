using NetCoreServer;
using System.Net.Sockets;
using System.Net;
using UtilsLib;

namespace Steam3Server.Servers
{
    public class UDPServerBase : UdpServer
    {
        public class UDPReceivedEvent
        {
            public EndPoint endpoint;
            public byte[] buffer;
            public long offset;
            public long size;

            public UDPReceivedEvent(EndPoint Endpoint, byte[] Buffer, long Offset, long Size)
            { 
                endpoint = Endpoint;
                buffer = Buffer;
                offset = Offset;
                size = Size;
            }
        }
        public string ServerName;
        public event EventHandler<UDPReceivedEvent>? ServerRecieved;
        public UDPServerBase(string Servername, string address, int port) : base(address, port)
        {
            ServerName = Servername;
        }
        protected override void OnStarted()
        {
            Debug.PWDebug($"{ServerName} Server Started!", $"{ServerName}.OnStarted");
            ReceiveAsync();
        } 


        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            string message = BitConverter.ToString(buffer[..(int)size]);
            Debug.PWDebug("Incoming: " + message, $"{ServerName}.OnReceived");
            ServerRecieved?.Invoke(this, new UDPReceivedEvent(endpoint,buffer,offset,size));
            ReceiveAsync();
        }

        protected override void OnSent(EndPoint endpoint, long sent) => ReceiveAsync();

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"UDP server caught an error with code {error}");
        }
    }

}
