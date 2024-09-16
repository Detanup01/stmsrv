using ModdableWebServer;
using Steam3Kit.MSG;
using UtilsLib;

namespace Steam3Server.CMServer.Packets.ServiceMethods;

public class ServiceIdentifier
{
    public static void Identify(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        //ServiceMethodCallFromClient
        var JobName = clientMsgProtobuf.Header.Proto.TargetJobName;
        Debug.PWDebug(JobName);
        var splitted = JobName.Split(".");
        switch (splitted[0])
        {
            case "RemoteClient":
                ServiceRemoteClient.Process(splitted[1], clientMsgProtobuf, webSocket);
                break;
            case "RemoteClientSteamClient":
                break;
            case "Player":
                ServicePlayer.Process(splitted[1], clientMsgProtobuf, webSocket);
                break;
            case "Offline":
                ServiceOffline.Process(splitted[1], clientMsgProtobuf, webSocket);
                break;
            case "Cloud":
                break;
            case "PublishedFile":
                break;
            case "Credentials":
                ServiceCredential.Process(splitted[1], clientMsgProtobuf, webSocket);
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
                ServiceParential.Process(splitted[1], clientMsgProtobuf, webSocket);
                break;
            default:
                Console.WriteLine("ServiceIdentifier! " + splitted[0]);
                break;
        }
        //var request = Client.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
    }
}
