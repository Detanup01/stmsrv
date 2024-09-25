using Google.Protobuf;
using Steam.Messages.Base;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Net;

namespace Steam3Kit.Utils;

public static class NetHelpers
{
    public static IPAddress GetLocalIP(Socket activeSocket)
    {
        var ipEndPoint = activeSocket.LocalEndPoint as IPEndPoint;

        if (ipEndPoint == null || ipEndPoint.Address == IPAddress.Any)
            throw new InvalidOperationException("Socket not connected");

        return ipEndPoint.Address;
    }

    public static IPAddress GetIPAddress(uint ipAddr)
    {
        byte[] addrBytes = BitConverter.GetBytes(ipAddr);
        Array.Reverse(addrBytes);

        return new IPAddress(addrBytes);
    }

    public static uint GetIPAddressAsUInt(IPAddress ipAddr)
    {
        byte[] addrBytes = ipAddr.GetAddressBytes();
        Array.Reverse(addrBytes);

        return BitConverter.ToUInt32(addrBytes, 0);
    }

    public static IPAddress GetIPAddress(this CMsgIPAddress ipAddr)
    {
        if (ipAddr.HasV6)
        {
            return new IPAddress(ipAddr.V6.ToArray());
        }
        else
        {
            return GetIPAddress(ipAddr.V4);
        }
    }

    public static CMsgIPAddress GetMsgIPAddress(IPAddress ipAddr)
    {
        var msgIpAddress = new CMsgIPAddress();
        byte[] addrBytes = ipAddr.GetAddressBytes();

        if (ipAddr.AddressFamily == AddressFamily.InterNetworkV6)
        {
            msgIpAddress.V6 = ByteString.CopyFrom(addrBytes);
        }
        else
        {
            Array.Reverse(addrBytes);

            msgIpAddress.V4 = BitConverter.ToUInt32(addrBytes, 0);
        }

        return msgIpAddress;
    }

    public static CMsgIPAddress ObfuscatePrivateIP(this CMsgIPAddress msgIpAddress)
    {
        var localIp = msgIpAddress;

        if (localIp.HasV6)
        {
            byte[] v6 = localIp.V6.ToArray();
            v6[0] ^= 0x0D;
            v6[1] ^= 0xF0;
            v6[2] ^= 0xAD;
            v6[3] ^= 0xBA;

            v6[4] ^= 0x0D;
            v6[5] ^= 0xF0;
            v6[6] ^= 0xAD;
            v6[7] ^= 0xBA;

            v6[8] ^= 0x0D;
            v6[9] ^= 0xF0;
            v6[10] ^= 0xAD;
            v6[11] ^= 0xBA;

            v6[12] ^= 0x0D;
            v6[13] ^= 0xF0;
            v6[14] ^= 0xAD;
            v6[15] ^= 0xBA;
            localIp.V6 = ByteString.CopyFrom(v6);
        }
        else
        {
            localIp.V4 ^= MsgClientLogon.ObfuscationMask;
        }

        return localIp;
    }

    public static bool TryParseIPEndPoint(string stringValue, [NotNullWhen(true)] out IPEndPoint? endPoint)
    {
        var colonPosition = stringValue.LastIndexOf(':');

        if (colonPosition == -1)
        {
            endPoint = null;
            return false;
        }

        if (!IPAddress.TryParse(stringValue.Substring(0, colonPosition), out var address))
        {
            endPoint = null;
            return false;
        }

        if (!ushort.TryParse(stringValue.Substring(colonPosition + 1), out var port))
        {
            endPoint = null;
            return false;
        }

        endPoint = new IPEndPoint(address, port);
        return true;
    }

    public static (string host, int port) ExtractEndpointHost(EndPoint endPoint)
    {
        switch (endPoint)
        {
            case IPEndPoint ipep:
                return (ipep.Address.ToString(), ipep.Port);

            case DnsEndPoint dns:
                return (dns.Host, dns.Port);

            default:
                throw new InvalidOperationException("Unknown endpoint type.");
        }
    }
}