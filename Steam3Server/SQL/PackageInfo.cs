using Steam3Kit;

namespace Steam3Server.SQL
{
    public class JPackageInfo
    {
        public int Id { get; set; }
        public uint Magic { get; set; }
        public EUniverse Universe { get; set; }
        public List<uint> Packages { get; set; }
    }
    public class JPackage
    {
        public int Id { get; set; }
        public uint SubID { get; set; }
        public byte[] Hash { get; set; }
        public uint ChangeNumber { get; set; }
        public ulong Token { get; set; }
        public byte[] DataBytes { get; set; }
    }
}
