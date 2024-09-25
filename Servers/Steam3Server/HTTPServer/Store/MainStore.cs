using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using ModdableWebServer;
using NetCoreServer;

namespace Steam3Server.HTTPServer.Store;

internal class MainStore
{
    [HTTPHeader("GET", "/", "host", "store.steampowered.com")]
    public static bool Store(HttpRequest _, ServerStruct serverStruct)
    {
        serverStruct.Response.MakeGetResponse(File.ReadAllText("WWW/store-home.html"), "text/html;charset=UTF-8");
        serverStruct.SendResponse();
        return true;
    }

    [HTTPHeader("GET", "/join/", "host", "store.steampowered.com")]
    public static bool Join(HttpRequest _, ServerStruct serverStruct)
    {
        serverStruct.Response.MakeGetResponse(File.ReadAllText("WWW/join.html"), "text/html;charset=UTF-8");
        serverStruct.SendResponse();
        return true;
    }
}
