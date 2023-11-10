using Steam3Kit;

namespace Steam3Server.SQL
{
    public class JAppInfo
    {
        public int Id { get; set; }
        public uint Magic { get; set; }
        public EUniverse Universe { get; set; }
        public List<uint> Apps { get; set; }
    }

    public class JApp
    {
        public int Id { get; set; }
        public uint AppID { get; set; }
        public uint InfoState { get; set; }
        public DateTime LastUpdated { get; set; }
        public ulong Token { get; set; }
        public byte[] Hash { get; set; }
        public byte[] BinaryDataHash { get; set; }
        public uint ChangeNumber { get; set; }
        public byte[] DataByte { get; set; }
    }
}
