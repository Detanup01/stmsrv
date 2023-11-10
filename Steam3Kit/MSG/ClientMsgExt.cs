namespace Steam3Kit.MSG
{
    public static class ClientMsgExt
    {
        public static void ParseHeader(this ClientMsgProtobuf clientMsgProtobuf, PacketClientMsgProtobuf packetClientMsgProtobuf)
        {
            clientMsgProtobuf.Header.Proto = packetClientMsgProtobuf.Header.Proto;
            clientMsgProtobuf.Header.Proto.JobIdTarget = packetClientMsgProtobuf.Header.Proto.JobIdSource;
            clientMsgProtobuf.Header.Proto.JobIdSource = packetClientMsgProtobuf.Header.Proto.JobIdTarget;
        }

    }
}
