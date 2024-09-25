using Newtonsoft.Json;

namespace MainLib
{
    public class Config
    {
        public static Dictionary<string, string> GetConfig()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("config.json"));
        
        }
    }
}