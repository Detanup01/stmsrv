using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using NetCoreServer;
using Steam3Server.Servers;
using Steam3Server.Others;
using Steam3Server.SQL;
using UtilsLib;
using Steam3Server.HTTPServer;
using Steam3Server.Settings;
using ModdableWebServer.Servers;
using ModdableWebServer.Helper;
using System.Reflection;

namespace Steam3Server
{
    public class ServerCore
    {
        public static void Start()
        {
            DebugPrinter.EnableLogs = true;
            MainConfig.Instance();
            DBPrepare.Prepare();
            if (DBAppInfo.GetAppInfoCache() == null)
            {
                if (File.Exists("Content/appinfo.vdf"))
                {
                    Console.WriteLine("Reading AppInfo... Might take some time");
                    AppInfoReader.Read("Content/appinfo.vdf"); ;
                }
            }
            if (DBPackageInfo.GetPackageInfoCache() == null)
            {
                if (File.Exists("Content/packageinfo.vdf"))
                {
                    Console.WriteLine("Reading PackageInfo... Might take some time");
                    PackageInfoReader.Read("Content/packageinfo.vdf");
                }
            }
            AppInfoExtra.ReadAll();
            SslContext context = new(SslProtocols.Tls12, GetCert());
            /*
            var serverhttp = new HTTPServerBase("SteamWeb", "192.168.1.50", 80);
            serverhttp.Start();*/

            WS_Server serverWeb = new("192.168.1.50", 80);
            serverWeb.HTTP_AttributeToMethods.Merge(Assembly.GetAssembly(typeof(ServerCore)));
            serverWeb.WS_AttributeToMethods = AttributeMethodHelper.UrlWSLoader(Assembly.GetAssembly(typeof(ServerCore)));
            serverWeb.ReceivedFailed += ReceivedFailed;
            serverWeb.Start();
            WSS_Server serverWebSSL = new(context, "192.168.1.50", 443);
            serverWebSSL.HTTP_AttributeToMethods.Merge(Assembly.GetAssembly(typeof(ServerCore)));
            serverWebSSL.WS_AttributeToMethods = AttributeMethodHelper.UrlWSLoader(Assembly.GetAssembly(typeof(ServerCore)));
            serverWebSSL.ReceivedFailed += ReceivedFailed;
            serverWebSSL.Start();
            /*
            serverhttp.EventConnected += Serverhttp_EventConnected;
            serverhttp.EventDisconnected += Serverhttp_EventDisconnected;
            var SteamWebWSS = new WSSServerBase("SteamWebWSS", context, "192.168.1.50", 443);
            SteamWebWSS.Start();
            SteamWebWSS.EventConnected += Overrides.SteamWebWSS_EventConnected;
            SteamWebWSS.EventDisconnected += Overrides.SteamWebWSS_EventDisconnected;
            var Content3Server = new SSLServerBase("Content3Server", context, "192.168.1.50", 8881);
            Content3Server.Start();
            var Content3DeliveryNetworkServer = new SSLServerBase("Content3DeliveryNetworkServer", context, "192.168.1.50", 8882);
            Content3DeliveryNetworkServer.Start();
            var CMServer = new SSLServerBase("CMServer", context, "192.168.1.50", 27017);
            CMServer.Start();
            var GeneralDirectoryServer = new SSLServerBase("GeneralDirectoryServer", context, "192.168.1.50", 27030);
            GeneralDirectoryServer.Start();
            var DrmFSServer = new SSLServerBase("DrmFSServer", context, "192.168.1.50", 27031);
            DrmFSServer.Start();
            var ContentServerDirectoryServer = new SSLServerBase("ContentServerDirectoryServer", context, "192.168.1.50", 28032);
            ContentServerDirectoryServer.Start();
            var MasterAuthenticationServer = new SSLServerBase("MasterAuthenticationServer", context, "192.168.1.50", 28039);
            MasterAuthenticationServer.Start();
            var SlaveAuthenticationServer = new SSLServerBase("SlaveAuthenticationServer", context, "192.168.1.50", 28040);
            SlaveAuthenticationServer.Start();
            var FileUploadServer = new SSLServerBase("FileUploadServer", context, "192.168.1.50", 28100);
            FileUploadServer.Start();
            var ClientUpdateServer = new SSLServerBase("ClientUpdateServer", context, "192.168.1.50", 28101);
            ClientUpdateServer.Start();
            var Content2Server = new SSLServerBase("Content2Server", context, "192.168.1.50", 28102);
            Content2Server.Start();
            var ConfigServer = new SSLServerBase("ConfigServer", context, "192.168.1.50", 28138);
            ConfigServer.Start();
            var StunServer = new UDPServerBase("StunServer", "192.168.1.50", 3478);
            StunServer.Start();
            var StunServer2 = new UDPServerBase("StunServer2", "192.168.1.50", 3479);
            StunServer2.Start();
            var TurnServer = new UDPServerBase("TurnServer", "192.168.1.50", 4380);
            TurnServer.Start();
            var CMServerUDP = new UDPServerBase("CMServerUDP", "192.168.1.50", 27017);
            CMServerUDP.Start();
            CMServerUDP.ServerRecieved += Overrides.CMServerUDP_ServerRecieved;
            
            //this sending out our IP's and our MAC ADDRESS. (Or the clients)
            //STEAMDISCOVER server?
            /*
            var NoIdeaServer = new UDPServerBase("NoIdea", "192.168.1.50", 27036);
            NoIdeaServer.Start();
           */
            var CellPingServer = new UDPServerBase("CellPingServer", "192.168.1.50", 27019);
            CellPingServer.Start();
            /*
            var HLMasterServer = new UDPServerBase("HLMasterServer", "192.168.1.50", 28010);
            HLMasterServer.Start();
            var SourceMasterServer = new UDPServerBase("SourceMasterServer", "192.168.1.50", 28011);
            SourceMasterServer.Start();
            var RDKFMasterServer = new UDPServerBase("RDKFMasterServer", "192.168.1.50", 28012);
            RDKFMasterServer.Start();
            var ReportingServer = new UDPServerBase("ReportingServer", "192.168.1.50", 28013);
            ReportingServer.Start();*/
            Debug.PrintDebug("Everything Started!");
        }
        public static X509Certificate GetCert()
        {
            X509Certificate2 cert = new(File.ReadAllBytes($"Keys/global.pfx"), "global");
            return cert;
        }

        private static void ReceivedFailed(object? sender, HttpRequest request)
        {
            try
            {
                File.AppendAllText("REQUESTED.txt", request.ToString() + "\n" + request.Body + "\n");
            }
            catch (IOException e) { }
            Console.Write("something isnt good: ");
            Console.Write(request.Method + "  ");
            Console.WriteLine(request.Url);
        }
    }
}