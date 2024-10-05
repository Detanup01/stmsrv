using DB4Steam;
using System.Security.Cryptography;
using System.Text;
using ValveKeyValue;

namespace PICS_Backend;

public static class VDFHelper
{
    public static StringTable stringTable = new();

    public static byte[] GetAppInfoStringData(this JApp jApp)
    {
        var serializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
        var data_text = DataFromKVBinToKVText_StringTable(jApp.DataByte);
        string des_string = Encoding.UTF8.GetString(data_text);
        des_string = des_string.Replace("\"\t\"", "\"\t\t\"");
        return Encoding.UTF8.GetBytes(des_string);
    }

    public static byte[] DataFromKVTextToKVBin(this byte[] bytes)
    {
        var binary = KVSerializer.Create(KVSerializationFormat.KeyValues1Binary);
        var text = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
        var kv = text.Deserialize(new MemoryStream(bytes));
        using MemoryStream ms = new();
        binary.Serialize(ms, kv);
        return ms.ToArray();
    }


    public static byte[] DataFromKVBinToKVText(this byte[] bytes)
    {
        var binary = KVSerializer.Create(KVSerializationFormat.KeyValues1Binary);
        var text = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
        var kv = binary.Deserialize(new MemoryStream(bytes));
        using MemoryStream ms = new();
        text.Serialize(ms, kv);
        return ms.ToArray();
    }

    public static byte[] DataFromKVBinToKVText_StringTable(this byte[] bytes)
    {
        var binary = KVSerializer.Create(KVSerializationFormat.KeyValues1Binary);
        var text = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
        var kv = binary.Deserialize(new MemoryStream(bytes), new KVSerializerOptions() { StringTable = stringTable });
        using MemoryStream ms = new();
        text.Serialize(ms, kv);
        return ms.ToArray();
    }

    public static byte[] ParseAppInfoGZ(byte[] appinfoBytes, int compress = -1)
    {
        using var mem_out = new MemoryStream();
        var gz = new ValveAppInfo_GZ(mem_out, compress);
        gz.Write(appinfoBytes, 0, appinfoBytes.Length);
        gz.Close();
        return mem_out.ToArray();
    }

    internal static int ReadVDF_File(string vdf_File, string replace_this, out uint Id, out byte[] sha_binhash, out byte[] sha_texthash, out byte[] bin_bytes, out byte[] text_bytes)
    {
        Id = 0;
        sha_binhash = [];
        sha_texthash = [];
        bin_bytes = [];
        text_bytes = [];
        var last_write = File.GetLastWriteTime(vdf_File);
        var vdf_name = vdf_File.Replace(".vdf", "").Replace(replace_this, "");
        var vdfBytes = File.ReadAllBytes(vdf_File);
        if (!uint.TryParse(vdf_name, out uint uint_res))
            return 0;
        Id = uint_res;
        // binary
        var sha1 = SHA1.Create();
        if (vdfBytes[0] == 0x00)
        {
            bin_bytes = vdfBytes;
        }
        else
        {
            string des_string = Encoding.UTF8.GetString(vdfBytes);
            des_string = des_string.Replace("\"\t\"", "\"\t\t\"");
            text_bytes = Encoding.UTF8.GetBytes(des_string);
        }
        if (bin_bytes.Length == 0)
        {
            string des_string = Encoding.UTF8.GetString(vdfBytes);
            des_string = des_string.Replace("\"\t\t\"", "\"\t\"");
            bin_bytes = Encoding.UTF8.GetBytes(des_string).DataFromKVTextToKVBin();
        }
        else if (text_bytes.Length == 0)
        {
            string des_string = Encoding.UTF8.GetString(vdfBytes.DataFromKVBinToKVText());
            des_string = des_string.Replace("\"\t\"", "\"\t\t\"");
            text_bytes = Encoding.UTF8.GetBytes(des_string);
        }
        sha_binhash = sha1.ComputeHash(bin_bytes);
        sha_texthash = sha1.ComputeHash(text_bytes);
        return 1;
    }
}
