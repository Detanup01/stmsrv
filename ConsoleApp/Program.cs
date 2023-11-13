using Google.Protobuf;
using MainLib;
using Newtonsoft.Json;
using Steam.Messages.Base;
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