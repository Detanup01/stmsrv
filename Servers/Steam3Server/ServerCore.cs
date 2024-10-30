using System.Security.Authentication;
using NetCoreServer;
using Steam3Server.Servers;
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
    static WS_Server? ServerWeb;
    static WSS_Server? ServerWebSLL;
    public static void Start()
    {
        DebugPrinter.EnableLogs = true;
        MainConfig.Instance();
        DBPrepare.Prepare(MainConfig.Instance().DatabaseConfig.AlwaysRegenerateInfoDBs);
        CustomPICSVersioning.Init();
        AppInfoExtra.ReadAll();
        PackageInfoExtra.ReadAll();
        SslContext context = CertHelper.GetContext(SslProtocols.Tls12, $"Keys/global.pfx", "global");
        ServerWeb = new("192.168.1.50", 80);
        ServerWeb.HeaderAttributeToMethods.Merge(Assembly.GetAssembly(typeof(ServerCore)));
        ServerWeb.HTTP_AttributeToMethods.Merge(Assembly.GetAssembly(typeof(ServerCore)));
        ServerWeb.WS_AttributeToMethods = AttributeMethodHelper.UrlWSLoader(Assembly.GetAssembly(typeof(ServerCore)));
        ServerWeb.ReceivedFailed += ReceivedFailed;
        ServerWeb.Start();
        ServerWebSLL = new(context, "192.168.1.50", 443);
        ServerWebSLL.HeaderAttributeToMethods.Merge(Assembly.GetAssembly(typeof(ServerCore)));
        ServerWebSLL.HTTP_AttributeToMethods.Merge(Assembly.GetAssembly(typeof(ServerCore)));
        ServerWebSLL.WS_AttributeToMethods.Merge(Assembly.GetAssembly(typeof(ServerCore)));
        ServerWebSLL.ReceivedFailed += ReceivedFailed;

        ServerWebSLL.Start();
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

    public static void Stop()
    {
        ServerWeb?.Stop();
        ServerWebSLL?.Stop();
        CustomPICSVersioning.Quit();
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