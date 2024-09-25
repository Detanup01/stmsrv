namespace DB4Steam;

public class DBPrepare
{
    public static void Prepare(bool RegenerateDB)
    {

        if (!Directory.Exists("Database"))
            Directory.CreateDirectory("Database");
        if (RegenerateDB)
        {
            if (File.Exists("Database/AppInfos.db"))
                File.Delete("Database/AppInfos.db");
            if (File.Exists("Database/PackageInfos.db"))
                File.Delete("Database/PackageInfos.db");
        }
        DBSteamUser.AddRegisteredUser(new() 
        { 
            SteamID = 303030,
            UserName = "test",
            Email = "test@test.com",
            PublicRSA = "yeet",
            Password = "password",
        });
    }
}
