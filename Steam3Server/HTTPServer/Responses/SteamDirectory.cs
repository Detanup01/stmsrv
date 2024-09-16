using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModdableWebServer.Helper;
using Steam3Kit.Types;
using UtilsLib;
using Steam3Server.Servers;

namespace Steam3Server.HTTPServer.Responses;

internal class SteamDirectory
{
    [HTTP("GET", "/ISteamDirectory/GetCMListForConnect/{version}?{args}")]
    public static bool ServerStatus(HttpRequest _, ServerStruct serverStruct)
    {
        ResponseCreator responseCreator = new(200);
        if (serverStruct.Parameters.ContainsKey("format") && serverStruct.Parameters["format"] == "vdf")
        {
            Debug.PWDebug("VDF!!");
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

    [HTTP("GET", "/cmping/")]
    public static bool CMPing(HttpRequest _, ServerStruct serverStruct)
    {
        Console.WriteLine("CMPING");
        ResponseCreator responseCreator = new(200);
        responseCreator.SetHeader("X-Steam-CMLoad", "20");
        responseCreator.SetHeader("Content-Length", "0");
        responseCreator.SetHeader("Date", DateTime.UtcNow.ToString("R"));
        serverStruct.Response = responseCreator.GetResponse();
        serverStruct.SendResponse();
        return true;
    }




    public static KeyValue MakeResponseKV()
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
        /*
        KeyValue serverListMember1_endpoint = new("endpoint", "cm.steampowered.com:443");
        KeyValue serverListMember1_legacy_endpoint = new("legacy_endpoint", "cm.steampowered.com:443");
        KeyValue serverListMember1_wtd_load = new("wtd_load", "52.6077117919921875");
        KeyValue serverList_1 = new KeyValue("1");
        serverList_1.Children.AddRange([serverListMember1_endpoint, serverListMember1_legacy_endpoint, serverListMember_type, serverListMember_dc, serverListMember_realm, serverListMember_load, serverListMember1_wtd_load]);


        KeyValue serverListMember2_endpoint = new("endpoint", "cm.steampowered.com:8882");
        KeyValue serverListMember2_legacy_endpoint = new("legacy_endpoint", "cm.steampowered.com:8882");
        KeyValue serverListMember2_wtd_load = new("wtd_load", "54.6077117919921875");
        KeyValue serverList_2 = new KeyValue("2");
        serverList_2.Children.AddRange([serverListMember2_endpoint, serverListMember2_legacy_endpoint, serverListMember_type, serverListMember_dc, serverListMember_realm, serverListMember_load, serverListMember2_wtd_load]);

        KeyValue serverListMember3_endpoint = new("endpoint", "cm.steampowered.com:80");
        KeyValue serverListMember3_legacy_endpoint = new("legacy_endpoint", "cm.steampowered.com:80");
        KeyValue serverListMember3_wtd_load = new("wtd_load", "55.6077117919921875");
        KeyValue serverList_3 = new KeyValue("3");
        serverList_3.Children.AddRange([serverListMember3_endpoint, serverListMember3_legacy_endpoint, serverListMember_type, serverListMember_dc, serverListMember_realm, serverListMember_load, serverListMember3_wtd_load]);
        */

        KeyValue success = new KeyValue("success", "1");
        KeyValue message = new KeyValue("message", "");
        KeyValue serverlist = new KeyValue("serverlist");
        serverlist.Children.AddRange([serverList_0]);
        KeyValue response = new KeyValue("response");
        response.Children.AddRange([serverlist, success, message]);
        return response;
    }
}
