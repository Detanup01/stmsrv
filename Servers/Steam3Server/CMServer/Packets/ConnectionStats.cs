using ModdableWebServer;
using Steam3Kit.MSG;
using Steam3Kit;
using Steam.Messages.ClientServer;
using ModdableWebServer.Helper;

namespace Steam3Server.CMServer.Packets;

internal class ConnectionStats
{
    public static void Response(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        var proto = CMsgClientConnectionStats.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/ConnectionStats.txt", "REQ:" + proto.ToString() + "\n");
        var protoRSP = new ClientMsgProtobuf<CMsgClientRequestedClientStats>(EMsg.ClientRequestedClientStats);
        protoRSP.ParseHeader(clientMsgProtobuf);
        protoRSP.Header.Proto.Eresult = (int)EResult.OK;
        for (uint i = 0; i < 5; i++)
        {
            protoRSP.Body.StatsToSend.Add(new CMsgClientRequestedClientStats.Types.StatsToSend()
            { 
                ClientStat = i,
                StatAggregateMethod = 0
            });
        }
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/ConnectionStats.txt", "RSP:" + protoRSP.Body.ToString() + "\n");
        webSocket.SendWebSocketByteArray(protoRSP.Serialize());
    }
}
