using Steam3Kit.MSG;
using Steam3Kit;
using Steam.Messages.ClientServer.Appinfo;
using Steam3Server.SQL;
using Google.Protobuf;
using UtilsLib;
using System.Security.Cryptography;
using Steam3Kit.Types;
using ModdableWebServer;
using ModdableWebServer.Helper;
using System.Text;

namespace Steam3Server.CMServer.Packets;

public class PICS
{
    public static void Response(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        switch (clientMsgProtobuf.Header.Msg)
        {
            case EMsg.ClientPICSProductInfoRequest:
                {
                    ProductInfo(clientMsgProtobuf, webSocket);
                    break;
                }
            case EMsg.ClientPICSChangesSinceRequest:
                {
                    ChangesSince(clientMsgProtobuf, webSocket);
                    break;
                }
            case EMsg.ClientPICSAccessTokenRequest:
                {
                    AccessToken(clientMsgProtobuf, webSocket);
                    break;
                }
            default:
                break;
        }
    }

    static void ProductInfo(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        //  Apps buffer is text VDF, and package buffer is binary VDF
        // only meta data will be returned in the reponse (e.g. change number, missing_token, sha1)
        var proto = CMsgClientPICSProductInfoRequest.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/ProductInfo.txt", proto.ToString() + "\n");
        var protoRSP = new ClientMsgProtobuf<CMsgClientPICSProductInfoResponse>(EMsg.ClientPICSProductInfoResponse);
        protoRSP.ParseHeader(clientMsgProtobuf);
        protoRSP.Header.Proto.Eresult = (int)EResult.OK;
        protoRSP.Body.MetaDataOnly = proto.MetaDataOnly;
        List<CMsgClientPICSProductInfoResponse.Types.AppInfo> appInfos = new();
        List<uint> UnknownApps = new();
        var apps = proto.Apps.ToList();
        foreach (var item in apps)
        {
            var japp = DBAppInfo.GetApp(item.Appid);
            if (japp == null)
            {
                UnknownApps.Add(item.Appid);
            }
            else
            {
                string des_string = Encoding.UTF8.GetString(japp.DataByte);
                des_string = des_string.Replace("\"\t\"", "\"\t\t\"");
                var toZip = Encoding.UTF8.GetBytes(des_string).Concat(new byte[] { 0x0 }).ToArray();
                CMsgClientPICSProductInfoResponse.Types.AppInfo app = new()
                {
                    Appid = item.Appid,
                    ChangeNumber = japp.ChangeNumber,
                    Sha = ByteString.CopyFrom(japp.Hash),//ByteString.CopyFrom(SHA1.HashData(toZip)),
                    Size = (uint)toZip.Length,
                    MissingToken = false,
                    OnlyPublic = false, 
                };
                if (!proto.MetaDataOnly) 
                {
                    app.Buffer = ByteString.CopyFrom(toZip);
                }
                appInfos.Add(app);
            }
        }
        protoRSP.Body.UnknownAppids.AddRange(UnknownApps);
        protoRSP.Body.Apps.AddRange(appInfos);

        List<CMsgClientPICSProductInfoResponse.Types.PackageInfo> pkgInfos = new();
        List<uint> UnkownPKGs = new();
        var pkgs = proto.Packages.ToList();
        foreach (var item in pkgs)
        {
            var jpkg = DBPackageInfo.GetPackage(item.Packageid);
            if (jpkg == null)
            {
                UnkownPKGs.Add(item.Packageid);
            }
            else
            {
                using MemoryStream ms = new MemoryStream(jpkg.DataBytes);
                using var br = new BinaryWriter(ms);
                br.Seek(0, SeekOrigin.Begin);
                br.Write((uint)1);
                br.Flush();
                br.Dispose();
                var PkgBytes = ms.ToArray();
                ms.Dispose();
                CMsgClientPICSProductInfoResponse.Types.PackageInfo package = new()
                { 
                    ChangeNumber = jpkg.ChangeNumber,
                    Sha = ByteString.CopyFrom(SHA1.HashData(PkgBytes)),
                    Size = (uint)PkgBytes.Length,
                    Packageid = item.Packageid,
                    //Buffer = ByteString.CopyFrom(jpkg.DataBytes),
                    MissingToken = false
                }; 
                if (!proto.MetaDataOnly)
                {
                    package.Buffer = ByteString.CopyFrom(PkgBytes);
                }
                pkgInfos.Add(package);
            }
        }
        protoRSP.Body.UnknownPackageids.AddRange(UnkownPKGs);
        protoRSP.Body.Packages.AddRange(pkgInfos);
        protoRSP.Body.HttpMinSize = 4096;
        protoRSP.Body.HttpHost = "clientconfig.local.steamstatic.com";
        protoRSP.Body.ResponsePending = false;
        webSocket.SendWebSocketByteArray(protoRSP.Serialize());
    }

    static void ChangesSince(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        var proto = CMsgClientPICSChangesSinceRequest.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/ChangesSince.txt", proto.ToString() + "\n");
        var protoRSP = new ClientMsgProtobuf<CMsgClientPICSChangesSinceResponse>(EMsg.ClientPICSChangesSinceResponse);
        protoRSP.ParseHeader(clientMsgProtobuf);
        List<CMsgClientPICSChangesSinceResponse.Types.AppChange> appChanges = new();
        List<CMsgClientPICSChangesSinceResponse.Types.PackageChange> packageChanges = new();
        uint since = proto.SinceChangeNumber;
        if (proto.SendAppInfoChanges)
        {
            var appinfo = DBAppInfo.GetAppInfoCache();
            foreach (var appid in appinfo.Apps)
            {
                var app = DBAppInfo.GetApp(appid);
                if (app != null && app.ChangeNumber > since)
                {
                    appChanges.Add(new()
                    { 
                        Appid = appid,
                        ChangeNumber = app.ChangeNumber,
                        NeedsToken = false
                    });
                }
            }
        }
        if (proto.SendPackageInfoChanges)
        {
            var packageInfo = DBPackageInfo.GetPackageInfoCache();
            foreach (var subid in packageInfo.Packages)
            {
                var sub = DBPackageInfo.GetPackage(subid);
                if (sub != null && sub.ChangeNumber > since)
                {
                    packageChanges.Add(new()
                    {
                        Packageid = subid,
                        ChangeNumber = sub.ChangeNumber,
                        NeedsToken = false
                    });
                }
            }
        }
        protoRSP.Body = new()
        { 
            AppChanges = { appChanges },
            CurrentChangeNumber = 19573493,
            PackageChanges = { packageChanges },
            SinceChangeNumber = proto.SinceChangeNumber
        };
        webSocket.SendWebSocketByteArray(protoRSP.Serialize());
    }

    static void AccessToken(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        var proto = CMsgClientPICSAccessTokenRequest.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/AccessToken.txt", proto.ToString() + "\n");
        var protoRSP = new ClientMsgProtobuf<CMsgClientPICSAccessTokenResponse>(EMsg.ClientPICSAccessTokenResponse);
        protoRSP.ParseHeader(clientMsgProtobuf);
        protoRSP.Header.Proto.Eresult = (int)EResult.OK;
        List<CMsgClientPICSAccessTokenResponse.Types.AppToken> appTokens = new();
        List<CMsgClientPICSAccessTokenResponse.Types.PackageToken> packageTokens = new();
        List<uint> denied = new();
        foreach (var item in proto.Appids)
        {
            var japp = DBAppInfo.GetApp(item);
            if (japp == null)
            {
                denied.Add(item);
            }
            else
            {
                appTokens.Add(new()
                { 
                    Appid = item,
                    AccessToken = japp.Token
                });
            }
        }
        protoRSP.Body.AppAccessTokens.AddRange(appTokens);
        protoRSP.Body.AppDeniedTokens.AddRange(denied);
        denied = new();
        foreach (var item in proto.Packageids)
        {
            var jpkg = DBPackageInfo.GetPackage(item);
            if (jpkg == null)
            {
                denied.Add(item);
            }
            else
            {
                packageTokens.Add(new()
                {
                    Packageid = item,
                    AccessToken = jpkg.Token
                });
            }
        }
        protoRSP.Body.PackageAccessTokens.AddRange(packageTokens);
        protoRSP.Body.PackageDeniedTokens.AddRange(denied);
        webSocket.SendWebSocketByteArray(protoRSP.Serialize());
    }
}
