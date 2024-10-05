using UtilsLib;

namespace PICS_Backend;

public class CustomPICSVersioning
{
    public static Action<uint>? Changed;
    static uint PICSVersion;
    static DateTime LastUpdate;

    public static void Init()
    {
        FileExt.CreateIfNotExists("PICS/Version.txt", PICSVersion.ToString());
        PICSVersion = uint.Parse(File.ReadAllText("PICS/Version.txt"));
        LastUpdate = DateTime.Now;
    }

    public static void IndicateChange()
    {
        PICSVersion++;
        LastUpdate = DateTime.Now;
        Changed?.Invoke(PICSVersion);
    }

    public static void Quit()
    {
        File.WriteAllText("PICS/Version.txt", PICSVersion.ToString());
    }

    public static uint Latest => PICSVersion;

    public static uint GetNew()
    {
        IndicateChange();
        return PICSVersion; 
    }

    public static (uint changeid, DateTime time) GetLast() => (PICSVersion, LastUpdate);
}
