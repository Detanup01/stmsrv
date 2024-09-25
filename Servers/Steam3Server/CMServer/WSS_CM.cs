using Steam.Messages.ClientServer.Login;
using Steam3Kit;
using Steam3Kit.MSG;
using Steam3Kit.Utils;
using Steam3Server.CMServer.Packets;
using Steam3Server.CMServer.Packets.ServiceMethods;
using UtilsLib;
using ModdableWebServer.Attributes;
using ModdableWebServer;
using ModdableWebServer.Helper;

namespace Steam3Server.CMServer
{
    public class WSS_CM
    {
        [WS("/cmsocket/")]
        public static void cmsocket(WebSocketStruct socketStruct)
        {
            if (socketStruct.WSRequest != null)
            {
                var bytes = socketStruct.WSRequest.Value.buffer.Skip((int)socketStruct.WSRequest.Value.offset).Take((int)socketStruct.WSRequest.Value.size).ToArray();
                ProcessWSS((bytes, socketStruct));
            }
        }

        public static void ProcessWSS((byte[] data, WebSocketStruct webSocket) incoming)
        {
            byte[] rsp = [];
            uint rawEMsg = BitConverter.ToUInt32(incoming.data, 0);
            Logger.PWLog("rawEmsg: " + rawEMsg);
            EMsg eMsg = MsgUtil.GetMsg(rawEMsg);
            switch (eMsg)
            {
                // certain message types are always MsgHdr
                case EMsg.ChannelEncryptRequest:
                case EMsg.ChannelEncryptResponse:
                case EMsg.ChannelEncryptResult:
                    {
                        var enc = new PacketMsg(eMsg, incoming.data);
                        Logger.WriteLog($"ChannelEncrypt! IsProto: {enc.IsProto}\nTargetJobID: {enc.TargetJobID}\nSourceJobID: {enc.SourceJobID}\nMsgType: {enc.MsgType}", "WSS_CM");
                    }
                    return;
            }

            try
            {
                if (MsgUtil.IsProtoBuf(rawEMsg))
                {
                    // if the emsg is flagged, we're a proto message
                    var clientMsgProtobuf = new PacketClientMsgProtobuf(eMsg, incoming.data);
                    ProtoSwitcher(clientMsgProtobuf, incoming.webSocket);
                }
                else
                {
                    // otherwise we're a struct message
                    var enc = new PacketClientMsg(eMsg, incoming.data);
                    Logger.WriteLog($"PacketClientMsg!!\nIsProto: {enc.IsProto}\nTargetJobID: {enc.TargetJobID}\nSourceJobID: {enc.SourceJobID}\nMsgType: {enc.MsgType}", "WSS_CM.NOT.IsProtoBuf");
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog($"Exception deserializing emsg {eMsg} ({MsgUtil.IsProtoBuf(rawEMsg)}).\n{ex.ToString()}");
            }
        }

        public static void ProtoSwitcher(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
        {
            //todo: send ClientServersAvailable with ClientLogOnResponse
            //todo: should save our ID to not send both data when loggin in.

            //Logger.WriteLog($"IsProto: {clientMsgProtobuf.IsProto}\nTargetJobID: {clientMsgProtobuf.TargetJobID}\nSourceJobID: {clientMsgProtobuf.SourceJobID}\nMsgType: {clientMsgProtobuf.MsgType}", "WSS_CM.ProtoSwitcher");
            Logger.PWLog(clientMsgProtobuf.MsgType.ToString());
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
                        AppTicket.Response(clientMsgProtobuf, webSocket);
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
                    ConnectionStats.Response(clientMsgProtobuf, webSocket);
                    // Here we could track each thing of these, but we dont want currently.
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
        }

    }
}
