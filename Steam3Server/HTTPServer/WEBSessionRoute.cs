using NetCoreServer;
using Steam3Server.HTTPServer.Responses;
using Steam3Server.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilsLib;

namespace Steam3Server.HTTPServer
{
    internal class WEBSessionRoute
    {

        public static void SteamWEBSession_ReceivedRequest(object? sender, (HttpRequest request, HTTPServerSession session) e)
        {
            //Debug.PWDebug("SteamWEBSession_ReceivedRequest ovverride?");
            Debug.WriteDebug(e.request.Url, "SteamWEBSession_ReceivedRequest");
            if (e.request.Url == "/server-status")
            {
                e.session.SendResponseAsync(e.session.Response.MakeErrorResponse());
            }
            else
            {
                if (e.session.Headers["host"].Contains("clientconfig.local.steamstatic.com") && e.request.Url.Contains("/appinfo/"))
                {
                    AppInfoResponse.Response(e);
                }
                else
                {
                    e.session.SendResponseAsync(e.session.Response.MakeOkResponse());
                }
                

            }

        }
    }
}
