using Steam3Kit.MSG;
using Steam3Kit;
using Steam.Messages.ClientServer;
using Google.Protobuf;
using System.Net;
using ModdableWebServer;
using Steam3Kit.Types;
using Steam3CMServer.Packets;

namespace Steam3Server.CMServer.Packets;


public class AppTicket : BaseResponse<CMsgClientGetAppOwnershipTicket, CMsgClientGetAppOwnershipTicketResponse>
{
    public AppTicket(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket) : base(clientMsgProtobuf, webSocket, EMsg.ClientGetAppOwnershipTicketResponse)
    {

    }

    public override void MakeResponse(CMsgClientGetAppOwnershipTicket req, ClientMsgProtobuf<CMsgClientGetAppOwnershipTicketResponse> rsp)
    {
        rsp.Header.Proto.Eresult = (int)EResult.OK;
        rsp.Body.Eresult = (int)EResult.OK;
        rsp.Body.AppId = req.AppId;

        //get user dlc
        //get user licenses
        //get if token is would be GC (always yes?)

        var ticket = AppTickets.CreateTicket(new()
        { 
            AppId = req.AppId,
            DLC = new(),
            SteamId = ClientMsgProtobuf.Header.Proto.Steamid,
            OwnershipFlags = 0,
            GcToken = (ulong)(req.AppId == 7 ? 0 : 324234230),
            HasGCToken = req.AppId != 7,
            Licenses = new(),
            Version = 4,
            OwnershipTicketExternalIP = IPAddress.Parse("192.168.3.50"), // todo make the token
            OwnershipTicketInternalIP = IPAddress.Parse("192.168.1.50")
        });
        //var ticket = AppTickets.CreateTicket(clientMsgProtobuf.Header.Proto.ClientSteamId, request.AppId, Public, Private, 0, new List<uint>() { 0 }, new List<AppTickets.DlcDetails>());
        rsp.Body.Ticket = ByteString.CopyFrom(ticket);
    }
}