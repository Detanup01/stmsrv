using UtilsLib;

namespace PICS_Backend;

public class CustomPICSVersioning
{
    static Action<ulong>? Changed;
    static ulong PICSVersion;

    public static void Init()
    {
        FileExt.CreateIfNotExists("PICS/Version.txt", PICSVersion.ToString());
        PICSVersion = ulong.Parse(File.ReadAllText("PICS/Version.txt"));
    }

    public static void IndicateChange()
    {
        PICSVersion++;
        Changed?.Invoke(PICSVersion);
    }

    public static void Quit()
    {
        File.WriteAllText("PICS/Version.txt", PICSVersion.ToString());
    }

    public static ulong Latest => PICSVersion;

    public static ulong GetNew()
    {
        IndicateChange();
        return PICSVersion; 
    }
}
