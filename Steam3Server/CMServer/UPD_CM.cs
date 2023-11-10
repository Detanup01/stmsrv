using Steam.NetworkingSockets.UDP;
using Steam3Kit;
using Steam3Server.Servers;
using Steam3Kit.Types;
using UtilsLib;

namespace Steam3Server.CMServer
{
    public class UPD_CM
    {
        public static void ProcessUDP(Tuple<UDPServerBase, UDPServerBase.UDPReceivedEvent> incoming)
        {
            var bytes = incoming.Item2.buffer[..(int)incoming.Item2.size];
            var packet = new UdpPacket(new MemoryStream(bytes));
            MemoryStream ms = new MemoryStream();
            if (!packet.IsValid)
            {
                Debug.WriteDebug("Packet not valid");
                return;
            }

            Debug.WriteDebug("PacketType: " + packet.Header.PacketType);

            Debug.WriteDebug(BitConverter.ToString(packet.Payload.ToArray()));

            var chlr =  CMsgSteamSockets_UDP_ChallengeRequest.Parser.ParseFrom(packet.Payload);

            Debug.PWDebug(chlr);

            incoming.Item1.Send(ms.ToArray());
        }
    }
}
