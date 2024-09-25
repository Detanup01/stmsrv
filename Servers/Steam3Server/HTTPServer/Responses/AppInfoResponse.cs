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
        Console.WriteLine(appid);
        var hash = serverStruct.Parameters["hash"];
        Console.WriteLine(hash);
        var app = DBAppInfo.GetApp(uint.Parse(appid));

        var appHash = BitConverter.ToString(app.Hash).Replace("-", "");
        var appBinHash = BitConverter.ToString(app.BinaryDataHash);

        Console.WriteLine(hash.ToUpper() + " vs " + appHash + " vs " + appBinHash);

        //if (hash.ToUpper() == appHash)
        var data = InfoExt.ParseAppInfoGZ(app.GetAppInfoStringData());
        ResponseCreator creator = new();
        creator.SetHeader("Content-Type", "application/gzip");
        creator.SetBody(data);
        serverStruct.SendResponse(creator.GetResponse());
        Console.WriteLine($"appinfo http for {appid}: OK!");
        return true;
    }
}
