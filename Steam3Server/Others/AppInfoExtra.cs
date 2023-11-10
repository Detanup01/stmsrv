using Newtonsoft.Json;
using Steam3Server.SQL;
using System.Security.Cryptography;
using ValveKeyValue;

namespace Steam3Server.Others
{
    public class AppInfoExtra
    {
        class AppInfoAddon
        {
            public uint AppID { get; set; }
            public uint InfoState { get; set; }
            public DateTime LastUpdated { get; set; }
            public ulong Token { get; set; }
            public string BinaryDataHash { get; set; }
            public uint ChangeNumber { get; set; }
            public uint VDFFormat { get; set; }
        }

        public static void ReadAll()
        {
            if (!Directory.Exists("Addons/AppInfo"))
                return;
            
            Dictionary<uint, AppInfoAddon> Addons = new();
            /*
             * File Addon:
             * 480.json       -   Add or Override AppId 480 to the Database
             * Also PLEASE do not add multiple of that Json :)
             * All files should NOT contain the VDF data, only these as JSON!:
             * AppID, InfoState, LastUpdated (DATETIME), Token, ChangeNumber, BinaryDataHash, VDFFormat (0=>Text,1=Binary)
             * BinaryDataHash must be SHA1 of the added 480.vdf as parsed to BINARY data format. And Base64'd
             */

            foreach (var jsonAddonFile in Directory.GetFiles("Addons/AppInfo", "*.json"))
            {
                var Addon = JsonConvert.DeserializeObject<AppInfoAddon>(File.ReadAllText(jsonAddonFile));
                Addons.Add(Addon.AppID, Addon);
            }
            /*
             * VDF Addon:
             * 480.vdf
             * Can be Binary or Text, Will be converted into Binary data.
             */
            foreach (var vdfFile in Directory.GetFiles("Addons/AppInfo", "*.vdf"))
            {
                var vdf_name = vdfFile.Replace(".vdf","").Replace("Addons/AppInfo","");
                Console.WriteLine(vdf_name);
                var vdfBytes = File.ReadAllBytes(vdfFile);
                var AppId = uint.Parse(vdf_name);
                if (Addons.TryGetValue(AppId, out var infoAddon))
                {
                    MemoryStream mem = new(vdfBytes);
                    if (infoAddon.VDFFormat == 0)
                    {
                        try
                        {
                            var deserializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Binary);
                            var kvObject = deserializer.Deserialize(mem);
                            MemoryStream ms = new();
                            deserializer.Serialize(ms, kvObject);
                            var japp = DBAppInfo.GetApp(AppId);
                            var databytes = mem.ToArray();
                            var sha1 = SHA1.Create();

                            if (japp == null)
                            {
                                DBAppInfo.AddApp(new JApp()
                                {
                                    AppID = AppId,
                                    InfoState = infoAddon.InfoState,
                                    BinaryDataHash = Convert.FromBase64String(infoAddon.BinaryDataHash),
                                    ChangeNumber = infoAddon.ChangeNumber,
                                    DataByte = databytes,
                                    Hash = sha1.ComputeHash(databytes),
                                    LastUpdated = infoAddon.LastUpdated,
                                    Token = infoAddon.Token
                                });
                            }
                            else
                            {
                                japp.InfoState = infoAddon.InfoState;
                                japp.BinaryDataHash = Convert.FromBase64String(infoAddon.BinaryDataHash);
                                japp.ChangeNumber = infoAddon.ChangeNumber;
                                japp.DataByte = databytes;
                                japp.Hash = sha1.ComputeHash(databytes);
                                japp.LastUpdated = infoAddon.LastUpdated;
                                japp.Token = infoAddon.Token;
                                DBAppInfo.EditApp(japp);
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Exception! Not a valid Binary VDF!");
                        }
                    }
                    else if (infoAddon.VDFFormat == 1)
                    {
                        
                        try
                        {
                            var deserializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
                            var kvObject = deserializer.Deserialize(mem);
                            deserializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Binary);
                            MemoryStream ms = new();
                            deserializer.Serialize(ms, kvObject);
                            var japp = DBAppInfo.GetApp(AppId);
                            var databytes = mem.ToArray();
                            var sha1 = SHA1.Create();

                            if (japp == null)
                            {
                                DBAppInfo.AddApp(new JApp()
                                {
                                    AppID = AppId,
                                    InfoState = infoAddon.InfoState,
                                    BinaryDataHash = Convert.FromBase64String(infoAddon.BinaryDataHash),
                                    ChangeNumber = infoAddon.ChangeNumber,
                                    DataByte = databytes,
                                    Hash = sha1.ComputeHash(databytes),
                                    LastUpdated = infoAddon.LastUpdated,
                                    Token = infoAddon.Token
                                });
                            }
                            else
                            {
                                japp.InfoState = infoAddon.InfoState;
                                japp.BinaryDataHash = Convert.FromBase64String(infoAddon.BinaryDataHash);
                                japp.ChangeNumber = infoAddon.ChangeNumber;
                                japp.DataByte = databytes;
                                japp.Hash = sha1.ComputeHash(databytes);
                                japp.LastUpdated = infoAddon.LastUpdated;
                                japp.Token = infoAddon.Token;
                                DBAppInfo.EditApp(japp);
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Exception! Not a valid TEXT VDF!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("??? What format are you tring to parse?");
                    }
                
                }
            }
            var appinfo = DBAppInfo.GetAppInfoCache();
            foreach (var item in Addons.Keys)
            {
                if (!appinfo.Apps.Contains(item))
                {
                    appinfo.Apps.Add(item);
                }
            }

            DBAppInfo.EditAppInfoCache(appinfo);
        }
    }
}
