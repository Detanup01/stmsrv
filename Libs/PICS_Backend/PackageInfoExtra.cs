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
            VDFHelper.ReadVDF_File(vdfFile, "PackageInfo", out uint subId, out byte[] sha_binhash, out byte[] sha_texthash, out byte[] bin_bytes, out byte[] text_bytes);
            var idtoken = Packages.FirstOrDefault(x => x.Id == subId);
            if (idtoken == null)
            {
                idtoken = new();
            }
            CustomPICSVersioning.IndicateChange();
            var latest_pics = CustomPICSVersioning.GetLast();
            var jPackage = DBPackages.GetPackage(subId);
            if (jPackage == null)
            {
                DBPackages.AddPackage(new JPackage()
                {
                    SubID = subId,
                    ChangeNumber = latest_pics.changeid,
                    DataBytes = bin_bytes,
                    Hash = sha_texthash,
                    Token = idtoken.Token
                });
            }
            else
            {
                jPackage.ChangeNumber = latest_pics.changeid;
                jPackage.DataBytes = bin_bytes;
                jPackage.Hash = sha_texthash;
                jPackage.Token = idtoken.Token;
                DBPackages.EditPackage(jPackage);
            }

        }
    }
}
