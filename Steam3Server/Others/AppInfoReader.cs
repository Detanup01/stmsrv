using Steam3Kit;
using Steam3Server.SQL;
using System.Diagnostics;
using ValveKeyValue;

namespace Steam3Server.Others
{
    public class AppInfoReader
    {
        //  Reason to limit:    AppInfo vdf is big, and we have a small db. Import AppIds that only we want in first place, then we can expand it later. AKA Extras or something.
        private static List<uint> AppListToGet = new List<uint>()
                {
                    7, 8, 92, 211, 215, 218, 440, 480, 564, 570, 575, 630, 640, 760, 761, 764, 765, 766, 767, 1007, 1840, 42300, 42320, 61800, 61810, 61820, 61830, 202351, 202352, 202355, 203300, 218800, 221410, 223910, 228980, 241100, 243730, 243750, 244310, 248210, 250820, 261310, 313250, 321770, 366490, 373300, 401530, 405270, 407350, 413080, 413090, 413100, 424690, 443510, 476580, 551410, 588460, 613220, 733580, 736120, 744350, 807210, 858280, 875860, 891390, 961940, 1016370, 1054830, 1070560, 1070910, 1113280, 1161040, 1182480, 1235260, 1245040, 1391110, 1420170, 1456390, 1493710, 1580130, 1628350, 1635560, 1716750, 1826330, 1877380, 1887720, 1899670, 1977700, 2053530, 2180100, 2230260, 2348590, 2371090
                };
        private const uint Magic28 = 0x07_56_44_28;
        private const uint Magic = 0x07_56_44_27;
        /// <summary>
        /// Opens and reads the given filename.
        /// </summary>
        /// <param name="filename">The file to open and read.</param>
        public static void Read(string filename)
        {
            using var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            Read(fs);
        }

        /// <summary>
        /// Reads the given <see cref="Stream"/>.
        /// </summary>
        /// <param name="input">The input <see cref="Stream"/> to read from.</param>
        public static void Read(Stream input)
        {
            var sp = Stopwatch.StartNew();

            using var reader = new BinaryReader(input);
            var magic = reader.ReadUInt32();
            if (magic != Magic && magic != Magic28)
            {
                throw new InvalidDataException($"Unknown magic header: {magic:X}");
            }
            var Universe = (EUniverse)reader.ReadUInt32();

            List<uint> Apps = new();

            var deserializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Binary);
            do
            {
                var appid = reader.ReadUInt32();

                if (appid == 0)
                {
                    break;
                }

                if (AppListToGet.Contains(appid))
                {
                    var size = reader.ReadUInt32();
                    var app = new JApp
                    {
                        AppID = appid,
                        InfoState = reader.ReadUInt32(),
                        LastUpdated = DateTimeFromUnixTime(reader.ReadUInt32()),
                        Token = reader.ReadUInt64(),
                        Hash = reader.ReadBytes(20),
                        ChangeNumber = reader.ReadUInt32(),
                    };
                    Apps.Add(appid);
                    if (magic == Magic28)
                    {
                        app.BinaryDataHash = reader.ReadBytes(20);
                    }

                    var kvData = deserializer.Deserialize(input);
                    MemoryStream ms = new();
                    ms = new();
                    deserializer.Serialize(ms, kvData);
                    ms.Dispose();
                    app.DataByte = ms.ToArray();
                    DBAppInfo.AddApp(app);
                }
                else
                {
                    //Skipping it. Can be made smaller to just read X bytes (might do it later)
                    //  4 + 4 + 4 + 8 + 20 + 4 + (20)
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                    reader.ReadUInt64();
                    reader.ReadBytes(20);
                    reader.ReadUInt32();
                    if (magic == Magic28)
                    {
                        reader.ReadBytes(20);
                    }
                }

            } while (true);


            DBAppInfo.AddAppInfoCache(new JAppInfo()
            {
                Id = 1,
                Apps = Apps,
                Magic = magic,
                Universe = Universe
            }); 
            sp.Stop();
            UtilsLib.Debug.PWDebug("Elapsed time for AppInfo: " + sp.ElapsedMilliseconds + " ms");
        }

        public static DateTime DateTimeFromUnixTime(uint unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime);
        }
    }
}
