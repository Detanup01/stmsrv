using Steam3Kit.Types;

namespace Steam3Server.Others
{
    public static class VDFParserExt
    {
        public static byte[] ParseAppInfoGZ(byte[] appinfoBytes, int compress = -1)
        {
            using var mem_out = new MemoryStream();
            var gz = new ValveAppInfo_GZ(mem_out, compress);
            gz.Write(appinfoBytes, 0, appinfoBytes.Length);
            gz.Close();
            return mem_out.ToArray();
        }
    }
}
