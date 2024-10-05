namespace DB4Steam;

public class JApp
{
    public int Id { get; set; }
    public uint AppID { get; set; }
    public DateTime LastUpdated { get; set; }
    public ulong Token { get; set; }
    public byte[] Hash { get; set; } = [];
    public byte[] BinaryDataHash { get; set; } = [];
    public uint ChangeNumber { get; set; }
    public byte[] DataByte { get; set; } = [];
}
