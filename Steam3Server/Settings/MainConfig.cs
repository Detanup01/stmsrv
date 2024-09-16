using Newtonsoft.Json;

namespace Steam3Server.Settings;

public class MainConfig
{
    public static MainConfig Instance()
    {
        if (File.Exists("MainConfig.config.json"))
        {
            MainConfig? settings = JsonConvert.DeserializeObject<MainConfig>(File.ReadAllText("MainConfig.config.json"));
            if (settings != null)
            {
                return settings;
            }
        }
        MainConfig instance = new();
        File.WriteAllText("MainConfig.config.json", JsonConvert.SerializeObject(instance, Formatting.Indented));
        return instance;
    }


    public AppInfoConfig AppInfoConfig { get; set; } = new();

    public PackageInfoConfig PackageInfoConfig { get; set; } = new();

    public DatabaseConfig DatabaseConfig { get; set; } = new();

}
