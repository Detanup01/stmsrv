using Google.Protobuf;
using ModdableWebServer;
using ModdableWebServer.Helper;
using Steam.Messages.ClientServer.Friends;
using Steam3Kit;
using Steam3Kit.MSG;

namespace Steam3Server.CMServer.Packets;

public class FriendData
{
    public static void Response(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        var proto = CMsgClientRequestFriendData.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/FriendData.txt", "REQ:" + proto.ToString() + "\n");
        var protoRSP = new ClientMsgProtobuf<CMsgClientPersonaState>(EMsg.ClientPersonaState);
        protoRSP.ParseHeader(clientMsgProtobuf);
        protoRSP.Header.Proto.Eresult = (int)EResult.OK;
        protoRSP.Body.StatusFlags = proto.PersonaStateRequested;
        List<CMsgClientPersonaState.Types.Friend> Friends = new();
        foreach (var item in proto.Friends)
        {
            Friends.Add(new()
            {
                Friendid = item,
                AvatarHash = ByteString.CopyFromUtf8("0000000000000000000000000000000000000000"),
                LastLogoff = 0,
                LastLogon = 0,
                LastSeenOnline = 0
            });
        }
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/FriendData.txt", "RSP:" + protoRSP.Body.ToString() + "\n");
        webSocket.SendWebSocketByteArray(protoRSP.Serialize());
    }
}
