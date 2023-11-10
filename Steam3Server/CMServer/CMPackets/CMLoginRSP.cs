using Steam3Server.Servers;
using Steam.Messages.ClientServer.Login;
using Steam3Kit;
using Steam3Kit.MSG;
using Steam3Kit.Utils;
using UtilsLib;

namespace Steam3Server.CMServer.CMPackets
{
    public class CMLoginRSP
    {
        public static void Response(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            Debug.PWDebug("Sending CMLoginRSP");
            var logon = CMsgClientLogon.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
            Debug.PWDebug(logon.ToString());
            var protoRSP = new ClientMsgProtobuf<CMsgClientLogonResponse>(EMsg.ClientLogOnResponse);
            protoRSP.ParseHeader(clientMsgProtobuf);
            protoRSP.Header.Proto.Eresult = (int)EResult.OK;
            protoRSP.Body = new()
            { 
                AccountFlags = 133,
                CellId = 51,
                ClientSuppliedSteamid = clientMsgProtobuf.Header.Proto.ClientSteamId,
                ClientInstanceId = clientMsgProtobuf.Header.Proto.ClientSteamId,
                DeprecatedUsePics = true,
                CountDisconnectsToMigrate = 0,
                CountLoginfailuresToMigrate = 0,
                Eresult = (int)EResult.OK,
                WebapiAuthenticateUserNonce = "1nonce2nonce3nonce4",
                CellIdPingThreshold = 2147483647,
                //ForceClientUpdateCheck = true,
                EresultExtended = 0,
                Rtime32ServerTime = (uint)DateUtils.DateTimeToUnixTime(DateTime.UtcNow),
                HeartbeatSeconds = 10,
                LegacyOutOfGameHeartbeatSeconds = 10,
                IpCountryCode = "US",
                VanityUrl = ""
            };
            //Debug.PWDebug(protoRSP.Body.ToString());
            sessionBase.SendBinaryAsync(protoRSP.Serialize());
        }
    }
}
