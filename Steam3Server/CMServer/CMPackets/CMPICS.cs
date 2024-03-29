﻿using Steam3Kit.MSG;
using Steam3Kit;
using Steam.Messages.ClientServer.Appinfo;
using Steam3Server.SQL;
using Google.Protobuf;
using Steam3Server.Servers;
using UtilsLib;
using Steam3Server.Others;
using System.Text;
using System.Linq;
using System.Security.Cryptography;

namespace Steam3Server.CMServer.CMPackets
{
    public class CMPICS
    {
        public static void Response(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            switch (clientMsgProtobuf.Header.Msg)
            {
                case EMsg.ClientPICSProductInfoRequest:
                    {
                        ProductInfo(clientMsgProtobuf, sessionBase);
                        break;
                    }
                case EMsg.ClientPICSChangesSinceRequest:
                    {
                        ChangesSince(clientMsgProtobuf, sessionBase);
                        break;
                    }
                case EMsg.ClientPICSAccessTokenRequest:
                    {
                        AccessToken(clientMsgProtobuf, sessionBase);
                        break;
                    }
                default:
                    break;
            }
        }

        static void ProductInfo(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            //  Apps buffer is text VDF, and package buffer is binary VDF
            // only meta data will be returned in the reponse (e.g. change number, missing_token, sha1)
            var proto = CMsgClientPICSProductInfoRequest.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
            Debug.PWDebug(proto.ToString());
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
                    var buf = VDFParserExt.ParseAppInfo(japp.DataByte);
                    CMsgClientPICSProductInfoResponse.Types.AppInfo app = new()
                    {
                        Appid = item.Appid,
                        ChangeNumber = japp.ChangeNumber,
                        Sha = ByteString.CopyFrom(SHA1.HashData(buf)),
                        Size = (uint)buf.Length,
                        //Buffer = ByteString.CopyFrom(japp.DataByte),
                        MissingToken = false,
                        OnlyPublic = false
                    };
                    if (!proto.MetaDataOnly) 
                    {
                        app.Buffer = ByteString.CopyFrom(buf);
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
                var jpkg = DBPackageInfo.GetPackages(item.Packageid);
                if (jpkg == null)
                {
                    UnkownPKGs.Add(item.Packageid);
                }
                else
                {
                    // should be ok result?
                    byte[] PkgBytes = new byte[] { 0x01, 0x00, 0x00, 0x00 };
                    PkgBytes = PkgBytes.Concat(jpkg.DataBytes).ToArray();
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
            Debug.PWDebug(protoRSP.Body.ToString());
            sessionBase.SendBinaryAsync(protoRSP.Serialize());
        }

        static void ChangesSince(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            var proto = CMsgClientPICSChangesSinceRequest.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
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
                    var sub = DBPackageInfo.GetPackages(subid);
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
            Debug.PWDebug(protoRSP.Body.ToString());
            sessionBase.SendBinaryAsync(protoRSP.Serialize());
        }

        static void AccessToken(PacketClientMsgProtobuf clientMsgProtobuf, WSSSessionBase sessionBase)
        {
            var proto = CMsgClientPICSAccessTokenRequest.Parser.ParseFrom(clientMsgProtobuf.GetData()[(int)clientMsgProtobuf.BodyOffset..]);
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
                var jpkg = DBPackageInfo.GetPackages(item);
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
            sessionBase.SendBinaryAsync(protoRSP.Serialize());
        }
    }
}
