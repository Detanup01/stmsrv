namespace Steam3Server.Settings;

public sealed class CMServerConfig
{
    // use this to host with other apps
    public string Host { get; set; } = string.Empty;

    // use this to let others see it. (cmp1-sto1.otherdns:port) ? [cmp1-sto1.steamserver.net:7777]
    public string EndPoint { get; set; } = string.Empty;
    public string CMType { get; set; } = "websockets";
    public string Destination { get; set; } = "sto1"; // Stockholm 1
    public int Load { get; set; } = 10;
    public float WTD_Load { get; set; } = 50f; // how many % is on load, smaller are better?
}
