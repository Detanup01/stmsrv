using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using System.Text;
using ModdableWebServer.Helper;
using Steam3Kit.Types;
using Steam3Server.Settings;

namespace Steam3Server.HTTPServer.Responses;

internal class SteamDirectory
{
    [HTTP("GET", "/ISteamDirectory/GetCMListForConnect/{version}?{args}")]
    public static bool GetCMListForConnect(HttpRequest _, ServerStruct serverStruct)
    {
        ResponseCreator responseCreator = new(200);
        if (serverStruct.Parameters.ContainsKey("format") && serverStruct.Parameters["format"] == "vdf")
        {
            using MemoryStream memoryStream = new MemoryStream();
            MakeResponseKV().SaveToStream(memoryStream, false);
            Console.WriteLine(Encoding.UTF8.GetString(memoryStream.ToArray()));
            responseCreator.SetBody(memoryStream.ToArray());
            memoryStream.Dispose();
            serverStruct.Response = responseCreator.GetResponse();
        }
        else
        {
            serverStruct.Response.MakeOkResponse();
        }
        serverStruct.SendResponse();
        return true;
    }

    public static KeyValue MakeResponseKV()
    {
        KeyValue success = new KeyValue("success", "1");
        KeyValue message = new KeyValue("message", "");
        KeyValue serverlist = new KeyValue("serverlist");
        KeyValue response = new KeyValue("response");
        if (MainConfig.Instance().CMServerConfigs.Count == 0)
        {
            serverlist.Children.AddRange([CreateDefaultCM()]);
            response.Children.AddRange([serverlist, success, message]);
            return response;
        }
        var cmconfig = MainConfig.Instance().CMServerConfigs;
        for (int i = 0; i < cmconfig.Count; i++)
        {
            serverlist.Children.Add(CreateCM(cmconfig[i], i));
        }
        response.Children.AddRange([serverlist, success, message]);
        return response;
    }

    public static KeyValue CreateDefaultCM()
    {
        KeyValue serverListMember_endpoint = new("endpoint", "cm.steampowered.com:443");
        KeyValue serverListMember_legacy_endpoint = new("legacy_endpoint", "cm.steampowered.com:443");
        KeyValue serverListMember_type = new("type", "websockets");
        KeyValue serverListMember_dc = new("dc", "fra1");
        KeyValue serverListMember_realm = new("realm", "steamglobal");
        KeyValue serverListMember_load = new("load", "10");
        KeyValue serverListMember_wtd_load = new("wtd_load", "50.6077117919921875");
        KeyValue serverList_0 = new KeyValue("0");
        serverList_0.Children.AddRange([serverListMember_endpoint, serverListMember_legacy_endpoint, serverListMember_type, serverListMember_dc, serverListMember_realm, serverListMember_load, serverListMember_wtd_load]);
        return serverList_0;
    }

    public static KeyValue CreateCM(CMServerConfig serverConfig, int server_count)
    {
        KeyValue serverListMember_endpoint = new("endpoint", serverConfig.EndPoint);
        KeyValue serverListMember_legacy_endpoint = new("legacy_endpoint", serverConfig.EndPoint);
        KeyValue serverListMember_type = new("type", serverConfig.CMType);
        KeyValue serverListMember_dc = new("dc", serverConfig.Destination);
        KeyValue serverListMember_realm = new("realm", "steamglobal");
        KeyValue serverListMember_load = new("load", serverConfig.Load.ToString());
        KeyValue serverListMember_wtd_load = new("wtd_load", serverConfig.WTD_Load.ToString());
        KeyValue serverList = new KeyValue(server_count.ToString());
        serverList.Children.AddRange([serverListMember_endpoint, serverListMember_legacy_endpoint, serverListMember_type, serverListMember_dc, serverListMember_realm, serverListMember_load, serverListMember_wtd_load]);
        return serverList;
    }
}
