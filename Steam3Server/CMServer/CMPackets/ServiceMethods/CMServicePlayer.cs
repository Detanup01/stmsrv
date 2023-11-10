using Steam3Kit.MSG;
using Steam3Server.Servers;

namespace Steam3Server.CMServer.CMPackets.ServiceMethods
{
    public class CMServicePlayer
    {
        public static void Process(string name, PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            name = name.Split("#")[0];
            switch (name)
            {
                default:
                    break;
            }
        }
    }
}
