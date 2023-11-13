using Google.Protobuf;
using MainLib;
using Newtonsoft.Json;
using Steam.Messages.Base;
using Steam3Kit.Types;
using Steam3Kit.Utils;
using Steam3Server;
using Steam3Server.Others;
using System.Net;
using System.Security.Cryptography;
using ValveKeyValue;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string x = File.ReadAllText("ticket.txt");
            var bytes_long = Enumerable.Range(0, x.Length / 2)
   .Select(i => Convert.ToByte(x.Substring(i * 2, 2), 16))
   .ToArray();
            var ticketstruct = AppTickets.GetTicket(bytes_long);

            Console.WriteLine(ticketstruct.ToString());
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
            //ServerCore.Start();
            Console.ReadLine();
        }
    }
}