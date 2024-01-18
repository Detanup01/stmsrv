using Newtonsoft.Json;
using Steam3Kit.Types;
using Steam3Kit.Utils;
using Steam3Server;
using Steam3Server.Others;
using Steam3Server.SQL;
using System;
using System.IO.Compression;
using System.Net;
using System.Text;
using ValveKeyValue;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
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
            var buf = File.ReadAllBytes("480.appinfo.bin");
            using var mem_out = new MemoryStream();
            var gz = new ValveAppInfo_GZ(mem_out);
            gz.Write(buf);
            gz.Close();
            File.WriteAllBytes("6E05A57678FD3EA9FD7DCC76DA6F6356BE1787C3.txt.gz", mem_out.ToArray());





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
            ServerCore.Start();
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
            Console.ReadLine();

        }
    }
}