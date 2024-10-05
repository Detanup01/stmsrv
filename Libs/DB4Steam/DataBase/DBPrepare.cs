using Steam3Kit.Types;

namespace DB4Steam;

public class DBPrepare
{
    public static void Prepare(bool RegenerateDB)
    {

        if (!Directory.Exists("Database"))
            Directory.CreateDirectory("Database");
        if (RegenerateDB)
        {
            string AppInfos_path = Path.Combine("Database", "AppInfos.db");
            if (File.Exists(AppInfos_path))
                File.Delete(AppInfos_path);
            string PackageInfos_path = Path.Combine("Database", "PackageInfos.db");
            if (File.Exists(Path.Combine("Database", "PackageInfos.db")))
                File.Delete(Path.Combine("Database", "PackageInfos.db"));
        }
        DBSteamUser.AddRegisteredUser(new() 
        { 
            SteamID = new SteamID(666, Steam3Kit.EUniverse.Public, Steam3Kit.EAccountType.Individual).ConvertToUInt64(),
            UserName = "test",
            Email = "test@test.com",
            PublicRSA = "yeet",
            Password = "password",
        });
    }
}
