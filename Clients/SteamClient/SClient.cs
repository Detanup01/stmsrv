using SteamKit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamClientApp
{
    internal class SClient
    {
        private readonly object steamLock = new();
        public SteamClient Client;
        SteamUser User;
        SteamApps Apps;
        CallbackManager CallbackManager;
        bool Aborted;
        public SClient()
        {
            Client = new SteamClient();
            User = Client.GetHandler<SteamUser>()!;
            Apps = Client.GetHandler<SteamApps>()!;
            CallbackManager = new CallbackManager(Client);
            CallbackManager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            CallbackManager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);

            CallbackManager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            
            CallbackManager.Subscribe<SteamApps.PICSTokensCallback>(PICS_Tokens);
            CallbackManager.Subscribe<SteamApps.PICSProductInfoCallback>(PICS_ProductInfo);
            CallbackManager.Subscribe<SteamApps.PICSChangesCallback>(PICS_Changes);

            Thread thread = new(CallbackCaller);
            thread.Start();
            Client.Connect();
        }

        List<uint> PackageTokensReq = [];
        List<uint> AppTokensReq = [];

        Dictionary<uint, ulong> PackageTokens = [];
        Dictionary<uint, ulong> AppTokens = [];

        List<uint> PackageIds = [];
        List<uint> AppIds = [];

        private void PICS_Changes(SteamApps.PICSChangesCallback callback)
        {
            Console.WriteLine("PICS_Changes! " + callback.LastChangeNumber + " " + callback.CurrentChangeNumber + " " + callback.PackageChanges.Count + " " + callback.AppChanges.Count);
            foreach (var item in callback.PackageChanges)
            {
                if (item.Value.NeedsToken)
                    PackageTokensReq.Add(item.Key);
                else
                    PackageIds.Add(item.Key);
            }

            foreach (var item in callback.AppChanges)
            {
                if (item.Value.NeedsToken)
                    AppTokensReq.Add(item.Key);
                else
                    AppIds.Add(item.Key);
            }
            Console.WriteLine($"Need token for Packages: {PackageTokensReq.Count} and Apps: {AppTokensReq.Count} ");
            Apps.PICSGetAccessTokens(AppTokensReq, PackageTokensReq);
        }

        private void PICS_ProductInfo(SteamApps.PICSProductInfoCallback callback)
        {
            Console.WriteLine("PICS_ProductInfo");
            foreach (var item in callback.Packages)
            {
                if (item.Value.MissingToken)
                    continue;
                if (item.Value.KeyValues == KeyValue.Invalid)
                    continue;
                item.Value.KeyValues.SaveToFile($"pics/{item.Key}_pkg.txt", false);
                PICS_PKG(item.Value.KeyValues);
            }
            foreach (var item in callback.Apps)
            {
                if (item.Value.MissingToken)
                    continue;
                if (item.Value.KeyValues == KeyValue.Invalid)
                    continue;
                item.Value.KeyValues.SaveToFile($"pics/{item.Key}_apps_bytes.txt", true);
            }
        }

        void PICS_PKG(KeyValue kv)
        {
            var appids = kv["appids"];
            foreach (var child in appids.Children)
            {
                var chint = child.AsUnsignedInteger();
                Apps.PICSGetProductInfo(new(chint), new SteamApps.PICSRequest());
            }
        }

        private void PICS_Tokens(SteamApps.PICSTokensCallback callback)
        {
            Console.WriteLine("PICS_Tokens! ");
            AppTokens = callback.AppTokens;
            PackageTokens = callback.PackageTokens;
            List<SteamApps.PICSRequest> AppsPics = [];
            AppIds.ForEach(item => { AppsPics.Add(new(item)); });
            foreach (var item in AppTokens)
            {
                AppsPics.Add(new(item.Key, item.Value));
            }

            List<SteamApps.PICSRequest> Pkgs = [];
            PackageIds.ForEach(item => { Pkgs.Add(new(item)); });
            foreach (var item in PackageTokens)
            {
                Pkgs.Add(new(item.Key, item.Value));
            }
            Console.WriteLine($"{AppsPics.Count} | {Pkgs.Count}");
            Apps.PICSGetProductInfo(AppsPics, Pkgs);
        }

        private void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {
            Console.WriteLine("logged on!");
            //Apps.PICSGetChangesSince(25332439, true ,true);
            Apps.PICSGetProductInfo(new SteamApps.PICSRequest(), new SteamApps.PICSRequest(17906,0), false);
        }

        private void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            Aborted = true;
        }

        private void OnConnected(SteamClient.ConnectedCallback callback)
        {
            User.LogOnAnonymous();
        }

        public void CallbackCaller()
        {
            while (!Aborted)
            {
                lock (steamLock)
                {
                    CallbackManager.RunWaitAllCallbacks(TimeSpan.FromSeconds(1));
                }
            }
        }

    }
}
