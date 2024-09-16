using Steam3Server.Settings;

namespace Steam3Server.SQL
{
    public class DBPrepare
    {
        public static void Prepare()
        {

            if (!Directory.Exists("Database"))
                Directory.CreateDirectory("Database");
            if (MainConfig.Instance().DatabaseConfig.AlwaysRegenerateInfoDBs)
            {
                if (File.Exists("Database/AppInfos.db"))
                    File.Delete("Database/AppInfos.db");
                if (File.Exists("Database/PackageInfos.db"))
                    File.Delete("Database/PackageInfos.db");
            }

        }
    }
}
