using Steam3Kit.MSG;
using Steam3Server.Servers;

namespace Steam3Server.CMServer.CMPackets.ServiceMethods
{
    public class CMServiceIdentifier
    {
        public static void Identify(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            //ServiceMethodCallFromClient
            var rsp = new byte[] { };
            var JobName = clientMsgProtobuf.Header.Proto.TargetJobName;
            var splitted = JobName.Split(".");
            switch (splitted[0])
            {
                case "RemoteClient":
                    CMServiceRemoteClient.Process(splitted[1], clientMsgProtobuf, sessionBase);
                    break;
                case "Player":
                    CMServicePlayer.Process(splitted[1], clientMsgProtobuf, sessionBase);
                    break;
                case "Offline":
                    break;
                default:    //Shader, ClientMetrics, 
                    break;
            }
            //var request = Client.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
        }
    }
}
