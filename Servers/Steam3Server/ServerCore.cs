using System.Security.Authentication;
using NetCoreServer;
using Steam3Server.Servers;
using Steam3Server.Others;
using UtilsLib;
using Steam3Server.Settings;
using ModdableWebServer.Servers;
using ModdableWebServer.Helper;
using System.Reflection;
using DB4Steam;
using PICS_Backend;

namespace Steam3Server;

public class ServerCore
{
    public static void Start()
    {
        DebugPrinter.EnableLogs = true;
        MainConfig.Instance();
        DBPrepare.Prepare(MainConfig.Instance().DatabaseConfig.AlwaysRegenerateInfoDBs);
        if (DBAppInfo.GetAppInfoCache() == null)
        {
            if (File.Exists("Content/appinfo.vdf"))
            {
                Console.WriteLine("Reading AppInfo... Might take some time");
                AppInfoReader.Read("Content/appinfo.vdf"); ;
            }
        }
        if (DBPackageInfo.GetPackageInfoCache() == null)
        {
            if (File.Exists("Content/packageinfo.vdf"))
            {
                Console.WriteLine("Reading PackageInfo... Might take some time");
                PackageInfoReader.Read("Content/packageinfo.vdf");
            }
        }
        AppInfoExtra.ReadAll();
        SslContext context = CertHelper.GetContext(SslProtocols.Tls12, $"Keys/global.pfx", "global");
        WS_Server serverWeb = new("192.168.1.50", 80);
        serverWeb.HTTP_AttributeToMethods.Merge(Assembly.GetAssembly(typeof(ServerCore)));
        serverWeb.WS_AttributeToMethods = AttributeMethodHelper.UrlWSLoader(Assembly.GetAssembly(typeof(ServerCore)));
        serverWeb.ReceivedFailed += ReceivedFailed;
        serverWeb.Start();
        WSS_Server serverWebSSL = new(context, "192.168.1.50", 443);
        serverWebSSL.HTTP_AttributeToMethods.Merge(Assembly.GetAssembly(typeof(ServerCore)));
        serverWebSSL.WS_AttributeToMethods = AttributeMethodHelper.UrlWSLoader(Assembly.GetAssembly(typeof(ServerCore)));
        serverWebSSL.ReceivedFailed += ReceivedFailed;
        serverWebSSL.Start();
        //this sending out our IP's and our MAC ADDRESS. (Or the clients)
        //STEAMDISCOVER server?
        /*
        var NoIdeaServer = new UDPServerBase("NoIdea", "192.168.1.50", 27036);
        NoIdeaServer.Start();
       */
        var CellPingServer = new UDPServerBase("CellPingServer", "192.168.1.50", 27019);
        CellPingServer.Start();
        Logger.PWLog("Everything Started!");
    }

    private static void ReceivedFailed(object? sender, HttpRequest request)
    {
        try
        {
            File.AppendAllText("REQUESTED.txt", request.ToString() + "\n" + request.Body + "\n");
        }
        catch { }
        Console.Write("something isnt good: ");
        Console.Write(request.Method + "  ");
        Console.WriteLine(request.Url);
    }
}