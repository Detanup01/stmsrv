using Steam.Messages.ClientServer;
using Steam.Messages.Player;
using Steam3Kit;
using Steam3Kit.MSG;
using Steam3Kit.Utils;
using Steam3Server.Servers;
using System.Security.Cryptography;

namespace Steam3Server.CMServer.CMPackets.ServiceMethods
{
    public class CMServicePlayer
    {
        public static void Process(string name, PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            name = name.Split("#")[0];
            switch (name)
            {
                case "GetCommunityPreferences":
                    GetCommunityPreferences(clientMsgProtobuf,sessionBase);
                    break;
                default:
                    break;
            }
        }

        public static void GetCommunityPreferences(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
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
            sessionBase.SendBinaryAsync(protoRSP.Serialize());
        }
    }
}
