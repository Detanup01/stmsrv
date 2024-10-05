using Google.Protobuf;
using ModdableWebServer;
using ModdableWebServer.Helper;
using Steam3Kit;
using Steam3Kit.MSG;

namespace Steam3CMServer.Packets;

public interface IPacket
{
    public void Start();
}

public class FakeResponse : IPacket
{
    public void Start()
    {
        
    }
}


public abstract class BaseResponse<Request, Response>(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket, EMsg msg) : IPacket 
    where Request : IMessage<Request>, new() 
    where Response : IMessage<Response>, new()
{
    public PacketClientMsgProtobuf ClientMsgProtobuf { get; } = clientMsgProtobuf;
    public WebSocketStruct WebSocket { get; } = webSocket;
    public EMsg EMsg { get; } = msg;

    public void Start()
    {
        string ResponseName = this.GetType().Name;
        Request req = clientMsgProtobuf.GetBody<Request>();
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText(Path.Combine("Packets", $"{ResponseName}.txt"), "REQ: " + req.ToString() + "\n");
        var protoRSP = new ClientMsgProtobuf<Response>(msg);
        protoRSP.ParseHeader(clientMsgProtobuf);
        MakeResponse(req, protoRSP);
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText(Path.Combine("Packets", $"{ResponseName}.txt"), "RSP: " + protoRSP.Body.ToString() + "\n");
        webSocket.SendWebSocketByteArray(protoRSP.Serialize());
    }

    public abstract void MakeResponse(Request req, ClientMsgProtobuf<Response> rsp);
}

public abstract class BaseResponse<Request>(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket, List<(EMsg eMsg, IMessage message)> responses) : IPacket
    where Request : IMessage<Request>, new()
{
    public PacketClientMsgProtobuf ClientMsgProtobuf { get; } = clientMsgProtobuf;
    public WebSocketStruct WebSocket { get; } = webSocket;
    public List<(EMsg eMsg, IMessage message)> Responses { get; } = responses;
    public void Start()
    {
        string ResponseName = this.GetType().Name;
        Request req = clientMsgProtobuf.GetBody<Request>();
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText(Path.Combine("Packets", $"{ResponseName}.txt"), "REQ: " + req.ToString() + "\n");
        Dictionary<EMsg, TestClientMsg> ToResponse = [];
        foreach (var item in responses)
        {
            var test = new TestClientMsg(item.message, item.eMsg);
            test.ParseHeader(clientMsgProtobuf);
            ToResponse.Add(item.eMsg, test);
        }
        MakeResponse(req, ToResponse);

    }

    public void SendClientRsp(TestClientMsg testClientMsg)
    {
        string ResponseName = this.GetType().Name;
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText(Path.Combine("Packets", $"{ResponseName}.txt"), "RSP: " + testClientMsg.Body.ToString() + "\n");
        webSocket.SendWebSocketByteArray(testClientMsg.Serialize());
    }

    public abstract void MakeResponse(Request req, Dictionary<EMsg, TestClientMsg> rsp);
}

