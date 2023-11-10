using System.Runtime.CompilerServices;
using LLibrary;

namespace UtilsLib
{
    public class Debug
    {
        public static L logger = new(true, directory: "UtilsLog");

        public static bool IsDebug = true;

        public static void PWDebug(object obj, string label = "INFO", [CallerMemberName] string memberName = "Shared")
        {
            if (IsDebug == true)
            {
                Console.WriteLine($"[{label}] {obj}");
            }
            logger.Log(label, obj.ToString() + " | " + memberName);
        }

        public static void PrintDebug(object obj)
        {
            if (IsDebug == true)
            {
                Console.WriteLine(obj.ToString());
            }
        }

        public static void WriteDebug(string strLog, string label = "debug", [CallerMemberName] string memberName = "Shared")
        {
            logger.Log(label, strLog + " | " + memberName);
        }

    }

}