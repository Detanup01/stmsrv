namespace Steam3Kit.MSG;

public static class ClientMsgExt
{
    public static void ParseHeader(this ClientMsgProtobuf clientMsgProtobuf, PacketClientMsgProtobuf packetClientMsgProtobuf)
    {
        clientMsgProtobuf.Header.Proto.JobidTarget = packetClientMsgProtobuf.Header.Proto.JobidSource;
        clientMsgProtobuf.Header.Proto.JobidSource = packetClientMsgProtobuf.Header.Proto.JobidTarget;
    }
}
