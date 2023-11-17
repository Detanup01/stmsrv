using Steam3Kit.MSG;
using Steam3Kit;
using Steam.Messages.ClientServer;
using UtilsLib;
using Steam3Server.Others;
using Steam3Kit.Utils;
using Google.Protobuf;
using Steam3Server.Servers;
using System.Net;

namespace Steam3Server.CMServer.CMPackets
{
    public class CMAppTicket
    {
        public static void Response(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            var request = CMsgClientGetAppOwnershipTicket.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
            var protoRSP = new ClientMsgProtobuf<CMsgClientGetAppOwnershipTicketResponse>(EMsg.ClientGetAppOwnershipTicketResponse);
            protoRSP.ParseHeader(clientMsgProtobuf);
            protoRSP.Header.Proto.Eresult = (int)EResult.OK;
            protoRSP.Body.Eresult = (int)EResult.OK;
            protoRSP.Body.AppId = request.AppId;

            //get user dlc
            //get user licenses
            //get if token is would be GC (always yes?)

            var ticket = AppTickets.CreateTicket(new()
            { 
                AppId = request.AppId,
                DLC = new(),
                SteamID = clientMsgProtobuf.Header.Proto.ClientSteamId,
                OwnershipFlags = 0,
                GcToken = (ulong)(request.AppId == 7 ? 0 : 324234230),
                HasGCToken = request.AppId != 7,
                Licenses = new(),
                Version = 4,
                OwnershipTicketExternalIP = IPAddress.Parse("192.168.3.50"),
                OwnershipTicketInternalIP = IPAddress.Parse("192.168.1.50")
            });
            //var ticket = AppTickets.CreateTicket(clientMsgProtobuf.Header.Proto.ClientSteamId, request.AppId, Public, Private, 0, new List<uint>() { 0 }, new List<AppTickets.DlcDetails>());
            protoRSP.Body.Ticket = ByteString.CopyFrom(ticket);
            sessionBase.SendBinaryAsync(protoRSP.Serialize());
        }
    }
}