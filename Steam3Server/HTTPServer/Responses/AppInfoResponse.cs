using Steam3Server.Others;
using Steam3Server.Servers;
using Steam3Server.SQL;

namespace Steam3Server.HTTPServer.Responses
{
    internal class AppInfoResponse
    {
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
            MemoryStream memory = new();
            var gzip = new ValveAppInfo_GZ(memory);
            gzip.Write(app.DataByte);
            gzip.Close();
            ResponseCreator creator = new();
            creator.SetBody(memory.ToArray());
            e.session.SendResponse(creator.GetResponse());

        }
    }
}
