using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using ModdableWebServer;
using NetCoreServer;
using UtilsLib;

namespace Steam3Server.HTTPServer
{
    public class ClientUpdater
    {
        [HTTPHeader("GET", "/{file}", "host", "client-update.steamstatic.com")]
        public static bool ClientUpdate(HttpRequest _, ServerStruct serverStruct)
        {
            var file = serverStruct.Parameters["file"];
            string file_path = Path.Combine("ClientUpdates", file);
            if (!File.Exists(file_path))
            {
                Logger.PWLog($"client update asking for file: {file}");
                serverStruct.Response.MakeErrorResponse();
                serverStruct.SendResponse();
                return true;
            }
            string last_modified = File.GetLastWriteTime(file_path).ToString("R");
            byte[] bytes = File.ReadAllBytes(file_path);
            ResponseCreator creator = new();
            creator.SetHeader("Content-Type", "application/octet-stream");
            creator.SetHeader("Last-Modified", last_modified);
            creator.SetHeader("Accept-Ranges", "bytes");
            creator.SetHeader("Content-Length", bytes.Length.ToString());
            creator.SetBody(bytes);
            serverStruct.SendResponse(creator.GetResponse());
            return true;
        }
    }
}
