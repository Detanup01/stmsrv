using DB4Steam;
using Newtonsoft.Json;
using System.Text;

namespace PICS_Backend;

internal class PackageInfoExtra
{
    static List<uint> PackageAddons = [];

    public static void ReadAll()
    {
        if (!Directory.Exists("PackageInfo"))
            return;

        /*
         * VDF Addon:
         * 0.vdf
         * Can be Binary or Text, Will be converted into Binary data.
         */
        foreach (var vdfFile in Directory.GetFiles("PackageInfo", "*.vdf"))
        {
            ReadVDF_File(vdfFile);
        }
        var packageInfo = DBPackageInfo.GetPackageInfoCache();
        if (packageInfo == null)
            return;
        foreach (var item in PackageAddons)
        {
            if (!packageInfo.Packages.Contains(item))
            {
                packageInfo.Packages.Add(item);
            }
        }

        DBPackageInfo.EditPackageInfoCache(packageInfo);
    }

    static void ReadVDF_File(string vdf_File)
    {
        /*
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

        var package = DBPackageInfo.GetPackage(AppId);
        if (package == null)
        {
            DBPackageInfo.AddPackage(new JPackage()
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
            package.BinaryDataHash = sha_binhash;
            package.ChangeNumber = infoAddon.ChangeNumber;
            package.DataByte = bin_bytes;
            package.Hash = sha_texthash;
            package.LastUpdated = last_write;
            package.Token = infoAddon.Token;
            DBAppInfo.EditApp(package);
        }
        */
    }
}
