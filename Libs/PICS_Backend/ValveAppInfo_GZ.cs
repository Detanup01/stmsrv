using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace PICS_Backend;

public class ValveAppInfo_GZ : DeflaterOutputStream
{
    /*
        Thanks for steamCooker helping the GZIP Stream
        This is using his code with in C#
     */

    const ushort GZIP_MAGIC = 35615;
    const byte GZIP_COMPRESSIONMETHOD_DEFLATE = 8;
    const Int32 TimeStamp = 0;
    Crc32 Crc32;
    int Len;
    bool IsFinished;
    public ValveAppInfo_GZ(Stream baseOutputStream) : base(baseOutputStream, new Deflater(Deflater.DEFAULT_COMPRESSION, true))
    {
        IsFinished = false;
        Crc32 = new();
        Len = 0;
        WriteHeader();
    }

    public ValveAppInfo_GZ(Stream baseOutputStream, int level) : base(baseOutputStream, new Deflater(level, true))
    {
        IsFinished = false;
        Crc32 = new();
        Len = 0;
        WriteHeader();
    }

    public void WriteHeader()
    {
        Crc32.Reset();
        baseOutputStream_.Write(BitConverter.GetBytes(GZIP_MAGIC));
        baseOutputStream_.WriteByte(GZIP_COMPRESSIONMETHOD_DEFLATE);
        baseOutputStream_.WriteByte(0); //should be flags
        baseOutputStream_.Write(BitConverter.GetBytes(TimeStamp)); //timestamp (always 0)
        baseOutputStream_.WriteByte(0); //compression level
        baseOutputStream_.WriteByte(255); //ZipOS (255/Unknown)
        baseOutputStream_.Flush();
    }

    public void WriteTrailer()
    {
        baseOutputStream_.Write(BitConverter.GetBytes((int)Crc32.Value));
        baseOutputStream_.Write(BitConverter.GetBytes(Len));
        baseOutputStream_.Flush();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        base.Write(buffer, offset, count);
        Crc32.Update(buffer.ToArray());
        Len = buffer.Length;
    }
    public override void Finish()
    {
        if (IsFinished)
            return;
        base.Finish();
        WriteTrailer();
        IsFinished = true;
    }

    public override void Close()
    {
        Finish();
        base.Close();
    }
}
