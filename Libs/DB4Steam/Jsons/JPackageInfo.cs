using Steam3Kit;

namespace DB4Steam;

public class JPackageInfo
{
    public int Id { get; set; }
    public EUniverse Universe { get; set; }
    public List<uint> Packages { get; set; } = [];
}
