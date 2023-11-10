using Steam.Messages.ClientServer.Login;
using Steam3Kit.MSG;
using Steam3Kit;
using UtilsLib;
using Steam.Messages.ClientServer2;
using Steam.Messages.ClientServer.Friends;
using Steam.Messages.ClientServer;
using Steam3Kit.Utils;
using Steam3Server.Servers;

namespace Steam3Server.CMServer.CMPackets
{
    /*
     * This one used for response with:
     * ClientAccountInfo
     * ClientEmailAddrInfo
     * ClientFriendsList
     * ClientPlayerNicknameList
     * ClientLicenseList
     */
    public class CMLocalDeviceAuth
    {
        public static void Response(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            Debug.PWDebug("Sending CMLoginRSP");
            var rsp = new byte[] { };
            var logon = CMsgClientLogon.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);


            //  Sending AccountInfo
            var protoRSP = new ClientMsgProtobuf<CMsgClientAccountInfo>(EMsg.ClientAccountInfo);
            protoRSP.ParseHeader(clientMsgProtobuf);
            protoRSP.Header.Proto.Eresult = (int)EResult.OK;
            protoRSP.Body.AccountFlags = 133;
            protoRSP.Body.PersonaName = "TestUser";
            protoRSP.Body.CountAuthedComputers = 1;
            protoRSP.Body.IpCountry = "US";
            protoRSP.Body.TwoFactorState = 0;
            rsp = protoRSP.Serialize();
            sessionBase.SendBinaryAsync(rsp);

            //  Sending EmailAddrInfo
            var protoRSP2 = new ClientMsgProtobuf<CMsgClientEmailAddrInfo>(EMsg.ClientEmailAddrInfo);
            protoRSP2.ParseHeader(clientMsgProtobuf);
            protoRSP2.Header.Proto.Eresult = (int)EResult.OK;
            protoRSP2.Body.EmailAddress = "TestUser@STMSRV.com";
            protoRSP2.Body.EmailIsValidated = true;
            protoRSP2.Body.CredentialChangeRequiresCode = true;
            protoRSP2.Body.PasswordOrSecretqaChangeRequiresCode = true;
            rsp = protoRSP2.Serialize();
            sessionBase.SendBinaryAsync(rsp);


            //  Sending FriendsList
            var protoRSP3 = new ClientMsgProtobuf<CMsgClientFriendsList>(EMsg.ClientFriendsList);
            protoRSP3.ParseHeader(clientMsgProtobuf);
            protoRSP3.Header.Proto.Eresult = (int)EResult.OK;
            protoRSP3.Body.Bincremental = false;

            List<CMsgClientFriendsList.Types.Friend> friends = new()
            {
                new CMsgClientFriendsList.Types.Friend()
                {
                    Ulfriendid = 76561199897966918,
                    Efriendrelationship = (int)EFriendRelationship.Friend
                }
            };
            protoRSP3.Body.Friends.AddRange(friends);
            rsp = protoRSP3.Serialize();
            sessionBase.SendBinaryAsync(rsp);


            //  Sending ClientPlayerNicknameList
            var protoRSP4 = new ClientMsgProtobuf<CMsgClientPlayerNicknameList>(EMsg.ClientPlayerNicknameList);
            protoRSP4.ParseHeader(clientMsgProtobuf);
            protoRSP4.Header.Proto.Eresult = (int)EResult.OK;
            protoRSP4.Body.Removal = false;
            protoRSP4.Body.Incremental = false;
            rsp = protoRSP4.Serialize();
            sessionBase.SendBinaryAsync(rsp);

            //  Sending ClientLicenseList
            var protoRSP5 = new ClientMsgProtobuf<CMsgClientLicenseList>(EMsg.ClientLicenseList);
            protoRSP5.ParseHeader(clientMsgProtobuf);
            protoRSP5.Header.Proto.Eresult = (int)EResult.OK;
            protoRSP5.Body.Eresult = (int)EResult.OK;
            protoRSP5.Body.Licenses.Add(new CMsgClientLicenseList.Types.License()
            {
                PackageId = 0,
                TimeCreated = (uint)DateUtils.DateTimeToUnixTime(DateTime.UtcNow.AddDays(-10)),
                TimeNextProcess = 0,
                MinuteLimit = 0,
                MinutesUsed = 0,
                PaymentMethod = (uint)EPaymentMethod.AutoGrant,
                LicenseType = (uint)ELicenseType.NoLicense,
                TerritoryCode = 0,
                PurchaseCountryCode = "US",
                ChangeNumber = 5676048,
            });
            rsp = protoRSP5.Serialize();
            sessionBase.SendBinaryAsync(rsp);
        }
    }
}
