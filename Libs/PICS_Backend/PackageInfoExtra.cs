using DB4Steam;
using Newtonsoft.Json;

namespace PICS_Backend;

public class PackageInfoExtra
{
    static List<IdAndToken> Packages = [];

    public static void ReadAll()
    {
        if (!Directory.Exists("PackageInfo"))
            return;

        List<IdAndToken>? idAndTokens = JsonConvert.DeserializeObject<List<IdAndToken>>(File.ReadAllText(Path.Combine("PackageInfo", "Packages.json")));
        if (idAndTokens != null)
            Packages = idAndTokens;
        /*
         * VDF Addon:
         * 0.vdf
         * Can be Binary or Text, Will be converted into Binary data.
         */
        foreach (var vdfFile in Directory.GetFiles("PackageInfo", "*.vdf"))
        {
            int ret = VDFHelper.ReadVDF_File(vdfFile, "PackageInfo", out uint subId, out byte[] sha_binhash, out byte[] sha_texthash, out byte[] bin_bytes, out byte[] text_bytes);
            if (ret == 0)
                continue;
            IdAndToken? idtoken = Packages.FirstOrDefault(x => x.Id == subId);
            if (idtoken == null)
            {
                idtoken = new();
            }
            JPackage? jPackage = DBPackages.GetPackage(subId);
            if (jPackage != null && jPackage.Hash == sha_texthash)
                continue;
            CustomPICSVersioning.IndicateChange();
            var (changeid, time) = CustomPICSVersioning.GetLast();
            if (jPackage == null)
            {
                DBPackages.AddPackage(new()
                {
                    SubID = subId,
                    ChangeNumber = changeid,
                    DataBytes = bin_bytes,
                    Hash = sha_texthash,
                    Token = idtoken.Token
                });
                continue;
            }
            else
            {
                jPackage.ChangeNumber = changeid;
                jPackage.DataBytes = bin_bytes;
                jPackage.Hash = sha_texthash;
                jPackage.Token = idtoken.Token;
                DBPackages.EditPackage(jPackage);
            }

        }
    }
}
