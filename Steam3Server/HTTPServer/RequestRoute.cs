using Steam3Server.Servers;
using UtilsLib;

namespace Steam3Server.HTTPServer
{
    public class RequestRoute
    {

        public static void HTTPS_RequestReceived(object? sender, (NetCoreServer.HttpRequest request, WSSSessionBase session) e)
        {
            if (e.request.Url.ToLower().Contains("customapi"))
            {
                e.session.SendResponseAsync(CustomAPI.HandleAPIRequest(e.request));
            }
            if (e.session.Headers["host"].Contains("store.steampowered.com") && e.request.Url.Contains("/join/"))
            {
                var rsp = e.session.Response.MakeGetResponse(File.ReadAllText("WWW/join.html"), "text/html;charset=UTF-8");
                Debug.PWDebug(rsp);
                e.session.SendResponseAsync(rsp);
            }
            if (e.session.Headers["host"].Contains("store.steampowered.com") && !e.request.Url.Contains("/join/"))
            {
                var rsp = e.session.Response.MakeGetResponse(File.ReadAllText("WWW/store-home.html"), "text/html;charset=UTF-8");
                Debug.PWDebug(rsp);
                e.session.SendResponseAsync(rsp);
            }
        }


    }
}
