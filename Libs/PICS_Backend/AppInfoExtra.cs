using DB4Steam;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace PICS_Backend;

public class AppInfoExtra
{
    class AppInfoAddon
    {
        public uint AppID { get; set; }
        public ulong Token { get; set; }
        public uint ChangeNumber { get; set; }
    }

    static Dictionary<uint, AppInfoAddon> Addons = [];

    public static void ReadAll()
    {
        if (!Directory.Exists("AppInfo"))
            return;

        /*
         * File Addon:
         * 480.json       -   Add or Override AppId 480 to the Database
         * Also PLEASE do not add multiple of that Json :)
         * All files should NOT contain the VDF data, only these as JSON!:
         * AppID, InfoState, Token, ChangeNumber
         */

        foreach (var jsonAddonFile in Directory.GetFiles("AppInfo", "*.json"))
        {
            var Addon = JsonConvert.DeserializeObject<AppInfoAddon>(File.ReadAllText(jsonAddonFile));
            Addons.Add(Addon.AppID, Addon);
        }
        /*
         * VDF Addon:
         * 480.vdf
         * Can be Binary or Text, Will be converted into Binary data.
         */
        foreach (var vdfFile in Directory.GetFiles("AppInfo", "*.vdf"))
        {
            ReadVDF_File(vdfFile);
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

    static void ReadVDF_File(string vdf_File)
    {
        var last_write = File.GetLastWriteTime(vdf_File);
        var vdf_name = vdf_File.Replace(".vdf", "").Replace("AppInfo", "");
        Console.WriteLine(vdf_name);
        var vdfBytes = File.ReadAllBytes(vdf_File);
        var AppId = uint.Parse(vdf_name);
        if (!Addons.TryGetValue(AppId, out var infoAddon))
            return;
        // binary
        var sha1 = SHA1.Create();
        byte[] sha_binhash = [];
        byte[] sha_texthash = [];
        byte[] bin_bytes = [];
        byte[] textbytes = [];
        if (vdfBytes[0] == 0x00)
        {
            bin_bytes = vdfBytes;
        }
        else
        {
            string des_string = Encoding.UTF8.GetString(vdfBytes);
            des_string = des_string.Replace("\"\t\"", "\"\t\t\"");
            textbytes = Encoding.UTF8.GetBytes(des_string);
        }
        if (bin_bytes.Length == 0)
        {
            string des_string = Encoding.UTF8.GetString(vdfBytes);
            des_string = des_string.Replace("\"\t\t\"", "\"\t\"");
            bin_bytes = Encoding.UTF8.GetBytes(des_string).DataFromKVTextToKVBin();
        }
        else if (textbytes.Length == 0)
        {
            string des_string = Encoding.UTF8.GetString(vdfBytes.DataFromKVBinToKVText());
            des_string = des_string.Replace("\"\t\"", "\"\t\t\"");
            textbytes = Encoding.UTF8.GetBytes(des_string);
        }
        sha_binhash = sha1.ComputeHash(bin_bytes);
        sha_texthash = sha1.ComputeHash(textbytes);

        var japp = DBAppInfo.GetApp(AppId);
        if (japp == null)
        {
            DBAppInfo.AddApp(new JApp()
            {
                AppID = AppId,
                BinaryDataHash = sha_binhash,
                ChangeNumber = infoAddon.ChangeNumber,
                DataByte = bin_bytes,
                Hash = sha_texthash,
                LastUpdated = last_write,
                Token = infoAddon.Token
            });
        }
        else
        {
            japp.BinaryDataHash = sha_binhash;
            japp.ChangeNumber = infoAddon.ChangeNumber;
            japp.DataByte = bin_bytes;
            japp.Hash = sha_texthash;
            japp.LastUpdated = last_write;
            japp.Token = infoAddon.Token;
            DBAppInfo.EditApp(japp);
        }
    }

}
