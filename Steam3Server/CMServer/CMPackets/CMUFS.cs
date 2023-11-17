using Steam.Messages.ClientServer.UCM;
using Steam.Messages.ClientServer.UFS;
using Steam3Kit;
using Steam3Kit.MSG;
using Steam3Server.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam3Server.CMServer.CMPackets
{
    public class CMUFS
    {

        public static void ClientUFSGetFileListForApp(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            var req = CMsgClientUFSGetFileListForApp.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
            var protoRSP = new ClientMsgProtobuf<CMsgClientUFSGetFileListForAppResponse>(EMsg.ClientUFSGetFileListForAppResponse);
            protoRSP.ParseHeader(clientMsgProtobuf);
            protoRSP.Header.Proto.Eresult = (int)EResult.OK;
            CMsgClientUFSGetFileListForAppResponse rsp = new()
            { 
                Files =
                { 
                },
                PathPrefixes = { }
            };
            protoRSP.Body = rsp;
            sessionBase.SendBinaryAsync(protoRSP.Serialize());
        }

       
    }
}
