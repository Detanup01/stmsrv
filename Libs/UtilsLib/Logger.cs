using System.Runtime.CompilerServices;
using LLibrary;

namespace UtilsLib;

public class Logger
{
    public static L logger = new(true, directory: "UtilsLog");

    public static bool IsDebug = true;

    public static void PWLog(object obj, string label = "INFO", [CallerMemberName] string memberName = "Shared")
    {
        if (obj == null)
            return;
        string? obj_str = obj.ToString();
        if (obj_str == null)
            return;
        PrintLog(obj_str);
        WriteLog(obj_str, label, memberName);
    }

    public static void PrintLog(object obj)
    {
        if (obj == null)
            return;
        string? obj_str = obj.ToString();
        if (obj_str == null)
            return;
        if (IsDebug == true)
            Console.WriteLine(obj_str);
    }

    public static void PrintLog(string obj)
    {
        if (IsDebug == true)
            Console.WriteLine(obj);
    }

    public static void WriteLog(string strLog, string label = "debug", [CallerMemberName] string memberName = "Shared")
    {
        logger.Log(label, strLog + " | " + memberName);
    }
}