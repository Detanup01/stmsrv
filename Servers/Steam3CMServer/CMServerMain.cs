using ModdableWebServer.Helper;
using ModdableWebServer.Servers;
using NetCoreServer;
using System.Reflection;
using System.Security.Authentication;

namespace Steam3CMServer;

public class CMServerMain
{
    public static WSS_Server? CMServer;

    public static void Start(string CMServerIP, int CMServerPort)
    {
        SslContext context = CertHelper.GetContext(SslProtocols.Tls12, $"Keys/global.pfx", "global");
        CMServer = new(context, CMServerIP, CMServerPort);
        CMServer.HTTP_AttributeToMethods.Merge(Assembly.GetAssembly(typeof(CMServerMain)));
        CMServer.WS_AttributeToMethods.Merge(Assembly.GetAssembly(typeof(CMServerMain)));
        CMServer.Start();
    }

    public static void Stop()
    {
        CMServer?.Stop();
    }
}
