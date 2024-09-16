using Steam3Kit;
using Steam3Kit.Utils;
using Steam3Server.Settings;
using Steam3Server.SQL;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using ValveKeyValue;

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

                options.StringTable = new(stringPool);
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
                    UtilsLib.Debug.PWDebug("AppId in list, getting it: " + appid);
                    var app = new JApp
                    {
                        AppID = appid,
                        InfoState = reader.ReadUInt32(),
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
                   
                    Console.WriteLine("app hash: " + Convert.ToHexString(app.Hash));
                    var kv = deserializer.Deserialize(input, options);
                    using MemoryStream ms2 = new();
                    serializer.Serialize(ms2, kv, options);
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
                    using var mem_out = new MemoryStream();
                    var gz = new ValveAppInfo_GZ(mem_out, -1);
                    gz.Write(app.DataByte, 0, app.DataByte.Length);
                    gz.Close();
                    var appinfogz = mem_out.ToArray();
                    mem_out.Dispose();
                    Console.WriteLine("appinfogz hash: " + Convert.ToHexString(SHA1.HashData(appinfogz)));
                    File.WriteAllBytes($"apps/{appid}_appinfo_compressed.tar.gz", appinfogz);
                    using var mem3 = new MemoryStream();
                    deserializer.Serialize(mem3, kv, options);
                    var deser_arr = mem3.ToArray();
                    File.WriteAllBytes($"apps/{appid}_appinfo_arr", mem3.ToArray());
                    Console.WriteLine("deser_arr hash: " + Convert.ToHexString(SHA1.HashData(deser_arr)));
                    Apps.Add(appid);
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
    }
}
