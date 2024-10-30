using Steam3Server;
using Steam3CMServer;

namespace ServerConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServerCore.Start(); 
            var serverConfigs = Steam3Server.Settings.MainConfig.Instance().CMServerConfigs;
            foreach (var config in serverConfigs)
            {
                var endpoint = config.Host.Split(":");
                Console.WriteLine(config.Host);
                CMServerMain.Start(endpoint[0], int.Parse(endpoint[1])); // todo do this better
            }
            
            string ret = string.Empty;
            while (ret != "quit")
            {
                ret = Console.ReadLine()!;
            }
            CMServerMain.Stop();
            ServerCore.Stop();
        }
    }
}
