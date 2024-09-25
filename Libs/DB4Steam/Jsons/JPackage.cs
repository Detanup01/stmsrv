namespace DB4Steam;

public class JPackage
{
    public int Id { get; set; }
    public uint SubID { get; set; }
    public byte[] Hash { get; set; } = [];
    public uint ChangeNumber { get; set; }
    public ulong Token { get; set; }
    public byte[] DataBytes { get; set; } = [];
}
