using Google.Protobuf;
using Steam.Messages.ClientServer.UCM;
using Steam.Messages.Parental;
using Steam3Kit;
using Steam3Kit.MSG;
using Steam3Server.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam3Server.CMServer.CMPackets.ServiceMethods
{
    public class CMServiceParential
    {
        public static void Process(string name, PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            name = name.Split("#")[0];
            switch (name)
            {
                case "GetSignedParentalSettings":
                    GetSignedParentalSettings(clientMsgProtobuf, sessionBase);
                    break;
                default:
                    break;
            }
        }

        public static void GetSignedParentalSettings(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            var protoRSP = new ClientMsgProtobuf<CParental_GetSignedParentalSettings_Response>(EMsg.ServiceMethodResponse);
            protoRSP.ParseHeader(clientMsgProtobuf);
            protoRSP.Header.Proto.Eresult = (int)EResult.Fail;
            protoRSP.Body = new()
            { 
            };
            sessionBase.SendBinaryAsync(protoRSP.Serialize());
        }
    }
}
