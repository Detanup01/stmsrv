namespace UtilsLib;

public static class DirExt
{
    public static void EnsureDirExists(string? DirPath)
    {
        if (string.IsNullOrEmpty(DirPath))
            return;
        if (!Directory.Exists(DirPath))
            Directory.CreateDirectory(DirPath);
    }

    public static void RecreateDir(string? DirPath)
    {
        if (string.IsNullOrEmpty(DirPath))
            return;
        if (Directory.Exists(DirPath))
            Directory.Delete(DirPath, true);
        Directory.CreateDirectory(DirPath);
    }
}
