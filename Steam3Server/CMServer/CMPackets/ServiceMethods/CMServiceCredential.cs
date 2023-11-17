
using Steam3Kit.MSG;
using Steam3Kit;
using Steam3Server.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steam.Messages.Credentials;
using Google.Protobuf;

namespace Steam3Server.CMServer.CMPackets.ServiceMethods
{
    internal class CMServiceCredential
    {
        public static void Process(string name, PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            name = name.Split("#")[0];
            switch (name)
            {
                case "GetAccountAuthSecret":
                    GetAccountAuthSecret(clientMsgProtobuf, sessionBase);
                    break;
                default:
                    break;
            }
        }

        public static void GetAccountAuthSecret(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            var protoRSP = new ClientMsgProtobuf<CCredentials_GetAccountAuthSecret_Response>(EMsg.ServiceMethodResponse);
            protoRSP.ParseHeader(clientMsgProtobuf);
            protoRSP.Header.Proto.Eresult = (int)EResult.OK;
            protoRSP.Body = new()
            {
                Secret = ByteString.CopyFrom(Convert.FromHexString("")),
                SecretId = 666
            };
            sessionBase.SendBinaryAsync(protoRSP.Serialize());
        }
    }
}
