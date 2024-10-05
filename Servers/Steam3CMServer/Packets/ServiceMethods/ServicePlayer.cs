using ModdableWebServer;
using ModdableWebServer.Helper;
using Steam.Messages.Player;
using Steam3Kit;
using Steam3Kit.MSG;
using Steam3Kit.Utils;

namespace Steam3Server.CMServer.Packets.ServiceMethods;

public class ServicePlayer
{
    public static void Process(string name, PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        name = name.Split("#")[0];
        switch (name)
        {
            case "GetCommunityPreferences":
                GetCommunityPreferences(clientMsgProtobuf, webSocket);
                break;
            default:
                Console.WriteLine("ServicePlayer! " + name);
                break;
        }
    }

    public static void GetCommunityPreferences(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        var player_GetCommunityPreferences_Request = CPlayer_GetCommunityPreferences_Request.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
        if (!Directory.Exists("ServiceMethods"))
            Directory.CreateDirectory("ServiceMethods");
        File.AppendAllText("ServiceMethods/GetCommunityPreferences.txt", player_GetCommunityPreferences_Request.ToString() + "\n");
        var protoRSP = new ClientMsgProtobuf<CPlayer_GetCommunityPreferences_Response>(EMsg.ServiceMethodResponse);
        protoRSP.ParseHeader(clientMsgProtobuf);
        protoRSP.Header.Proto.Eresult = (int)EResult.OK;
        protoRSP.Body.Preferences = new()
        { 
            ParenthesizeNicknames = false,
            TextFilterIgnoreFriends = true,
            TextFilterSetting = ETextFilterSetting.KEtextFilterSettingDisabled,
            TextFilterWordsRevision = 0,
            TimestampUpdated = (uint)DateUtils.DateTimeToUnixTime(DateTime.UtcNow.AddDays(-10))
        };
        webSocket.SendWebSocketByteArray(protoRSP.Serialize());
    }
}
