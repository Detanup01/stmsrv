using Steam3Kit.MSG;
using Steam3Kit;
using Steam.Messages.Credentials;
using Google.Protobuf;
using ModdableWebServer;
using ModdableWebServer.Helper;

namespace Steam3Server.CMServer.Packets.ServiceMethods;

internal class ServiceCredential
{
    public static void Process(string name, PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        name = name.Split("#")[0];
        switch (name)
        {
            case "GetAccountAuthSecret":
                GetAccountAuthSecret(clientMsgProtobuf, webSocket);
                break;
            default:
                Console.WriteLine("ServiceCredential! " + name);
                break;
        }
    }

    public static void GetAccountAuthSecret(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        var credentials_GetAccountAuthSecret_Request = CCredentials_GetAccountAuthSecret_Request.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
        if (!Directory.Exists("ServiceMethods"))
            Directory.CreateDirectory("ServiceMethods");
        File.AppendAllText("ServiceMethods/GetAccountAuthSecret.txt", credentials_GetAccountAuthSecret_Request.ToString() + "\n");
        var protoRSP = new ClientMsgProtobuf<CCredentials_GetAccountAuthSecret_Response>(EMsg.ServiceMethodResponse);
        protoRSP.ParseHeader(clientMsgProtobuf);
        protoRSP.Header.Proto.Eresult = (int)EResult.OK;
        protoRSP.Body = new()
        {
            Secret = ByteString.CopyFrom(Convert.FromHexString("")),
            SecretId = 666
        };
        webSocket.SendWebSocketByteArray(protoRSP.Serialize());
    }
}
