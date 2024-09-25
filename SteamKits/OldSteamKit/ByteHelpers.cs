using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLib
{
    public static class ByteHelpers
    {
        public static byte[] Padding(this byte[] data, int padLength)    
        {
            var newArray = new byte[padLength];

            var startAt = newArray.Length - data.Length;
            Buffer.BlockCopy(data, 0, newArray, startAt, data.Length);
            return newArray;
        }
    }
}
