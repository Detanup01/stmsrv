using Steam.Messages.ClientServer.UCM;
using Steam3Kit.MSG;
using Steam3Kit;
using Steam3Server.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam3Server.CMServer.CMPackets
{
    public class CMUCM
    {
        public static void ClientUCMEnumerateUserSubscribedFiles(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            var req = CMsgClientUCMEnumerateUserSubscribedFilesWithUpdates.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
            var protoRSP = new ClientMsgProtobuf<CMsgClientUCMEnumerateUserSubscribedFilesWithUpdatesResponse>(EMsg.ClientUCMEnumerateUserSubscribedFilesWithUpdatesResponse);
            protoRSP.ParseHeader(clientMsgProtobuf);
            protoRSP.Header.Proto.Eresult = (int)EResult.OK;
            CMsgClientUCMEnumerateUserSubscribedFilesWithUpdatesResponse rsp = new()
            {
                Eresult = (int)EResult.Ignored,
                TotalResults = 0
            };
            protoRSP.Body = rsp;
            sessionBase.SendBinaryAsync(protoRSP.Serialize());
        }
    }
}
