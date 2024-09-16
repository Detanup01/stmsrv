using ModdableWebServer.Attributes;
using ModdableWebServer;
using ModdableWebServer.Helper;
using NetCoreServer;
using Steam3Server.Others;
using Steam3Server.Servers;
using Steam3Server.SQL;
using System.Text;
using Steam3Kit.MSG;

namespace Steam3Server.HTTPServer.Responses
{
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
            string des_string = Encoding.UTF8.GetString(app.DataByte);
            des_string = des_string.Replace("\"\t\"", "\"\t\t\"");
            var toZip = Encoding.UTF8.GetBytes(des_string).Concat(new byte[] { 0x0 }).ToArray();
            var data = VDFParserExt.ParseAppInfoGZ(toZip);
            ResponseCreator creator = new();
            creator.SetHeader("Content-Type", "application/gzip");
            creator.SetBody(data);
            serverStruct.SendResponse(creator.GetResponse());
            Console.WriteLine($"appinfo http for {appid}: OK!");
            return true;
        }
        public static void Response((NetCoreServer.HttpRequest request, HTTPServerSession session) e)
        {
            var splittedURL = e.request.Url.Split("/sha/");
            var appid = splittedURL[0].Split("appinfo/")[1];
            Console.WriteLine(appid);
            var hash = splittedURL[1].Replace(".txt.gz","");

            var app = DBAppInfo.GetApp(uint.Parse(appid));

            var appHash = BitConverter.ToString(app.Hash).Replace("-","");
            var appBinHash = BitConverter.ToString(app.BinaryDataHash);

            Console.WriteLine(hash.ToUpper() + " vs " + appHash + " vs " + appBinHash);

            //if (hash.ToUpper() == appHash)
            var data = VDFParserExt.ParseAppInfoGZ(app.DataByte);
            ResponseCreator creator = new();
            creator.SetHeader("Content-Type", "application/gzip");
            creator.SetBody(data);
            e.session.SendResponse(creator.GetResponse());
            Console.WriteLine($"appinfo http for {appid}: OK!");
        }
    }
}
