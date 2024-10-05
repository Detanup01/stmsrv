using ModdableWebServer;
using Steam.Messages.ClientServer.Login;
using Steam3Kit.MSG;
using Steam3Server.CMServer.Packets.ServiceMethods;
using Steam3Server.CMServer.Packets;
using UtilsLib;
using Steam3Kit;
using ModdableWebServer.Helper;

namespace Steam3CMServer.Packets;

public class ProtoSwitch
{
    public static void ProtoSwitcher(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        //todo: send ClientServersAvailable with ClientLogOnResponse
        //todo: should save our ID to not send both data when loggin in.

        //Logger.WriteLog($"IsProto: {clientMsgProtobuf.IsProto}\nTargetJobID: {clientMsgProtobuf.TargetJobID}\nSourceJobID: {clientMsgProtobuf.SourceJobID}\nMsgType: {clientMsgProtobuf.MsgType}", "WSS_CM.ProtoSwitcher");
        Logger.PWLog(clientMsgProtobuf.MsgType.ToString());

        IPacket packet = new FakeResponse();
        switch (clientMsgProtobuf.MsgType)
        {
            //  Logins
            case EMsg.ClientLogon:
                {
                    //CMSendServersAvailable.Response(clientMsgProtobuf, sessionBase);
                    LoginRSP.Response(clientMsgProtobuf, webSocket);
                    break;
                }
            case EMsg.ClientRegisterAuthTicketWithCM:
                {
                    SendServersAvailable.Response(clientMsgProtobuf, webSocket);
                    break;
                }
            // HB
            case EMsg.ClientHeartBeat:
                {
                    var protobuf = CMsgClientHeartBeat.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
                    if (protobuf.HasSendReply && protobuf.SendReply)
                    {
                        var protoRSP = new ClientMsgProtobuf<CMsgClientHeartBeat>(EMsg.ClientHeartBeat);
                        protoRSP.ParseHeader(clientMsgProtobuf);
                        webSocket.SendWebSocketByteArray(protoRSP.Serialize());
                    }
                    break;
                }
            case EMsg.ClientGetAppOwnershipTicket:
                {
                    packet = new AppTicket(clientMsgProtobuf, webSocket);
                    break;
                }
            case EMsg.ServiceMethodCallFromClient:
                {
                    //Logger.PWLog("ServiceMethodCallFromClient");
                    ServiceIdentifier.Identify(clientMsgProtobuf, webSocket);
                    break;
                }
            case EMsg.ClientPICSProductInfoRequest:
            case EMsg.ClientPICSChangesSinceRequest:
            case EMsg.ClientPICSAccessTokenRequest:
                {
                    PICS.Response(clientMsgProtobuf, webSocket);
                    break;
                }
            // Other
            case EMsg.ClientUFSGetFileListForApp:
                {
                    Logger.PWLog(EMsg.ClientUFSGetFileListForApp + " !");
                    //UFS.ClientUFSGetFileListForApp(clientMsgProtobuf, sessionBase);
                    break;
                }
            case EMsg.ClientUCMEnumerateUserSubscribedFiles:
            case EMsg.ClientUCMEnumeratePublishedFilesByUserAction:
                {
                    UCM.ClientUCMEnumerateUserSubscribedFiles(clientMsgProtobuf, webSocket);
                    break;
                }
            case EMsg.ClientConnectionStats:
                packet = new ConnectionStats(clientMsgProtobuf, webSocket);
                break;
            case EMsg.ClientUseLocalDeviceAuthorizations:
                LocalDeviceAuth.Response(clientMsgProtobuf, webSocket);
                break;
            case EMsg.ClientRequestFriendData:
                FriendData.Response(clientMsgProtobuf, webSocket);
                break;
            case EMsg.ClientLogOff:
                break;
            default:
                Logger.PWLog("Opps: " + clientMsgProtobuf.MsgType.ToString());
                break;
        }
        packet.Start();
    }
}
