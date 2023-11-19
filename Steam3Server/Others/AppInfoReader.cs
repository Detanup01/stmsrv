using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Steam3Kit;
using Steam3Server.SQL;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using ValveKeyValue;

namespace Steam3Server.Others
{
    public class AppInfoReader
    {
        public static AppInfoNode App7Node;
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
                var size = reader.ReadUInt32();
                if (AppListToGet.Contains(appid))
                {
                    //UtilsLib.Debug.PWDebug("AppId in list, getting it: " + appid);
                    var app = new JApp
                    {
                        AppID = appid,
                        InfoState = reader.ReadUInt32(),
                        LastUpdated = DateTimeFromUnixTime(reader.ReadUInt32()),
                        Token = reader.ReadUInt64(),
                        Hash = reader.ReadBytes(20),
                        ChangeNumber = reader.ReadUInt32(),
                    };
                    if (magic == Magic28)
                    {
                        app.BinaryDataHash = reader.ReadBytes(20);
                    }
                    Apps.Add(appid);

                    app.DataByte = reader.ReadBytes(((int)size-4-4-8-20-4-20));
                    /*
                    var x = AppInfoNodeExt.ReadEntries(reader);
                    if (app.AppID == 480)
                        App7Node = x;
                    var test = AppInfoNodeKV.ParseToBin(x);
                    var vdf = AppInfoNodeKV.ParseToVDF(x);
                    Console.WriteLine(vdf);
                    app.DataByte = test;//Encoding.UTF8.GetBytes(vdf);
                    //app.DataByte = ReadEntriesBin(reader);
                    */
                    DBAppInfo.AddApp(app);
                }
                else
                {
                    reader.BaseStream.Seek(size, SeekOrigin.Current);
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
       

        static byte[] ReadEntriesBin(BinaryReader _binaryReader)
        {
            List<byte> bytes = new();
            while (true)
            {
                byte type = _binaryReader.ReadByte();
                bytes.Add(type);
                if (type == 0x08)
                {
                    break;
                }

                bytes.AddRange(ReadStringBin(_binaryReader));

                switch (type)
                {
                    case 0x00:
                        bytes.AddRange(ReadEntriesBin(_binaryReader));

                        break;
                    case 0x01:
                        bytes.AddRange(ReadStringBin(_binaryReader));

                        break;
                    case 0x02:
                        bytes.AddRange(_binaryReader.ReadBytes(4));
                        break;
                    default:

                        throw new ArgumentOutOfRangeException(string.Format(CultureInfo.InvariantCulture, "Unknown entry type '{0}'", type));
                }
            }
            return bytes.ToArray();
        }

        static byte[] ReadStringBin(BinaryReader _binaryReader)
        {
            List<byte> bytes = new List<byte>();

            try
            {
                bool stringDone = false;
                do
                {
                    byte b = _binaryReader.ReadByte();
                    bytes.Add(b);
                    if (b == 0)
                    {
                        stringDone = true;
                    }
                } while (!stringDone);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
            return bytes.ToArray();
        }
    }
}
