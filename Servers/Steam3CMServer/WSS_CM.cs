using Steam3Kit;
using Steam3Kit.MSG;
using Steam3Kit.Utils;
using UtilsLib;
using ModdableWebServer.Attributes;
using ModdableWebServer;
using ModdableWebServer.Helper;
using NetCoreServer;
using Steam3CMServer.Packets;

namespace Steam3Server.CMServer
{
    public class WSS_CM
    {
        [HTTP("GET", "/cmping/")]
        public static bool CMPing(HttpRequest _, ServerStruct serverStruct)
        {
            ResponseCreator responseCreator = new(200);
            // todo: make/get CM Load
            responseCreator.SetHeader("X-Steam-CMLoad", "20");
            responseCreator.SetHeader("Content-Length", "0");
            responseCreator.SetHeader("Date", DateTime.UtcNow.ToString("R"));
            serverStruct.Response = responseCreator.GetResponse();
            serverStruct.SendResponse();
            return true;
        }

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
                    ProtoSwitch.ProtoSwitcher(clientMsgProtobuf, incoming.webSocket);
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

        

    }
}
