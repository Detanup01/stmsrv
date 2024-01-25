using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Steam3Server.Others
{
    public static class VDFParserExt
    {
        public static byte[] ParseAppInfoGZ(byte[] appinfoBytes, int compress = -1)
        {
            /*
            var buf = Encoding.UTF8.GetBytes(AppInfoNodeExt.ReadEntries(appinfoBytes).ParseToVDF() + "\n")[1..];
            var buf_list = buf.ToList();
            buf_list.Add(0x00);
            buf = buf_list.ToArray();*/
            var buf = ParseAppInfo(appinfoBytes);
            using var mem_out = new MemoryStream();
            var gz = new ValveAppInfo_GZ(mem_out, compress);
            gz.Write(buf, 0, buf.Length);
            gz.Close();
            return mem_out.ToArray();
        }

        public static byte[] ParseAppInfo(byte[] appinfoBytes)
        {
            var buf = Encoding.UTF8.GetBytes(AppInfoNodeExt.ReadEntries(appinfoBytes).ParseToVDF() + "\n")[1..];
            return buf;
        }
    }
}
