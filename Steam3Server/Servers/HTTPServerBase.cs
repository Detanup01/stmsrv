using System.Collections.Concurrent;
using System.Net.Sockets;
using NetCoreServer;
using UtilsLib;

namespace Steam3Server.Servers
{
    public class HTTPServerSession : HttpSession
    {
        public HTTPServerSession(HttpServer server) : base(server) { }
        public Dictionary<string, string> Headers = new();
        public event EventHandler<(HttpRequest request, HTTPServerSession session)> ReceivedRequest;

        protected override void OnReceivedRequest(HttpRequest request)
        {
            Headers.Clear();
            for (int i = 0; i < request.Headers; i++)
            {
                var headerpart = request.Header(i);
                Headers.Add(headerpart.Item1.ToLower(), headerpart.Item2);
            }
            // Show HTTP request content
            if (!Headers["host"].Contains("crash.steampowered.com"))
                Debug.PWDebug(request);


            // If we have something we override!
            if (ReceivedRequest != null)
            {
                ReceivedRequest.Invoke(this, (request, this));
                return;
            }


            // Process HTTP request methods
            if (request.Method == "HEAD")
                SendResponseAsync(Response.MakeHeadResponse());
            else if (request.Method == "GET")
            {
                string key = request.Url;

                // Decode the key value
                key = Uri.UnescapeDataString(key);
                SendResponseAsync(Response.MakeGetResponse(""));
            }
            else if (request.Method == "POST" || request.Method == "PUT")
            {
                string key = request.Url;
                string value = request.Body;

                // Decode the key value
                key = Uri.UnescapeDataString(key);
                SendResponseAsync(Response.MakeOkResponse());
            }
            else if (request.Method == "DELETE")
            {
                string key = request.Url;

                // Decode the key value
                key = Uri.UnescapeDataString(key);
                SendResponseAsync(Response.MakeOkResponse());
            }
            else if (request.Method == "OPTIONS")
                SendResponseAsync(Response.MakeOptionsResponse());
            else if (request.Method == "TRACE")
                SendResponseAsync(Response.MakeTraceResponse(request.Cache.Data));
            else
                SendResponseAsync(Response.MakeErrorResponse("Unsupported HTTP method: " + request.Method));
        }

        protected override void OnReceivedRequestError(HttpRequest request, string error)
        {
            Debug.PWDebug($"Request error: {error}");
        }

        protected override void OnError(SocketError error)
        {
            Debug.PWDebug($"HTTP session caught an error: {error}");
        }
    }

    public class HTTPServerBase : HttpServer
    {
        public string ServerName;
        public ConcurrentDictionary<Guid, HTTPServerSession> Sessions = new();

        public event EventHandler<HTTPServerSession> EventConnected;
        public event EventHandler<HTTPServerSession> EventDisconnected;
        public event EventHandler EventStarted;
        public event EventHandler EventStopped;
        public HTTPServerBase(string servername, string address, int port) : base(address, port) 
        {
            ServerName = servername;
        }

        protected override TcpSession CreateSession() => new HTTPServerSession(this);

        protected override void OnConnected(TcpSession session)
        {
            EventConnected?.Invoke(this, (HTTPServerSession)session);
            Sessions.TryAdd(session.Id, (HTTPServerSession)session);
            //Debug.PWDebug($"Session Connected with ID: {session.Id} ", $"{ServerName}.OnConnected");
        }

        protected override void OnDisconnected(TcpSession session)
        {
            EventDisconnected?.Invoke(this, (HTTPServerSession)session);
            Sessions.Remove(session.Id, out _);
            //Debug.PWDebug($"Session Disconnected with ID: {session.Id} ", $"{ServerName}.OnDisconnected");
        }

        protected override void OnError(SocketError error) => Debug.PWDebug($"HTTP session caught an error: {error}");

        protected override void OnStarted() => EventStarted?.Invoke(this, null);

        protected override void OnStopped() => EventStopped?.Invoke(this, null);
    }
}
