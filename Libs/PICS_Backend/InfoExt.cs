using DB4Steam;
using System.Text;
using ValveKeyValue;

namespace PICS_Backend;

public static class InfoExt
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
}
