using ModdableWebServer.Attributes;
using ModdableWebServer;
using ModdableWebServer.Helper;
using NetCoreServer;
using DB4Steam;
using PICS_Backend;

namespace Steam3Server.HTTPServer.Responses;

internal class AppInfoResponse
{
    [HTTP("GET", "/appinfo/{appid}/sha/{hash}.txt.gz")]
    public static bool ServerStatus(HttpRequest _, ServerStruct serverStruct)
    {
        var appid = serverStruct.Parameters["appid"];
        var hash = serverStruct.Parameters["hash"];
        if (!uint.TryParse(appid, out uint u_appid))
        {
            Console.WriteLine($"appid cannot be converted to uint ({appid})");
            serverStruct.Response.MakeErrorResponse("appid cannot be converted to uint");
            serverStruct.SendResponse();
            return true;
        }
        var app = DBApp.GetApp(u_appid);
        if (app == null)
        {
            Console.WriteLine("not app found under this appid");
            serverStruct.Response.MakeErrorResponse("not app found under this appid");
            serverStruct.SendResponse();
            return true;
        }
        var appHash = BitConverter.ToString(app.Hash).Replace("-", "");
        var appBinHash = BitConverter.ToString(app.BinaryDataHash).Replace("-","");

        Console.WriteLine(hash.ToUpper() + " vs " + appHash + " vs " + appBinHash);

        //if (hash.ToUpper() == appHash)
        ResponseCreator creator = new();
        creator.SetHeader("Content-Type", "application/gzip");
        creator.SetBody(VDFHelper.ParseAppInfoGZ(app.GetAppInfoStringData()));
        serverStruct.SendResponse(creator.GetResponse());
        Console.WriteLine($"appinfo http for {appid}: OK!");
        return true;
    }
}
