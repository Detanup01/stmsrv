using Steam3Kit.MSG;
using Steam3Kit;
using Steam.Messages.ClientServer;
using UtilsLib;
using Steam3Server.Others;
using Steam3Kit.Utils;
using Google.Protobuf;
using Steam3Server.Servers;

namespace Steam3Server.CMServer.CMPackets
{
    public class CMAppTicket
    {
        public static void Response(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            var rsp = new byte[] { };
            var request = CMsgClientGetAppOwnershipTicket.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
            Debug.PWDebug(request.ToString());
            var protoRSP = new ClientMsgProtobuf<CMsgClientGetAppOwnershipTicketResponse>(EMsg.ClientGetAppOwnershipTicketResponse);
            protoRSP.ParseHeader(clientMsgProtobuf);
            protoRSP.Header.Proto.Eresult = (int)EResult.OK;
            protoRSP.Body.Eresult = (int)EResult.OK;
            protoRSP.Body.AppId = request.AppId;
            uint Private = 0;
            uint Public = 0;
            if (NetHelpers.TryParseIPEndPoint("192.168.1.50", out var endPoint))
            {
                Public = NetHelpers.GetIPAddressAsUInt(endPoint.Address);
            }
            if (NetHelpers.TryParseIPEndPoint("192.168.3.50", out endPoint))    //  VPN IP
            {
                Private = NetHelpers.GetIPAddressAsUInt(endPoint.Address);
            }

            //var ticket = AppTickets.CreateTicket(clientMsgProtobuf.Header.Proto.ClientSteamId, request.AppId, Public, Private, 0, new List<uint>() { 0 }, new List<AppTickets.DlcDetails>());
           // protoRSP.Body.Ticket = ByteString.CopyFrom(ticket);
            Debug.PWDebug(protoRSP.Body.ToString());
            rsp = protoRSP.Serialize();
            sessionBase.SendBinaryAsync(rsp);
        }
    }
}