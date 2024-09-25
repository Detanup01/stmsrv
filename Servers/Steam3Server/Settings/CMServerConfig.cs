namespace Steam3Server.Settings;

public sealed class CMServerConfig
{
    public string EndPoint { get; set; } = string.Empty;
    public string CMType { get; set; } = "websocket";
    public string Destination { get; set; } = "sto1"; // Stockholm 1
    public int Load { get; set; } = 10;
    public float WTD_Load { get; set; } = 50f; // how many % is on load, smaller are better?
}
