namespace UtilsLib;

public static class FileExt
{

    public static void CreateIfNotExists(string file, string content)
    {
        DirExt.EnsureDirExists(Path.GetDirectoryName(file));
        File.WriteAllText(file, content);
    }
}
