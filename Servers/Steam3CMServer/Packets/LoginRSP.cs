using Steam.Messages.ClientServer.Login;
using Steam3Kit;
using Steam3Kit.MSG;
using Steam3Kit.Utils;
using ModdableWebServer;
using ModdableWebServer.Helper;
using Steam3Kit.Types;

namespace Steam3Server.CMServer.Packets;

public class LoginRSP
{
    public static void Response(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        var logon = CMsgClientLogon.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/LoginRSP.txt", "REQ:" + logon.ToString() + "\n");

        var anonunk = new SteamID(0, EUniverse.Public, EAccountType.AnonUser);
        Console.WriteLine(anonunk);
        Console.WriteLine(anonunk.ToString());

        var steamid = new SteamID(clientMsgProtobuf.Header.Proto.Steamid);
        Console.WriteLine(steamid);
        Console.WriteLine(steamid.ToString());
        if (steamid.ToString() == anonunk.ToString())
        {
            // create new steamid
            steamid = new SteamID((uint)Random.Shared.Next(), EUniverse.Public, EAccountType.AnonUser);
        }

        var protoRSP = new ClientMsgProtobuf<CMsgClientLogonResponse>(EMsg.ClientLogOnResponse);
        protoRSP.ParseHeader(clientMsgProtobuf);
        protoRSP.Header.Proto.Eresult = (int)EResult.OK;
        protoRSP.Header.Proto.Steamid = steamid.ConvertToUInt64();
        protoRSP.Body = new()
        { 
            AccountFlags = 0,//133,
            CellId = 51,
            ClientSuppliedSteamid = steamid.ConvertToUInt64(),
            ClientInstanceId = steamid.AccountInstance,
            CountDisconnectsToMigrate = 0,
            CountLoginfailuresToMigrate = 0,
            Eresult = (int)EResult.OK,
            CellIdPingThreshold = 2147483647,
            Rtime32ServerTime = (uint)DateUtils.DateTimeToUnixTime(DateTime.UtcNow),
            HeartbeatSeconds = 10,
            LegacyOutOfGameHeartbeatSeconds = 10,
            IpCountryCode = "US",
            VanityUrl = ""
        };
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/LoginRSP.txt", "RSP:" + protoRSP.Body.ToString() + "\n");
        webSocket.SendWebSocketByteArray(protoRSP.Serialize());
    }
}
