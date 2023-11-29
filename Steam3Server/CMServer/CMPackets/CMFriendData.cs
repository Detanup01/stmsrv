
using Google.Protobuf;
using Steam.Messages.ClientServer.Friends;
using Steam3Kit;
using Steam3Kit.MSG;
using Steam3Server.Servers;

namespace Steam3Server.CMServer.CMPackets
{
    public class CMFriendData
    {
        public static void Response(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            var proto = CMsgClientRequestFriendData.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
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
            sessionBase.SendBinaryAsync(protoRSP.Serialize());
        }
    }
}
