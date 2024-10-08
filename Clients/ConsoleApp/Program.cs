﻿
using Google.Protobuf;
using ModdableWebServer;
using ModdableWebServer.Helper;
using Steam.Messages.ClientServer;
using Steam.Messages.ClientServer.Login;
using Steam3CMServer.Packets;
using Steam3Kit;
using Steam3Kit.MSG;
using Steam3Server.CMServer.Packets;


namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPacket packet = new FakeResponse();
            var protob = new MsgHdrProtoBuf()
            { 
                Msg = EMsg.ClientGetAppOwnershipTicket,
                Proto = new()
                { 
                    ClientSessionid = 34,
                    Steamid = 666
                }
            };
            CMsgClientLogon CMsg = new()
            { 
                ClientSuppliedSteamId = new Steam3Kit.Types.SteamID(777, EUniverse.Public, EAccountType.Individual).ConvertToUInt64(),
                AccountName = "sfgsdfg"
            };
            MemoryStream ms = new MemoryStream();
            protob.Serialize(ms);
            CMsg.WriteTo(ms);
            var packetClientMsgProtobuf = new PacketClientMsgProtobuf(EMsg.ClientUseLocalDeviceAuthorizations, ms.ToArray());
            packet = new LocalDeviceAuth2(packetClientMsgProtobuf, new());

            packet.Start();
            //Steam3Server.ServerCore.Start();
            /*
            Console.WriteLine(args[0]);
            MemoryStream stream = new MemoryStream(File.ReadAllBytes(args[0]));
            int len = (int)stream.Length;
            byte[] dest = new byte[1048576 * 1024];
            int size = Steam3Kit.Utils.VZipUtil.Decompress(stream, dest, out uint crc_timestamp, true);
            var xx = dest.Take(size).ToArray();
            File.WriteAllBytes("test.zip", xx);*/
        }


    }
}


/*
 * 
 *             
        Steam3Server.ServerCore.Start();
        InfoExt.stringTable = new(File.ReadAllLines("_stringpool.txt"));
        AppHashCheck(7);
        AppHashCheck(440);
        AppHashCheck(480);
        Console.ReadLine();
            
        static void AppHashCheck(uint appid)
        {
            var app_appid = DBAppInfo.GetApp(appid);
            if (app_appid == null)
            {
                Console.WriteLine("app with aooud doesnt exist: " + appid);
                return;
            }
            Console.WriteLine("binarydata hash: " + Convert.ToHexString(app_appid.BinaryDataHash));
            string should_bindataHash = Convert.ToHexString(SHA1.HashData(app_appid.DataByte));
            Console.WriteLine("should be same as binarydata hash: " + should_bindataHash);
            var data = app_appid.GetAppInfoStringData();
            string appHash = Convert.ToHexString(app_appid.Hash);
            string should_appHash = Convert.ToHexString(SHA1.HashData(data));
            Console.WriteLine("app hash: " + appHash);
            Console.WriteLine("should be same as app hash: " + should_appHash);
            if (appHash != should_appHash)
            {
                Console.WriteLine("appid isnt good! " + appid);
                Console.WriteLine("plan B: " + Convert.ToHexString(SHA1.HashData(app_appid.DataByte.DataFromKVBinToKVText_StringTable())));
                File.WriteAllBytes($"apps/{appid}_plan_b.txt", app_appid.DataByte.DataFromKVBinToKVText_StringTable());
            }
        }


using var ms = new MemoryStream(File.ReadAllBytes("4a6bb73b6b2a142dec14a48219e744889e3ac8b7.txt.gz_ori"));
using var gz_reader = new BinaryReader(ms, System.Text.Encoding.UTF8, true);

var GZIPMagic = gz_reader.ReadUInt16();
var GZIP_COMP_METHOD = gz_reader.ReadByte();
var Flags = gz_reader.ReadByte();
var timestamp = gz_reader.ReadInt32();
var comp_lvl = gz_reader.ReadByte();
var os = gz_reader.ReadByte();
Console.WriteLine(GZIPMagic.ToString());
Console.WriteLine(GZIP_COMP_METHOD.ToString());
Console.WriteLine(Flags.ToString());
Console.WriteLine(timestamp.ToString());
Console.WriteLine(comp_lvl.ToString());
Console.WriteLine(os.ToString());
//
MemoryStream memory = new();
var gzip =  new customGZIP(memory);
gzip.Write(File.ReadAllBytes("4a6bb73b6b2a142dec14a48219e744889e3ac8b7.txt"));
gzip.Close();
File.WriteAllBytes("4a6bb73b6b2a142dec14a48219e744889e3ac8b7_test.txt.gz", memory.ToArray());
*/

/*
var decomp = VZipUtil.Decompress(File.ReadAllBytes("4a6bb73b6b2a142dec14a48219e744889e3ac8b7.txt.gz"));
File.WriteAllBytes("4a6bb73b6b2a142dec14a48219e744889e3ac8b7_test",decomp);
*/




/*
string x = File.ReadAllText("ticket.txt");
var bytes_long = Enumerable.Range(0, x.Length / 2)
.Select(i => Convert.ToByte(x.Substring(i * 2, 2), 16))
.ToArray();
var ticketstruct = AppTickets.GetTicket(bytes_long);
//Console.WriteLine(IPAddress.Parse("127.0.0.1").Address);
Console.WriteLine(ticketstruct.ToString());
/*
Console.WriteLine();
var steamID = new SteamID(666, Steam3Kit.EUniverse.Public, Steam3Kit.EAccountType.Individual);
var bytes = AppTickets.CreateTicket(new()
{ 
    AppId = ticketstruct.AppID,
    DLC = ticketstruct.DLC,
    SteamID = steamID,
    HasGCToken = ticketstruct.HasGC,
    GcToken = ticketstruct.HasGC ? ticketstruct.GC.GcToken : 0,
    Licenses = ticketstruct.Licenses,
    OwnershipFlags = ticketstruct.OwnershipFlags,
    OwnershipTicketExternalIP = IPAddress.Parse("192.168.3.50"),
    OwnershipTicketInternalIP = IPAddress.Parse("192.168.1.50"),
    Version = ticketstruct.Version,

});

Console.WriteLine(BitConverter.ToString(bytes).Replace("-",""));
Console.WriteLine(bytes.Length);
*/
// Console.WriteLine(ticketstruct.ToCensored());
/*
Console.WriteLine();

ticketstruct = AppTickets.GetTicket(bytes_long);
Console.WriteLine(ticketstruct.ToString());
Console.WriteLine();
Console.WriteLine(ticketstruct.ToCensored());*/

/*
var Ticketbytes = AppTickets.CreateTicket(ticketstruct.SteamId, ticketstruct.AppId, ticketstruct.IP_Public, ticketstruct.IP_Private, ticketstruct.TicketFlags, new() { 0 }, new List<AppTickets.dlc>() { });
ticketstruct = AppTickets.GetTicket(Ticketbytes);
AppTickets.PrintTicket(ticketstruct);
*/
/*
DBPrepare.Prepare();
if (File.Exists("Content/appinfo.vdf"))
{
    Console.WriteLine("Reading AppInfo... Might take some time");
    AppInfoReader.Read("Content/appinfo.vdf");
    //Console.WriteLine(JsonConvert.SerializeObject(AppInfoReader.Items));
}
if (File.Exists("Content/packageinfo.vdf"))
{
    Console.WriteLine("Reading PackageInfo... Might take some time");
    PackageInfoReader.Read("Content/packageinfo.vdf");
}
AppInfoExtra.ReadAll();

Console.WriteLine("appinfo!");
try
{
    // send this as appinfo!
    var app = DBAppInfo.GetApp(218);
    File.WriteAllBytes("218.bytes.txt", app.DataByte);

    var sp = Stopwatch.StartNew();
    using var ms = new MemoryStream(app.DataByte);
    KeyValue kv = new();
    kv.TryReadAsBinary(ms);
    using var ms2 = new MemoryStream();
    kv.SaveToStream(ms2, false);
    sp.Stop();
    Console.WriteLine(sp.Elapsed);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
try
{
    // send this as appinfo!
    var app = DBAppInfo.GetApp(218);
    File.WriteAllBytes("218.bytes2.txt", app.DataByte);
    var sp = Stopwatch.StartNew();
    var appinfo = VDFParserExt.ParseAppInfo(app.DataByte);
    using var ms = new MemoryStream(appinfo, 0, appinfo.Length - 1);
    KeyValue kv = new();
    kv.TryReadAsBinary(ms);
    using var ms2 = new MemoryStream();
    kv.SaveToStream(ms2, false);
    File.WriteAllBytes("218.vdf2.txt", ms2.ToArray());
    sp.Stop();
    Console.WriteLine(sp.Elapsed);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

Console.WriteLine("pkginfo!");
try
{
    // send this as appinfo!
    var pkg = DBPackageInfo.GetPackage(0);
    File.WriteAllBytes("0_pkginfo.bytes.txt", pkg.DataBytes);
    using var ms = new MemoryStream(pkg.DataBytes);
    KeyValue kv = new();
    kv.TryReadAsBinary(ms);
    Console.WriteLine(kv.Name);
    using var ms2 = new MemoryStream();
    kv.SaveToStream(ms2, false);
    File.WriteAllBytes("0_pkginfo.vdf.txt", ms2.ToArray());
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
*/

/*
var txt = File.ReadAllBytes("33A355AD45272DEFC8098366824961C15B8A7C28.txt");

var comp = GZip.Compress(txt);
File.WriteAllBytes("test.txt.gz",comp);
*/


//var sevev = AppInfoReader.App7Node;
/*
var app = DBAppInfo.GetApp(480);
Console.WriteLine(Encoding.UTF8.GetString(app.DataByte));
var appinf = AppInfoNodeExt.ReadEntries(app.DataByte);
Console.WriteLine(AppInfoNodeKV.ParseToVDF(appinf));*/
//var readed = AppInfoNodeExt.ReadEntries(app.DataByte);
//Console.WriteLine(BitConverter.ToString(app.DataByte).Replace("-"," "));

/*
Console.WriteLine(AppInfoNodeKV.ParseToVDF(sevev));
var bin = AppInfoNodeKV.ParseToBin(sevev);
Console.WriteLine(BitConverter.ToString(bin).Replace("-"," "));
var appinf = AppInfoNodeExt.ReadEntries(bin);
Console.WriteLine(AppInfoNodeKV.ParseToVDF(appinf));
/*
var sevev = AppInfoReader.App7Node;
//Console.WriteLine(JsonConvert.SerializeObject(sevev, Formatting.Indented));
Console.WriteLine(BitConverter.ToString(AppInfoNodeKV.ParseToBin(sevev)));
/*
var app = DBAppInfo.GetApp(7);
Console.WriteLine(BitConverter.ToString(app.DataByte));
MemoryStream mem = new(app.DataByte);
BinaryReader binaryReader = new BinaryReader(mem);
var ext = AppInfoNodeExt.ReadEntries(binaryReader);
Console.WriteLine(ext);*/