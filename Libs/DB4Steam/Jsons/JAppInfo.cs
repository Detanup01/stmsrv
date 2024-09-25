using Steam3Kit;

namespace DB4Steam;

public class JAppInfo
{
    public int Id { get; set; }
    public uint Magic { get; set; }
    public EUniverse Universe { get; set; }
    public List<uint> Apps { get; set; } = [];
}
