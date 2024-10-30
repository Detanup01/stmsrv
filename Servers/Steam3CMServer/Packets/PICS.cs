using Steam3Kit.MSG;
using Steam3Kit;
using Steam.Messages.ClientServer.Appinfo;
using Google.Protobuf;
using ModdableWebServer;
using ModdableWebServer.Helper;
using DB4Steam;
using UtilsLib;
using PICS_Backend;

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
        var proto = clientMsgProtobuf.GetBody<CMsgClientPICSProductInfoRequest>();
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/ProductInfo.txt", "REQ:" + proto.ToString() + "\n");
        var protoRSP = new ClientMsgProtobuf<CMsgClientPICSProductInfoResponse>(EMsg.ClientPICSProductInfoResponse);
        protoRSP.ParseHeader(clientMsgProtobuf);
        protoRSP.Header.Proto.Eresult = (int)EResult.OK;
        protoRSP.Body.MetaDataOnly = proto.MetaDataOnly;
        List<CMsgClientPICSProductInfoResponse.Types.AppInfo> appInfos = new();
        List<uint> UnknownApps = new();
        var apps = proto.Apps.ToList();
        foreach (var item in apps)
        {
            var japp = DBApp.GetApp(item.Appid);
            if (japp == null)
            {
                UnknownApps.Add(item.Appid);
            }
            else if (item.AccessToken != 0)
            {
                // check if AT is good with it.
            }
            else
            {
                var data = japp.GetAppInfoStringData();
                CMsgClientPICSProductInfoResponse.Types.AppInfo app = new()
                {
                    Appid = item.Appid,
                    ChangeNumber = japp.ChangeNumber,
                    Sha = ByteString.CopyFrom(japp.Hash),
                    Size = (uint)data.Length,
                    MissingToken = false,
                    OnlyPublic = false,
                };
                if (!proto.MetaDataOnly)
                {
                    app.Buffer = ByteString.CopyFrom(data);
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
            var jpkg = DBPackages.GetPackage(item.Packageid);
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
                    Sha = ByteString.CopyFrom(jpkg.Hash),
                    Size = (uint)PkgBytes.Length + 4,
                    Packageid = item.Packageid,
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
        protoRSP.Body.HttpHost = "192.168.1.50"; //Replace this with custom url.
        protoRSP.Body.ResponsePending = false;
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/ProductInfo.txt", "RSP:" + protoRSP.Body.ToString() + "\n");
        webSocket.SendWebSocketByteArray(protoRSP.Serialize());
    }

    static void ChangesSince(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        var proto = clientMsgProtobuf.GetBody<CMsgClientPICSChangesSinceRequest>();
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/ChangesSince.txt", "REQ:" + proto.ToString() + "\n");
        var protoRSP = new ClientMsgProtobuf<CMsgClientPICSChangesSinceResponse>(EMsg.ClientPICSChangesSinceResponse);
        protoRSP.ParseHeader(clientMsgProtobuf);
        protoRSP.Header.Proto.Steamid = clientMsgProtobuf.Header.Proto.Steamid;
        protoRSP.Header.Proto.ClientSessionid = clientMsgProtobuf.Header.Proto.ClientSessionid;
        List<CMsgClientPICSChangesSinceResponse.Types.AppChange> appChanges = new();
        List<CMsgClientPICSChangesSinceResponse.Types.PackageChange> packageChanges = new();
        uint since = proto.SinceChangeNumber;
        foreach (var app in DBApp.GetApps(x => x.ChangeNumber > since))
        {
            if (app != null)
            {
                appChanges.Add(new()
                {
                    Appid = app.AppID,
                    ChangeNumber = app.ChangeNumber,
                    NeedsToken = app.Token != 0
                });
            }
        }

        foreach (var sub in DBPackages.GetPackages(x => x.ChangeNumber > since))
        {
            if (sub != null)
            {
                packageChanges.Add(new()
                {
                    Packageid = sub.SubID,
                    ChangeNumber = sub.ChangeNumber,
                    NeedsToken = false
                });
            }
        }
        protoRSP.Body = new()
        { 
            AppChanges = { appChanges },
            CurrentChangeNumber = CustomPICSVersioning.Latest,
            PackageChanges = { packageChanges },
            SinceChangeNumber = proto.SinceChangeNumber
        };
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/ChangesSince.txt", "RSP:" + protoRSP.Body.ToString() + "\n");
        webSocket.SendWebSocketByteArray(protoRSP.Serialize());
    }

    static void AccessToken(PacketClientMsgProtobuf clientMsgProtobuf, WebSocketStruct webSocket)
    {
        var proto = clientMsgProtobuf.GetBody<CMsgClientPICSAccessTokenRequest>();
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/AccessToken.txt", "REQ:" + proto.ToString() + "\n");
        var protoRSP = new ClientMsgProtobuf<CMsgClientPICSAccessTokenResponse>(EMsg.ClientPICSAccessTokenResponse);
        protoRSP.ParseHeader(clientMsgProtobuf);
        protoRSP.Header.Proto.Eresult = (int)EResult.OK;
        List<CMsgClientPICSAccessTokenResponse.Types.AppToken> appTokens = new();
        List<CMsgClientPICSAccessTokenResponse.Types.PackageToken> packageTokens = new();
        List<uint> denied = new();
        foreach (var item in proto.Appids)
        {
            var japp = DBApp.GetApp(item);
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
            var jpkg = DBPackages.GetPackage(item);
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
        if (!Directory.Exists("Packets"))
            Directory.CreateDirectory("Packets");
        File.AppendAllText("Packets/AccessToken.txt", "RSP:" + protoRSP.Body.ToString() + "\n");
        webSocket.SendWebSocketByteArray(protoRSP.Serialize());
    }
}
