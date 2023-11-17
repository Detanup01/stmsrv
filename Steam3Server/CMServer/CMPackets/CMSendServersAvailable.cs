using Steam3Kit.MSG;
using Steam3Kit;
using Steam.Messages.ClientServer;
using UtilsLib;
using Steam3Server.Servers;

namespace Steam3Server.CMServer.CMPackets
{
    public class CMSendServersAvailable
    {
        public static void Response(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            var protoServerAvailable = new ClientMsgProtobuf<CMsgClientServersAvailable>(EMsg.ClientServersAvailable);
            protoServerAvailable.ParseHeader(clientMsgProtobuf);
            protoServerAvailable.Header.Proto.Eresult = (int)EResult.OK;
            List<CMsgClientServersAvailable.Types.Server_Types_Available> servers = new()
            {
                new CMsgClientServersAvailable.Types.Server_Types_Available()
                {
                    Server = 3
                },
                new CMsgClientServersAvailable.Types.Server_Types_Available()
                {
                    Server = 41
                },
                new CMsgClientServersAvailable.Types.Server_Types_Available()
                {
                    Server = 42
                },
                new CMsgClientServersAvailable.Types.Server_Types_Available()
                {
                    Server = 34
                },
                new CMsgClientServersAvailable.Types.Server_Types_Available()
                {
                    Server = 21
                },
                new CMsgClientServersAvailable.Types.Server_Types_Available()
                {
                    Server = 45
                },
                new CMsgClientServersAvailable.Types.Server_Types_Available()
                {
                    Server = 53
                },
                new CMsgClientServersAvailable.Types.Server_Types_Available()
                {
                    Server = 48
                },
                new CMsgClientServersAvailable.Types.Server_Types_Available()
                {
                    Server = 59
                },
                new CMsgClientServersAvailable.Types.Server_Types_Available()
                {
                    Server = 50
                },
                new CMsgClientServersAvailable.Types.Server_Types_Available()
                {
                    Server = 17
                },
                new CMsgClientServersAvailable.Types.Server_Types_Available()
                {
                    Server = 58
                },
                new CMsgClientServersAvailable.Types.Server_Types_Available()
                {
                    Server = 102
                },
                new CMsgClientServersAvailable.Types.Server_Types_Available()
                {
                    Server = 16
                },
                new CMsgClientServersAvailable.Types.Server_Types_Available()
                {
                    Server = 14
                },
                new CMsgClientServersAvailable.Types.Server_Types_Available()
                {
                    Server = 47
                }
            };
            protoServerAvailable.Body.ServerTypesAvailable.AddRange(servers);
            protoServerAvailable.Body.ServerTypeForAuthServices = 58;   //idk what is this
            sessionBase.SendBinaryAsync(protoServerAvailable.Serialize());
        }
    }
}
