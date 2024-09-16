using Steam3Kit;
using Steam3Server.SQL;
using ValveKeyValue;
using System.Diagnostics;
using Steam3Kit.Types;
using Steam3Server.Settings;

namespace Steam3Server.Others
{
    public class PackageInfoReader
    {
        static List<uint> EnabledPackages = new () { 0, 17906 };
        private const uint Magic = 0x06_56_55_28;
        private const uint Magic27 = 0x06_56_55_27;
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
            if (magic != Magic && magic != Magic27)
            {
                throw new InvalidDataException($"Unknown magic header: {magic:X}");
            }
            var Universe = (EUniverse)reader.ReadUInt32();

            List<uint> Packages = new();

            var deserializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Binary);
            var serializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);

            do
            {
                uint subid = reader.ReadUInt32();
                if (subid == 0xFFFFFFFF)
                {
                    break;
                }

                JPackage package = new()
                {
                    SubID = subid,
                    Hash = reader.ReadBytes(20),
                    ChangeNumber = reader.ReadUInt32(),
                };

                if (magic != Magic27)
                {
                    package.Token = reader.ReadUInt64();
                }
                var kv = deserializer.Deserialize(input);
                using MemoryStream ms = new();
                deserializer.Serialize(ms, kv);
                package.DataBytes = ms.ToArray();
                ms.Dispose();
                if (MainConfig.Instance().PackageInfoConfig.StopSkipping || MainConfig.Instance().PackageInfoConfig.SkipIds.Contains(subid))
                {
                    UtilsLib.Debug.PWDebug("SubId in list, getting it: " + subid);
                    DBPackageInfo.AddPackage(package);
                    Packages.Add(subid);
                }

            } while (true);


            DBPackageInfo.AddPackageInfoCache(new JPackageInfo()
            {
                Id = 1,
                Packages = Packages,
                Magic = magic,
                Universe = Universe
            });
            sp.Stop();
            UtilsLib.Debug.PWDebug("Elapsed time for PackageInfo: " + sp.ElapsedMilliseconds + " ms");
        }

    }
}
