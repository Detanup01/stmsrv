﻿using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Steam3Kit.Utils
{
    public static class StreamHelpers
    {
        [ThreadStatic]
        static byte[]? data;

        [MemberNotNull(nameof(data))]
        static void EnsureInitialized()
        {
            data ??= new byte[8];
        }

        public static Int16 ReadInt16(this Stream stream)
        {
            EnsureInitialized();

            stream.Read(data, 0, 2);
            return BitConverter.ToInt16(data, 0);
        }

        public static UInt16 ReadUInt16(this Stream stream)
        {
            EnsureInitialized();

            stream.Read(data, 0, 2);
            return BitConverter.ToUInt16(data, 0);
        }

        public static Int32 ReadInt32(this Stream stream)
        {
            EnsureInitialized();

            stream.Read(data, 0, 4);
            return BitConverter.ToInt32(data, 0);
        }

        public static Int64 ReadInt64(this Stream stream)
        {
            EnsureInitialized();

            stream.Read(data, 0, 8);
            return BitConverter.ToInt64(data, 0);
        }

        public static UInt32 ReadUInt32(this Stream stream)
        {
            EnsureInitialized();

            stream.Read(data, 0, 4);
            return BitConverter.ToUInt32(data, 0);
        }

        public static UInt64 ReadUInt64(this Stream stream)
        {
            EnsureInitialized();

            stream.Read(data, 0, 8);
            return BitConverter.ToUInt64(data, 0);
        }

        public static float ReadFloat(this Stream stream)
        {
            EnsureInitialized();

            stream.Read(data, 0, 4);
            return BitConverter.ToSingle(data, 0);
        }

        public static string ReadNullTermString(this Stream stream, Encoding encoding)
        {
            int characterSize = encoding.GetByteCount("e");

            using MemoryStream ms = new MemoryStream();

            while (true)
            {
                byte[] data = new byte[characterSize];
                stream.Read(data, 0, characterSize);

                if (encoding.GetString(data, 0, characterSize) == "\0")
                {
                    break;
                }

                ms.Write(data, 0, data.Length);
            }

            return encoding.GetString(ms.GetBuffer(), 0, (int)ms.Length);
        }

        public static void WriteNullTermString(this Stream stream, string value, Encoding encoding)
        {
            var dataLength = encoding.GetByteCount(value);
            var data = new byte[dataLength + 1];
            encoding.GetBytes(value, 0, value.Length, data, 0);
            data[dataLength] = 0x00; // '\0'

            stream.Write(data, 0, data.Length);
        }

        public static string ReadNullTermUtf8String(this Stream stream)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(32);

            try
            {
                var position = 0;

                do
                {
                    var b = stream.ReadByte();

                    if (b <= 0) // null byte or stream ended
                    {
                        break;
                    }

                    if (position >= buffer.Length)
                    {
                        var newBuffer = ArrayPool<byte>.Shared.Rent(buffer.Length * 2);
                        Buffer.BlockCopy(buffer, 0, newBuffer, 0, buffer.Length);
                        ArrayPool<byte>.Shared.Return(buffer);
                        buffer = newBuffer;
                    }

                    buffer[position++] = (byte)b;
                }
                while (true);

                return Encoding.UTF8.GetString(buffer[..position]);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public static int ReadAll(this Stream stream, byte[] buffer)
        {
            int bytesRead;
            int totalRead = 0;
            while ((bytesRead = stream.Read(buffer, totalRead, buffer.Length - totalRead)) != 0)
            {
                totalRead += bytesRead;
            }
            return totalRead;
        }
    }

}
