using System.IO.Compression;
using System.IO.Hashing;

namespace Steam3Kit.Utils
{
    static class ZipUtil
    {
        public static int Decompress(MemoryStream ms, byte[] destination, bool verifyChecksum = true)
        {
            using var zip = new ZipArchive(ms);
            var entries = zip.Entries;

            //DebugLog.Assert(entries.Count == 1, nameof(ZipUtil), "Expected the zip to contain only one file");

            var entry = entries[0];
            var sizeDecompressed = (int)entry.Length;

            if (destination.Length < sizeDecompressed)
            {
                throw new ArgumentException("The destination buffer is smaller than the decompressed data size.", nameof(destination));
            }

            using var entryStream = entry.Open();

            entryStream.ReadExactly(destination, 0, sizeDecompressed);

            if (verifyChecksum && Crc32.HashToUInt32(destination.AsSpan()[..sizeDecompressed]) != entry.Crc32)
            {
                throw new Exception("Checksum validation failed for decompressed file");
            }

            return sizeDecompressed;
        }

        public static byte[] Compress(string filename, byte[] input)
        {
            using MemoryStream ms = new();
            using var zip = new ZipArchive(ms, ZipArchiveMode.Create);

            var entry = zip.CreateEntry(filename);
            using var entryStream = entry.Open();
            entryStream.Write(input, 0, input.Length);
            entryStream.Flush();
            entryStream.Dispose();
            return ms.ToArray();
        }
    }
}