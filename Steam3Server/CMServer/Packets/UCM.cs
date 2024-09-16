using Steam.Messages.ClientServer.UCM;
using Steam3Kit.MSG;
using Steam3Kit;
using ModdableWebServer;
using ModdableWebServer.Helper;

namespace Steam3Server.CMServer.Packets;

public class UCM
{
    public static void ClientUCMEnumerateUserSubscribedFiles(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        var req = CMsgClientUCMEnumerateUserSubscribedFilesWithUpdates.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/UCM.txt", req.ToString() + "\n");
        var protoRSP = new ClientMsgProtobuf<CMsgClientUCMEnumerateUserSubscribedFilesWithUpdatesResponse>(EMsg.ClientUCMEnumerateUserSubscribedFilesWithUpdatesResponse);
        protoRSP.ParseHeader(clientMsgProtobuf);
        protoRSP.Header.Proto.Eresult = (int)EResult.OK;
        CMsgClientUCMEnumerateUserSubscribedFilesWithUpdatesResponse rsp = new()
        {
            Eresult = (int)EResult.Ignored,
            TotalResults = 0
        };
        protoRSP.Body = rsp;
        webSocket.SendWebSocketByteArray(protoRSP.Serialize());
    }
}
