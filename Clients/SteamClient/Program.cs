﻿namespace SteamClientApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var client = new SClient();
            Console.ReadLine();
            client.Client.Disconnect();
        }
    }
}