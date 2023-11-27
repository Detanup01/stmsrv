namespace Steam3Kit.Networking
{
    public interface INetFilterEncryption
    {
        byte[] ProcessIncoming(byte[] data);
        byte[] ProcessOutgoing(byte[] data);
    }
}
