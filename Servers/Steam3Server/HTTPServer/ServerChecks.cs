using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using ModdableWebServer.Helper;

namespace Steam3Server.HTTPServer;

internal class ServerChecks
{
    [HTTP("GET", "/server-status")]
    public static bool ServerStatus(HttpRequest _, ServerStruct serverStruct)
    {
        serverStruct.Response.MakeErrorResponse();
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("GET", "/204")]
    public static bool Check204(HttpRequest _, ServerStruct serverStruct)
    {
        serverStruct.Response.MakeOkResponse(204);
        serverStruct.SendResponse();
        return true;
    }
    [HTTP("GET", "/ipv6check")]
    public static bool ipv6check(HttpRequest _, ServerStruct serverStruct)
    {
        serverStruct.Response.MakeErrorResponse();
        serverStruct.SendResponse();
        return true;
    }
}
