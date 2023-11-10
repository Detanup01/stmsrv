using Steam3Kit;
using Steam3Server.Servers;
using Steam3Server.HTTPServer;
using UtilsLib;

namespace Steam3Server.CMServer
{
    public class CMOverrides
    {
        public static void SteamWebWSS_EventDisconnected(object? sender, WSSSessionBase e)
        {
            e.RequestReceived -= RequestRoute.HTTPS_RequestReceived;
            e.WsReceived -= WSS_Received;
        }
        public static void SteamWebWSS_EventConnected(object? sender, WSSSessionBase e)
        {
            e.WsReceived += WSS_Received;
            e.RequestReceived += RequestRoute.HTTPS_RequestReceived;
        }

        private static void WSS_Received(object? sender, (byte[] bytes, WSSSessionBase session) e)
        {
            Tuple<byte[], WSSSessionBase> tuple = new(e.bytes,e.session);
            CMIdentifier.Identifier(tuple, "WSS");
            /*
            Debug.WriteDebug(BitConverter.ToString(bytes));
            e.session.Send(bytes);
            */
        }

        public static void CMServerUDP_ServerRecieved(object? sender, UDPServerBase.UDPReceivedEvent e)
        {
            Tuple<UDPServerBase, UDPServerBase.UDPReceivedEvent> x = new((UDPServerBase)sender,e);
            CMIdentifier.Identifier(x, "UDP");
            /*
            var pendingToSend = CMIdentifier.Identifier(e, "UDP");
            var UDP = (UDPServerBase)sender;
            UDP.Send(pendingToSend);
            */
        }
    }
}
