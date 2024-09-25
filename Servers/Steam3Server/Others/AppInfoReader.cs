using DB4Steam;
using PICS_Backend;
using Steam3Kit;
using Steam3Server.Settings;
using System.Diagnostics;
using System.Security.Cryptography;
using UtilsLib;
using ValveKeyValue;
using Steam3Kit.Utils;
using static Steam3Kit.Utils.StreamHelpers;


namespace Steam3Server.Others
{
    public class AppInfoReader
    {
        private const uint Magic29 = 0x07_56_44_29;
        private const uint Magic28 = 0x07_56_44_28;
        private const uint Magic27 = 0x07_56_44_27;
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
            if (magic != Magic27 && magic != Magic28 && magic != Magic29)
            {
                throw new InvalidDataException($"Unknown magic header: {magic:X} {magic}");
            }
            var Universe = (EUniverse)reader.ReadUInt32();

            List<uint> Apps = new();

            var options = new KVSerializerOptions();

            if (magic == Magic29)
            {
                var stringTableOffset = reader.ReadInt64();
                var offset = reader.BaseStream.Position;
                reader.BaseStream.Position = stringTableOffset;
                var stringCount = reader.ReadUInt32();
                var stringPool = new string[stringCount];

                for (var i = 0; i < stringCount; i++)
                {
                    stringPool[i] = reader.BaseStream.ReadNullTermUtf8String();
                }

                reader.BaseStream.Position = offset;

                for(var i = 0;  i < stringCount; i++)
                {
                    stringPool[i] = stringPool[i].ToLower();
                }

                options.StringTable = new(stringPool);
                InfoExt.stringTable = options.StringTable;
                File.WriteAllLines("_stringpool.txt", stringPool);
                //Console.WriteLine(string.Join(" ", stringPool));
            }

            var deserializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Binary);
            var serializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);

            do
            {
                var appid = reader.ReadUInt32();

                if (appid == 0)
                {
                    break;
                }
                var size = reader.ReadUInt32();
                if (MainConfig.Instance().AppInfoConfig.StopSkipping || MainConfig.Instance().AppInfoConfig.SkipIds.Contains(appid))
                {
                    Logger.PWLog("AppId in list, getting it: " + appid);
                    reader.ReadUInt32(); //infoState
                    var app = new JApp
                    {
                        AppID = appid,
                        LastUpdated = DateTimeFromUnixTime(reader.ReadUInt32()),
                        Token = reader.ReadUInt64(),
                        Hash = reader.ReadBytes(20),
                        ChangeNumber = reader.ReadUInt32(),
                    };
                    if (magic == Magic28 || magic == Magic29)
                    {
                        app.BinaryDataHash = reader.ReadBytes(20);
                        Console.WriteLine("app.BinaryDataHash hash: " + Convert.ToHexString(app.BinaryDataHash));
                    }


                    var kv = deserializer.Deserialize(input, options);

                    /*
                    
                    Console.WriteLine("app hash: " + Convert.ToHexString(app.Hash));
                    using MemoryStream ms2 = new();
                    serializer.Serialize(ms2, kv);
                    app.DataByte = ms2.ToArray();
                    ms2.Dispose();
                    Console.WriteLine("deser hash: " + Convert.ToHexString(SHA1.HashData(app.DataByte)));
                    File.WriteAllBytes($"apps/{appid}.txt", app.DataByte);
                    string des_string = Encoding.UTF8.GetString(app.DataByte);
                    des_string = des_string.Replace("\"\t\"", "\"\t\t\"");
                    Console.WriteLine("toZip beforenull hash: " + Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(des_string))));
                    var toZip = Encoding.UTF8.GetBytes(des_string).Concat(new byte[] { 0x0 }).ToArray();
                    Console.WriteLine("toZip hash: " + Convert.ToHexString(SHA1.HashData(toZip)));
                    File.WriteAllBytes($"apps/{appid}_appinfo_tozip.txt", toZip);
                    var appinfogz = VDFParserExt.ParseAppInfoGZ(app.DataByte);
                    Console.WriteLine("appinfogz hash: " + Convert.ToHexString(SHA1.HashData(appinfogz)));
                    File.WriteAllBytes($"apps/{appid}_appinfo_compressed.tar.gz", appinfogz);
                    using var mem3 = new MemoryStream();
                    deserializer.Serialize(mem3, kv, options);
                    var deser_arr = mem3.ToArray();
                    File.WriteAllBytes($"apps/{appid}_appinfo_arr", mem3.ToArray());
                    Console.WriteLine("deser_arr hash: " + Convert.ToHexString(SHA1.HashData(deser_arr)));
                    */
                    
                    using var mem = new MemoryStream();
                    deserializer.Serialize(mem, kv, options);
                    app.DataByte = mem.ToArray();
                    string should_bindataHash = Convert.ToHexString(SHA1.HashData(app.DataByte));
                    Console.WriteLine("should be same as binarydata hash: " + should_bindataHash);
                    Apps.Add(appid);
                    DBAppInfo.AddApp(app);

                    /*
                    var app2 = DBAppInfo.GetApp(appid);
                    string should_bindataHash = Convert.ToHexString(SHA1.HashData(app2.DataByte));
                    Console.WriteLine("should be same as binarydata hash: " + should_bindataHash);
                    var data = app2.GetAppInfoStringData();;
                    string appHash = Convert.ToHexString(app2.Hash);
                    string should_appHash = Convert.ToHexString(SHA1.HashData(data));
                    Console.WriteLine("app hash: " + appHash);
                    Console.WriteLine("should be same as app hash: " + should_appHash);
                    if (appHash != should_appHash)
                    {

                        Console.WriteLine("plan B: " + Convert.ToHexString(SHA1.HashData(app2.DataByte.DataFromKVBinToKVText())));
                        File.WriteAllBytes($"apps/{appid}_plan_b.txt", app2.DataByte.DataFromKVBinToKVText());
                        Console.WriteLine("plan D: " + Convert.ToHexString(SHA1.HashData(data.DataFromKVTextToKVBin())));
                        File.WriteAllBytes($"apps/{appid}_plan_d.txt", data.DataFromKVTextToKVBin());
                    }

                    */
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
                Universe = Universe,
            }); 
            sp.Stop();
            Logger.PWLog("Elapsed time for AppInfo: " + sp.ElapsedMilliseconds + " ms");
        }

        public static DateTime DateTimeFromUnixTime(uint unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime);
        }
    }
}
