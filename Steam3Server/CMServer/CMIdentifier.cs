using Steam3Server.Servers;
using UtilsLib;

namespace Steam3Server.CMServer
{
    public class CMIdentifier
    {
        public static void Identifier(object CMObject, string FromDestination)
        {
            //Debug.WriteDebug(FromDestination, "CMIdentifier");
            switch (FromDestination)
            {
                case "UDP":
                    {
                        Tuple<UDPServerBase, UDPServerBase.UDPReceivedEvent> receivedEvent = (Tuple<UDPServerBase, UDPServerBase.UDPReceivedEvent>)CMObject;
                        UPD_CM.ProcessUDP(receivedEvent);
                    }
                    return;
                case "WSS":
                    {
                        Tuple<byte[], WSSSessionBase> x = (Tuple<byte[], WSSSessionBase>)CMObject;
                        WSS_CM.ProcessWSS(x);
                    }
                    return;
                case "TCP":
                    {
                        Debug.PrintDebug("ERROR! TCP/SSL CM server not exist!");
                    }
                    return;
                default:
                    return;
            }
        }
    }
}
