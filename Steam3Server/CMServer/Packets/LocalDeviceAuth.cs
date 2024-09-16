﻿using Steam.Messages.ClientServer.Login;
using Steam3Kit.MSG;
using Steam3Kit;
using Steam.Messages.ClientServer2;
using Steam.Messages.ClientServer.Friends;
using Steam.Messages.ClientServer;
using Steam3Kit.Utils;
using ModdableWebServer;
using ModdableWebServer.Helper;
using Steam3Kit.Types;

namespace Steam3Server.CMServer.Packets;

/*
 * This one used for response with:
 * ClientAccountInfo
 * ClientEmailAddrInfo
 * ClientFriendsList
 * ClientPlayerNicknameList
 * ClientLicenseList
 */
public class LocalDeviceAuth
{
    public static void Response(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        var logon = CMsgClientLogon.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/LocalDeviceAuth.txt", logon.ToString() + "\n");

        var id = new SteamID(logon.ClientSuppliedSteamId);
        if (id.AccountType != EAccountType.Individual)
            return;
        //  Sending AccountInfo
        var protoRSP = new ClientMsgProtobuf<CMsgClientAccountInfo>(EMsg.ClientAccountInfo);
        protoRSP.ParseHeader(clientMsgProtobuf);
        protoRSP.Header.Proto.Eresult = (int)EResult.OK;
        protoRSP.Body.AccountFlags = 133;
        protoRSP.Body.PersonaName = "TestUser";
        protoRSP.Body.CountAuthedComputers = 1;
        protoRSP.Body.IpCountry = "US";
        protoRSP.Body.TwoFactorState = 0;
        webSocket.SendWebSocketByteArray(protoRSP.Serialize());

        //  Sending EmailAddrInfo
        var protoRSP2 = new ClientMsgProtobuf<CMsgClientEmailAddrInfo>(EMsg.ClientEmailAddrInfo);
        protoRSP2.ParseHeader(clientMsgProtobuf);
        protoRSP2.Header.Proto.Eresult = (int)EResult.OK;
        protoRSP2.Body.EmailAddress = "TestUser@STMSRV.com";
        protoRSP2.Body.EmailIsValidated = true;
        protoRSP2.Body.CredentialChangeRequiresCode = true;
        protoRSP2.Body.PasswordOrSecretqaChangeRequiresCode = true;
        webSocket.SendWebSocketByteArray(protoRSP2.Serialize());


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
        webSocket.SendWebSocketByteArray(protoRSP3.Serialize());


        //  Sending ClientPlayerNicknameList
        var protoRSP4 = new ClientMsgProtobuf<CMsgClientPlayerNicknameList>(EMsg.ClientPlayerNicknameList);
        protoRSP4.ParseHeader(clientMsgProtobuf);
        protoRSP4.Header.Proto.Eresult = (int)EResult.OK;
        protoRSP4.Body.Removal = false;
        protoRSP4.Body.Incremental = false;
        webSocket.SendWebSocketByteArray(protoRSP4.Serialize());

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
        webSocket.SendWebSocketByteArray(protoRSP5.Serialize());
    }
}
