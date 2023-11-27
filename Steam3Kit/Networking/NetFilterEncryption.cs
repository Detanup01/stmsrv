using Steam3Kit.Utils;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Steam3Kit.Networking
{
    class NetFilterEncryption : INetFilterEncryption
    {
        readonly byte[] sessionKey;

        public NetFilterEncryption(byte[] sessionKey)
        {
            Debug.Assert(sessionKey.Length == 32, nameof(NetFilterEncryption), "AES session key was not 32 bytes!");

            this.sessionKey = sessionKey;
        }

        public byte[] ProcessIncoming(byte[] data)
        {
            try
            {
                return CryptoHelper.SymmetricDecrypt(data, sessionKey);
            }
            catch (CryptographicException ex)
            {
                //log.LogDebug(nameof(NetFilterEncryption), "Unable to decrypt incoming packet: " + ex.Message);

                // rethrow as an IO exception so it's handled in the network thread
                throw new IOException("Unable to decrypt incoming packet", ex);
            }
        }

        public byte[] ProcessOutgoing(byte[] data)
        {
            return CryptoHelper.SymmetricEncrypt(data, sessionKey);
        }
    }
}
