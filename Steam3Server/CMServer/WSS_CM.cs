using Steam.Messages.ClientServer.Login;
using Steam.Messages.ClientServer2;
using Steam.Messages.RemoteClient.Service.Message;
using Steam3Kit;
using Steam3Kit.MSG;
using Steam3Kit.Utils;
using Steam3Server.CMServer.CMPackets;
using Steam3Server.CMServer.CMPackets.ServiceMethods;
using UtilsLib;
using Steam3Server.Servers;

namespace Steam3Server.CMServer
{
    public class WSS_CM
    {
        public static void ProcessWSS(Tuple<byte[], WSSSessionBase> incoming)
        {
            var rsp = new byte[] { };
            uint rawEMsg = BitConverter.ToUInt32(incoming.Item1, 0);
            EMsg eMsg = MsgUtil.GetMsg(rawEMsg);
            switch (eMsg)
            {
                // certain message types are always MsgHdr
                case EMsg.ChannelEncryptRequest:
                case EMsg.ChannelEncryptResponse:
                case EMsg.ChannelEncryptResult:
                    {
                        var enc = new PacketMsg(eMsg, incoming.Item1);
                        Debug.WriteDebug($"ChannelEncrypt! IsProto: {enc.IsProto}\nTargetJobID: {enc.TargetJobID}\nSourceJobID: {enc.SourceJobID}\nMsgType: {enc.MsgType}", "WSS_CM");
                    }
                    return;
            }

            try
            {
                if (MsgUtil.IsProtoBuf(rawEMsg))
                {
                    // if the emsg is flagged, we're a proto message
                    var clientMsgProtobuf = new PacketClientMsgProtobuf(eMsg, incoming.Item1);
                    ProtoSwitcher(clientMsgProtobuf, incoming.Item2);
                }
                else
                {
                    // otherwise we're a struct message
                    var enc = new PacketClientMsg(eMsg, incoming.Item1);
                    Debug.WriteDebug($"PacketClientMsg!!\nIsProto: {enc.IsProto}\nTargetJobID: {enc.TargetJobID}\nSourceJobID: {enc.SourceJobID}\nMsgType: {enc.MsgType}", "WSS_CM.NOT.IsProtoBuf");
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteDebug($"Exception deserializing emsg {eMsg} ({MsgUtil.IsProtoBuf(rawEMsg)}).\n{ex.ToString()}");
            }
        }

        public static void ProtoSwitcher(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            //todo: send ClientServersAvailable with ClientLogOnResponse
            //todo: should save our ID to not send both data when loggin in.

            //Debug.WriteDebug($"IsProto: {clientMsgProtobuf.IsProto}\nTargetJobID: {clientMsgProtobuf.TargetJobID}\nSourceJobID: {clientMsgProtobuf.SourceJobID}\nMsgType: {clientMsgProtobuf.MsgType}", "WSS_CM.ProtoSwitcher");
            Debug.PWDebug(clientMsgProtobuf.MsgType.ToString());
            switch (clientMsgProtobuf.MsgType)
            {
                //  Logins
                case EMsg.ClientLogon:
                    {
                        //CMSendServersAvailable.Response(clientMsgProtobuf, sessionBase);
                        CMLoginRSP.Response(clientMsgProtobuf, sessionBase);
                        break;
                    }
                case EMsg.ClientRegisterAuthTicketWithCM:
                    {
                        /*
                        var protobuf = CMsgClientRegisterAuthTicketWithCM.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
                        Debug.PWDebug(protobuf.ToString());*/
                        CMSendServersAvailable.Response(clientMsgProtobuf, sessionBase);
                        //CMLoginRSP.Response(clientMsgProtobuf, sessionBase);
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
                            sessionBase.Send(protoRSP.Serialize());
                        }
                        break;
                    }
                case EMsg.ClientGetAppOwnershipTicket:
                    {
                        CMAppTicket.Response(clientMsgProtobuf, sessionBase);
                        break;
                    }
                case EMsg.ServiceMethodCallFromClient:
                    {
                        //Debug.PWDebug("ServiceMethodCallFromClient");
                        CMServiceIdentifier.Identify(clientMsgProtobuf, sessionBase);
                        break;
                    }
                case EMsg.ClientPICSProductInfoRequest:
                case EMsg.ClientPICSChangesSinceRequest:
                case EMsg.ClientPICSAccessTokenRequest:
                    {
                        CMPICS.Response(clientMsgProtobuf, sessionBase);
                        break;
                    }
                // Other
                case EMsg.ClientUFSGetFileListForApp:
                    {
                        CMUFS.ClientUFSGetFileListForApp(clientMsgProtobuf, sessionBase);
                        break;
                    }
                case EMsg.ClientUCMEnumerateUserSubscribedFiles:
                case EMsg.ClientUCMEnumeratePublishedFilesByUserAction:
                    {
                        CMUCM.ClientUCMEnumerateUserSubscribedFiles(clientMsgProtobuf, sessionBase);
                        break;
                    }
                case EMsg.ClientCurrentUIMode:
                case EMsg.ClientConnectionStats:
                    // Here we could track each thing of these, but we dont want currently.
                    break;
                case EMsg.ClientUseLocalDeviceAuthorizations:
                    CMLocalDeviceAuth.Response(clientMsgProtobuf, sessionBase);
                    break;
                case EMsg.ClientRequestFriendData:
                    CMFriendData.Response(clientMsgProtobuf, sessionBase);
                    break;
                default:
                    Debug.PWDebug("Opps: " + clientMsgProtobuf.MsgType.ToString());
                    break;
            }
        }

    }
}
