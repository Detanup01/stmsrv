using Steam3Kit.MSG;
using Steam3Server.Servers;
using Steam.Messages.RemoteClient.Service.Message;
using UtilsLib;

namespace Steam3Server.CMServer.CMPackets.ServiceMethods
{
    public class CMServiceRemoteClient
    {
        public static void Process(string name, PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            name = name.Split("#")[0];
            switch (name)
            {
                case "NotifyOnline":
                    var notif  = CRemoteClient_Online_Notification.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
                    //Debug.PWDebug("NotifyOnline! " + notif.ToString());
                    break;
                default:
                    break;
            }
        }
    }
}
