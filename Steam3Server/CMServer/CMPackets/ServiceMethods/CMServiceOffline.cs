using Google.Protobuf;
using Steam.Messages.Offline;
using Steam3Kit;
using Steam3Kit.MSG;
using Steam3Kit.Types;
using Steam3Kit.Utils;
using Steam3Server.Servers;
using System.Security.Cryptography;
using UtilsLib;

namespace Steam3Server.CMServer.CMPackets.ServiceMethods
{
    public class CMServiceOffline
    {
        internal const string Password = "OfflineTicket";
        internal const string pbkdf1_HASH = "a449ac97703fbde2";
        internal const string pbkdf2_HASH = "aaf594640556b170";


        public static void Process(string name, PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            name = name.Split("#")[0];
            switch (name)
            {
                case "GetOfflineLogonTicket":
                    GetOfflineLogonTicket(clientMsgProtobuf, sessionBase);
                    break;
                default:
                    break;
            }
        }

        public static void GetOfflineLogonTicket(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            var offlineLogonTicket_Request = COffline_GetOfflineLogonTicket_Request.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
            var now = (uint)DateUtils.DateTimeToUnixTime(DateTime.UtcNow);
            COffline_OfflineLogonTicket offlineLogonTicket = new()
            {
                Accountid = new SteamID(clientMsgProtobuf.Header.Proto.ClientSteamId).AccountID,
                Rtime32CreationTime = now
            };
            var protoRSP = new ClientMsgProtobuf<COffline_GetOfflineLogonTicket_Response>(EMsg.ServiceMethodResponse);
            protoRSP.ParseHeader(clientMsgProtobuf);
            protoRSP.Header.Proto.Eresult = (int)EResult.OK;
            protoRSP.Body.SerializedTicket = ByteString.CopyFrom(offlineLogonTicket.ToByteArray());
            using RSA rsa = RSA.Create();
            rsa.ImportFromPem(File.ReadAllText("Keys/OfflineKey.key"));
            var HashSig = rsa.SignData(offlineLogonTicket.ToByteArray(), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            Debug.PWDebug("Ticket Hash len: " + HashSig.Length, "GetOfflineLogonTicket");

            //throw new Exception("MAKE SURE YOU ARE MAKING B1 and to B2 a new SALT!");

            protoRSP.Body.Signature = ByteString.CopyFrom(HashSig);

            if (offlineLogonTicket_Request.PerformEncryption)
            {
                Debug.PWDebug("Requested with Encryption! Opps");

                byte[] bytes1 = new byte[32];
                byte[] bytes2 = new byte[32];
                using (var pbkdf1 = new Rfc2898DeriveBytes(
                Password,
                Convert.FromHexString(pbkdf1_HASH),
                6,
                HashAlgorithmName.SHA512))
                {
                    bytes1 = pbkdf1.GetBytes(32);
                }
                using (var pbkdf2 = new Rfc2898DeriveBytes(
                Password,
                Convert.FromHexString(pbkdf2_HASH),
                4,
                HashAlgorithmName.SHA512))
                {
                    bytes2 = pbkdf2.GetBytes(32);
                }
                var byte3 = bytes1.Concat(bytes2).ToArray();
                protoRSP.Body.EncryptedTicket = new()
                {
                    EncryptedTicket = ByteString.CopyFrom(byte3),
                    Kdf1 = 6,
                    Salt1 = ByteString.CopyFrom(Convert.FromHexString(pbkdf1_HASH)),
                    Kdf2 = 4,
                    Salt2 = ByteString.CopyFrom(Convert.FromHexString(pbkdf2_HASH)),
                    Signature = ByteString.CopyFrom(HashSig)

                };
            }

            sessionBase.SendBinaryAsync(protoRSP.Serialize());
        }
    }
}
