using Newtonsoft.Json;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using static MainLib.ByteHelpers;

namespace MainLib
{
    public class Encryption
    {
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public static RSA MainKey;
        public static RSA NetworkKey;
        public static RSA NetworkKeySign;

        static Encryption()
        {

            var config = Config.GetConfig();
            MainKey = RSA.Create();
            var begin = StringToByteArray("30820120300d06092a864886f70d01010105000382010d00308201080282010100");
            var begin2 = StringToByteArray("30819d300d06092a864886f70d010101050003818b0030818702818100");
            var end = StringToByteArray("020111");
            var m = StringToByteArray(config["main_key_n"]);
            var sig = Convert.ToBase64String(begin.Concat(m).Concat(end).ToArray());
            MainKey.ImportFromPem($"-----BEGIN PUBLIC KEY-----{sig}-----END PUBLIC KEY-----");
            NetworkKey = RSA.Create();
            NetworkKey.ImportFromPem(File.ReadAllText("Keys/network_key_pub.pem").ToCharArray());
            NetworkKey.ImportFromPem(File.ReadAllText("Keys/network_key_priv.pem").ToCharArray());
            NetworkKeySign = RSA.Create();
            m = StringToByteArray(config["net_key_n"]);
            sig = Convert.ToBase64String(begin2.Concat(m).Concat(end).ToArray());
            NetworkKeySign.ImportFromPem($"-----BEGIN PUBLIC KEY-----{sig}-----END PUBLIC KEY-----");
        }

        public static byte[] GetAESKey(byte[] encryptedbytes, RSA rsakey)
        {
            var decryptedbytes = rsakey.Decrypt(encryptedbytes, RSAEncryptionPadding.Pkcs1);

            if (decryptedbytes.Length != 127)
                throw new Exception("RSAdecrypted string not the correct length!");

            var firstpasschecksum = SHA1.HashData(decryptedbytes[20..127].Concat(new byte[] { 0x00, 0x00 , 0x00, 0x00 }).ToArray());

            var secondpasskey = BinaryXOR(firstpasschecksum, decryptedbytes[0..20]);

            var spbytes = Encoding.Default.GetBytes(secondpasskey);

            var secondpasschecksum0 = SHA1.HashData(spbytes.Concat(new byte[] { 0x00, 0x00, 0x00, 0x00 }).ToArray());
            var secondpasschecksum1 = SHA1.HashData(spbytes.Concat(new byte[] { 0x00, 0x00, 0x00, 0x01 }).ToArray());
            var secondpasschecksum2 = SHA1.HashData(spbytes.Concat(new byte[] { 0x00, 0x00, 0x00, 0x02 }).ToArray());
            var secondpasschecksum3 = SHA1.HashData(spbytes.Concat(new byte[] { 0x00, 0x00, 0x00, 0x03 }).ToArray());
            var secondpasschecksum4 = SHA1.HashData(spbytes.Concat(new byte[] { 0x00, 0x00, 0x00, 0x04 }).ToArray());
            var secondpasschecksum5 = SHA1.HashData(spbytes.Concat(new byte[] { 0x00, 0x00, 0x00, 0x05 }).ToArray());
            var secondpasstotalchecksum = secondpasschecksum0.Concat(secondpasschecksum1).Concat(secondpasschecksum2).Concat(secondpasschecksum3).Concat(secondpasschecksum4).Concat(secondpasschecksum5).ToArray();

            var finishedkey = Encoding.Default.GetBytes(BinaryXOR(secondpasstotalchecksum[0..107], decryptedbytes[20..127]));

            var controlchecksum = SHA1.HashData(new byte[] { });
            if (finishedkey[0..20] != controlchecksum)
            {
                throw new Exception("Control checksum didn't match!");
            }
            return finishedkey[-16..];
        }

        public static byte[] AESDecrypt(byte[] key, byte[] IV, byte[] message)
        {
            var crypto = Aes.Create();
            crypto.Key = key;
            crypto.IV = IV;
            var dec = crypto.CreateDecryptor(crypto.Key, crypto.IV);
            byte[] decrypted = new byte[] { };
            int i = 0;
            while (i < message.Length)
            {
                decrypted = decrypted.Concat(crypto.DecryptCbc(message[i..(i + 16)], crypto.IV)).ToArray();
                i = i + 16;
            }
            return decrypted;
        }

        public static byte[] AESEncrypt(byte[] key, byte[] IV, byte[] message)
        {
            var crypto = Aes.Create();
            crypto.Key = key;
            crypto.IV = IV;

            var overflow = message.Length % 16;
            var x = (16 - overflow) * Convert.ToChar(16 - overflow);
            message = message.Append((byte)x).ToArray();
            byte[] encrypted = new byte[] { };
            int i = 0;
            while (i < message.Length)
            {
                encrypted = encrypted.Concat(crypto.EncryptCbc(message[i..(i + 16)], crypto.IV)).ToArray();
                i = i + 16;
            }
            return encrypted;
        }

        public static bool VerifyMessage(byte[] key, byte[] message)
        {
            var ms = MessageStart(key);
            var key_a = Encoding.Default.GetBytes(ms.KeyA);
            var key_b = Encoding.Default.GetBytes(ms.KeyB);

            var phrase_a = key_a.Concat(message[..20]).ToArray();
            var checksum_a = SHA1.HashData(phrase_a);
            var phrase_b = key_b.Concat(checksum_a).ToArray();
            var checksum_b = SHA1.HashData(phrase_b);

            if (checksum_b == message[-20..])
            {
                return true;
            }
            return false;
        }

        public static byte[] SignMessage(byte[] key, byte[] message)
        {
            var ms = MessageStart(key);
            var key_a = Encoding.Default.GetBytes(ms.KeyA);
            var key_b = Encoding.Default.GetBytes(ms.KeyB);

            var phrase_a = key_a.Concat(message).ToArray();
            var checksum_a = SHA1.HashData(phrase_a);
            var phrase_b = key_b.Concat(checksum_a).ToArray();
            var checksum_b = SHA1.HashData(phrase_b);
            return checksum_b;
        }


        public static (string KeyA, string KeyB) MessageStart(byte[] key)
        {
            key = key.Concat(Enumerable.Repeat<byte>(0x00, 48)).ToArray();
            byte[] xor_a = Enumerable.Repeat<byte>(0x36, 64).ToArray();
            byte[] xor_b = Enumerable.Repeat<byte>(0x5c, 64).ToArray();

            var key_a = BinaryXOR(key, xor_a);
            var key_b = BinaryXOR(key, xor_b);

            return (key_a, key_b);
        }

        public static byte[] RSASignMessage(RSA rsaKey, byte[] message)
        {
            var digest = SHA1.HashData(message);
            byte[] fulldigest = new byte[] { 0x00, 0x01 }.Concat(Enumerable.Repeat<byte>(0xff, 90)).Concat(new byte[] { 0x00, 0x30, 0x21, 0x30, 0x09, 0x06, 0x05, 0x2b, 0x0e, 0x03, 0x02, 0x1a, 0x05, 0x00, 0x04, 0x14 }).Concat(digest).ToArray();
            var signature = rsaKey.Encrypt(fulldigest, RSAEncryptionPadding.Pkcs1)[0];
            string sigpadding = "".PadLeft(128, '\x00');
            return Encoding.Default.GetBytes(sigpadding);
        }

        public static byte[] RSASignMessage1024(RSA rsaKey, byte[] message)
        {
            var digest = SHA1.HashData(message);
            byte[] fulldigest = new byte[] { 0x00, 0x01 }.Concat(Enumerable.Repeat<byte>(0xff, 218)).Concat(new byte[] { 0x00, 0x30, 0x21, 0x30, 0x09, 0x06, 0x05, 0x2b, 0x0e, 0x03, 0x02, 0x1a, 0x05, 0x00, 0x04, 0x14 }).Concat(digest).ToArray();
            var signature = rsaKey.Encrypt(fulldigest, RSAEncryptionPadding.Pkcs1)[0];
            string sigpadding = "".PadLeft(256, '\x00');
            return Encoding.Default.GetBytes(sigpadding);
        }


        public static string TextXOR(string textstring)
        {
            string key = "@#$%^&*(}]{;:<>?*&^+_-=";
            string xorded = "";
            int j = 0;

            for (int i = 0; i < textstring.Length; i++)
            {
                if (j == key.Length)
                    j = 0;

                char vA = textstring[i];
                char vB = key[j];
                char vC = (char)(vA ^ vB);
                xorded += vC;
                j = j + 1;
            }

            return xorded;
        }

        public static string BinaryXOR(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                Environment.Exit(1);
            string ret = "";

            for (int i = 0; i < a.Length; i++)
            {
                int vA = a[i];
                int vB = b[i];
                int vC = vA ^ vB;
                ret += Convert.ToChar(vC);

            }

            return ret;
        }

        public static byte[] ChunkAESDecrypt(byte[] key, byte[] chunk)
        {
            var crypto = Aes.Create();
            crypto.Key = key;
            var lastblock = Enumerable.Repeat<byte>(0x00, 16).ToArray();
            string output = "";
            for (int i = 0; i < chunk.Length; i = i+16)
            {
                var block = chunk[i..(i + 16)];
                block = block.Padding(16);
                var key2 = crypto.EncryptEcb(lastblock, PaddingMode.None);
                output += BinaryXOR(block,key2);
                lastblock = block;
            }
            byte[] ret = Encoding.Default.GetBytes(output);

            return ret[..chunk.Length];
        }
    }
}
