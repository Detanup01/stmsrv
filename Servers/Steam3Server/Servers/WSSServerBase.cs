using System.Collections.Concurrent;
using System.Net.Sockets;
using NetCoreServer;
using UtilsLib;

namespace Steam3Server.Servers
{
    public class WSSSessionBase : WssSession
    {
        public Dictionary<string, string> Headers = new();
        public event EventHandler<(byte[] bytes, WSSSessionBase session)> WsReceived;

        public event EventHandler<(HttpRequest request, WSSSessionBase session)> RequestReceived;
        public WSSSessionBase(WssServer server) : base(server) { }

        public override void OnWsReceived(byte[] buffer, long offset, long size)
        {
            Logger.PWLog("OnWsReceived!!!");
            var s = (WSSServerBase)this.Server;
            string message = BitConverter.ToString(buffer[..(int)size]);
            //Logger.PWLog("Incoming: " + message, $"{s.ServerName}.OnWsRecieved");
            WsReceived?.Invoke(this, (buffer[..(int)size], this));
        }

        protected override void OnReceivedRequest(HttpRequest request)
        {
            Logger.PWLog("OnReceivedRequest!!!");
            Headers.Clear();
            for (int i = 0; i < request.Headers; i++)
            {
                var headerpart = request.Header(i);
                Headers.Add(headerpart.Item1.ToLower(), headerpart.Item2);
            }
            Logger.PWLog(" WSSSessionBase " + request);
            if (Headers.ContainsKey("upgrade"))
            {
                base.OnReceivedRequest(request);
            }
            else
            {
                RequestReceived?.Invoke(this, (request, this));
            }

        }

        protected override void OnReceivedRequestError(HttpRequest request, string error)
        {
            Logger.PWLog($"Request error: {error}");
        }

        protected override void OnError(SocketError error)
        {
            Logger.PWLog($"HTTP session caught an error: {error}");
        }
    }

    public class WSSServerBase : WssServer
    {
        public string ServerName;
        public static ConcurrentDictionary<Guid, WSSSessionBase> Sessions = new();

        public event EventHandler<WSSSessionBase> EventConnected;
        public event EventHandler<WSSSessionBase> EventDisconnected;
        public event EventHandler EventStarted;
        public event EventHandler EventStopped;
        public WSSServerBase(string servername, SslContext context, string address, int port) : base(context, address, port)  
        {
            ServerName = servername;
        }

        protected override SslSession CreateSession() => new WSSSessionBase(this);

        protected override void OnConnected(SslSession session)
        {
            EventConnected?.Invoke(this, (WSSSessionBase)session);
            Sessions.TryAdd(session.Id, (WSSSessionBase)session);
            Logger.PWLog($"Session Connected with ID: {session.Id} ", $"{ServerName}.OnConnected");
        }

        protected override void OnDisconnected(SslSession session)
        {
            EventDisconnected?.Invoke(this, (WSSSessionBase)session);
            Sessions.Remove(session.Id, out _);
            Logger.PWLog($"Session Disconnected with ID: {session.Id} ", $"{ServerName}.OnDisconnected");
        }

        protected override void OnError(SocketError error) => Logger.PWLog($"WSS session caught an error: {error}");

        protected override void OnStarted() => EventStarted?.Invoke(this, null);

        protected override void OnStopped() => EventStopped?.Invoke(this, null);
    }
}
