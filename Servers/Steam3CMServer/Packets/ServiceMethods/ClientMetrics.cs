using ModdableWebServer;
using Steam.ClientMetrics;
using Steam.Messages.ClientMetrics;
using Steam3Kit.MSG;

namespace Steam3Server.CMServer.Packets.ServiceMethods;

public class ClientMetrics
{
    public static void Process(string name, PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        name = name.Split("#")[0];
        switch (name)
        {
            case "ClientBootstrapReport":
                ClientBootstrapReport(clientMsgProtobuf, webSocket);
                break;
            case "ClientIPv6ConnectivityReport":
                ClientIPv6ConnectivityReport(clientMsgProtobuf, webSocket);
                break;
            default:
                Console.WriteLine("ServiceOffline! " + name);
                break;
        }
    }

    private static void ClientIPv6ConnectivityReport(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
       var req = CClientMetrics_IPv6Connectivity_Notification.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
        if (!Directory.Exists("ServiceMethods"))
            Directory.CreateDirectory("ServiceMethods");
        File.AppendAllText("ServiceMethods/ClientIPv6ConnectivityReport.txt", req.ToString() + "\n");
    }

    private static void ClientBootstrapReport(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        
        var req = clientMsgProtobuf.GetBody<CClientMetrics_ClientBootstrap_Notification>();
        if (!Directory.Exists("ServiceMethods"))
            Directory.CreateDirectory("ServiceMethods");
        File.AppendAllText("ServiceMethods/ClientBootstrapReport.txt", req.ToString() + "\n");
    }
}
