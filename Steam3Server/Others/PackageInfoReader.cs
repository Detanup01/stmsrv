using Steam3Kit;
using Steam3Server.SQL;
using ValveKeyValue;
using System.Diagnostics;

namespace Steam3Server.Others
{
    public class PackageInfoReader
    {
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
            do
            {
                var subid = reader.ReadUInt32();

                if (subid == 0xFFFFFFFF)
                {
                    break;
                }
                
                if (subid != 0)
                    break;
                
                var package = new JPackage
                {
                    SubID = subid,
                    Hash = reader.ReadBytes(20),
                    ChangeNumber = reader.ReadUInt32(),
                };

                if (magic != Magic27)
                {
                    package.Token = reader.ReadUInt64();
                }

                var Data = deserializer.Deserialize(input);
                MemoryStream ms = new();
                ms = new();
                deserializer.Serialize(ms, Data);             
                Console.WriteLine(ms.Length);
                package.DataBytes = ms.ToArray();
                ms.Dispose();
                DBPackageInfo.AddPackages(package);
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
