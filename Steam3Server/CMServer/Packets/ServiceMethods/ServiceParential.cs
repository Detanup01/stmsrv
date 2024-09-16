using ModdableWebServer;
using ModdableWebServer.Helper;
using Steam.Messages.Parental;
using Steam3Kit;
using Steam3Kit.MSG;

namespace Steam3Server.CMServer.Packets.ServiceMethods;

public class ServiceParential
{
    public static void Process(string name, PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        name = name.Split("#")[0];
        switch (name)
        {
            case "GetSignedParentalSettings":
                GetSignedParentalSettings(clientMsgProtobuf, webSocket);
                break;
            default:
                Console.WriteLine("ServiceParential! " + name);
                break;
        }
    }

    public static void GetSignedParentalSettings(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        var parental_GetSignedParentalSettings_Request = CParental_GetSignedParentalSettings_Request.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
        if (!Directory.Exists("ServiceMethods"))
            Directory.CreateDirectory("ServiceMethods");
        File.AppendAllText("ServiceMethods/GetSignedParentalSettings.txt", parental_GetSignedParentalSettings_Request.ToString() + "\n");
        var protoRSP = new ClientMsgProtobuf<CParental_GetSignedParentalSettings_Response>(EMsg.ServiceMethodResponse);
        protoRSP.ParseHeader(clientMsgProtobuf);
        protoRSP.Header.Proto.Eresult = (int)EResult.Fail;
        protoRSP.Body = new()
        { 
        };
        webSocket.SendWebSocketByteArray(protoRSP.Serialize());
    }
}
