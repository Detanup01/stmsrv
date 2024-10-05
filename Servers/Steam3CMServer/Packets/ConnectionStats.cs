using ModdableWebServer;
using Steam3Kit.MSG;
using Steam3Kit;
using Steam.Messages.ClientServer;
using Steam3CMServer.Packets;

namespace Steam3Server.CMServer.Packets;

internal class ConnectionStats : BaseResponse<CMsgClientConnectionStats, CMsgClientRequestedClientStats>
{
    public ConnectionStats(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket) : base(clientMsgProtobuf, webSocket, EMsg.ClientRequestedClientStats)
    {

    }

    public override void MakeResponse(CMsgClientConnectionStats req, ClientMsgProtobuf<CMsgClientRequestedClientStats> rsp)
    {
        rsp.Header.Proto.Eresult = (int)EResult.OK;
        for (uint i = 0; i < 5; i++)
        {
            rsp.Body.StatsToSend.Add(new CMsgClientRequestedClientStats.Types.StatsToSend()
            {
                ClientStat = i,
                StatAggregateMethod = 0
            });
        }
    }
}
