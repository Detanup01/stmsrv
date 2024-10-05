using DB4Steam;
using Newtonsoft.Json;

namespace PICS_Backend;

public class AppInfoExtra
{

    static List<IdAndToken> Apps = [];

    public static void ReadAll()
    {
        if (!Directory.Exists("AppInfo"))
            return;

        List<IdAndToken>? idAndTokens = JsonConvert.DeserializeObject<List<IdAndToken>>(File.ReadAllText(Path.Combine("AppInfo", "Apps.json")));
        if (idAndTokens != null)
            Apps = idAndTokens;
        /*
         * VDF Addon:
         * 480.vdf
         * Can be Binary or Text, Will be converted into Binary data.
         */
        foreach (var vdfFile in Directory.GetFiles("AppInfo", "*.vdf"))
        {
            VDFHelper.ReadVDF_File(vdfFile, "AppInfo", out uint appId, out byte[] sha_binhash, out byte[] sha_texthash, out byte[] bin_bytes, out byte[] text_bytes);
            var idtoken = Apps.FirstOrDefault(x=>x.Id == appId);
            if (idtoken == null)
            {
                idtoken = new();
            }
            CustomPICSVersioning.IndicateChange();
            var latest_pics = CustomPICSVersioning.GetLast();
            var japp = DBApp.GetApp(appId);
            if (japp == null)
            {
                DBApp.AddApp(new JApp()
                {
                    AppID = appId,
                    BinaryDataHash = sha_binhash,
                    ChangeNumber = latest_pics.changeid,
                    DataByte = bin_bytes,
                    Hash = sha_texthash,
                    LastUpdated = latest_pics.time,
                    Token = idtoken.Token
                });
            }
            else
            {
                japp.BinaryDataHash = sha_binhash;
                japp.ChangeNumber = latest_pics.changeid;
                japp.DataByte = bin_bytes;
                japp.Hash = sha_texthash;
                japp.LastUpdated = latest_pics.time;
                japp.Token = idtoken.Token;
                DBApp.EditApp(japp);
            }

        }
    }
}
