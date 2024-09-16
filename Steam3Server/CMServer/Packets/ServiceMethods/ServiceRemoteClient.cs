using Steam3Kit.MSG;
using Steam.Messages.RemoteClient.Service.Message;
using ModdableWebServer;

namespace Steam3Server.CMServer.Packets.ServiceMethods;

public class ServiceRemoteClient
{
    public static void Process(string name, PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        name = name.Split("#")[0];
        switch (name)
        {
            case "NotifyOnline":
                var notif  = CRemoteClient_Online_Notification.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
                //Debug.PWDebug("NotifyOnline! " + notif.ToString());
                break;
            default:
                Console.WriteLine("ServiceRemoteClient! " + name);
                break;
        }
    }
}
