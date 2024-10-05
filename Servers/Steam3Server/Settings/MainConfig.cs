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

    public DatabaseConfig DatabaseConfig { get; set; } = new();

    public List<CMServerConfig> CMServerConfigs { get; set; } = new();

}
