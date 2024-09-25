using System.Diagnostics.CodeAnalysis;

namespace Steam3Kit.Utils;

public static class Utils
{
    /// <summary>
    /// Performs an Adler32 on the given input
    /// </summary>
    public static uint AdlerHash(ReadOnlySpan<byte> input)
    {
        uint a = 0, b = 0;
        for (int i = 0; i < input.Length; i++)
        {
            a = (a + input[i]) % 65521;
            b = (b + a) % 65521;
        }

        return a | (b << 16);
    }

    public static string EncodeHexString(byte[] input)
    {
        return Convert.ToHexString(input).ToLowerInvariant();
    }

    [return: NotNullIfNotNull(nameof(hex))]
    public static byte[]? DecodeHexString(string? hex)
    {
        if (hex == null)
            return null;

        return Convert.FromHexString(hex);
    }
}