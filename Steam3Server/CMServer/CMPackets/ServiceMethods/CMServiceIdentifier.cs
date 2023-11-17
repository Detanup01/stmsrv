using Steam3Kit.MSG;
using Steam3Server.Servers;
using UtilsLib;

namespace Steam3Server.CMServer.CMPackets.ServiceMethods
{
    public class CMServiceIdentifier
    {
        public static void Identify(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            //ServiceMethodCallFromClient
            var JobName = clientMsgProtobuf.Header.Proto.TargetJobName;
            Debug.PWDebug(JobName);
            var splitted = JobName.Split(".");
            switch (splitted[0])
            {
                case "RemoteClient":
                    CMServiceRemoteClient.Process(splitted[1], clientMsgProtobuf, sessionBase);
                    break;
                case "RemoteClientSteamClient":
                    break;
                case "Player":
                    CMServicePlayer.Process(splitted[1], clientMsgProtobuf, sessionBase);
                    break;
                case "Offline":
                    CMServiceOffline.Process(splitted[1], clientMsgProtobuf, sessionBase);
                    break;
                case "Cloud":
                    break;
                case "PublishedFile":
                    break;
                case "Credentials":
                    break;
                case "ClientMetrics":
                    break;
                case "Store":
                    break;
                case "Shader":
                    break;
                case "SteamNotification":
                    break;
                case "FriendsList":
                    break;
                case "FriendMessages":
                    break;
                case "Authentication":
                    break;
                case "CloudConfigStore":
                    break;
                case "ChatRoom":
                    break;
                case "Chat":
                    break;
                case "UserNews":
                    break;
                case "UserGameActivity":
                    break;
                case "Community":
                    break;
                case "StoreBrowse":
                    break;
                case "UserReviews":
                    break;
                case "ContentServerDirectory":
                    break;
                case "Parental":
                    CMServiceParential.Process(splitted[1], clientMsgProtobuf, sessionBase);
                    break;
                default:
                    break;
            }
            //var request = Client.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
        }
    }
}
