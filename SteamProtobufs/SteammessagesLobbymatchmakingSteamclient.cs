// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: steammessages_lobbymatchmaking.steamclient.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Steam.Messages.LobbyMatchmaking {

  /// <summary>Holder for reflection information generated from steammessages_lobbymatchmaking.steamclient.proto</summary>
  public static partial class SteammessagesLobbymatchmakingSteamclientReflection {

    #region Descriptor
    /// <summary>File descriptor for steammessages_lobbymatchmaking.steamclient.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static SteammessagesLobbymatchmakingSteamclientReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CjBzdGVhbW1lc3NhZ2VzX2xvYmJ5bWF0Y2htYWtpbmcuc3RlYW1jbGllbnQu",
            "cHJvdG8aGHN0ZWFtbWVzc2FnZXNfYmFzZS5wcm90bxosc3RlYW1tZXNzYWdl",
            "c191bmlmaWVkX2Jhc2Uuc3RlYW1jbGllbnQucHJvdG8ingEKLUxvYmJ5TWF0",
            "Y2htYWtpbmdMZWdhY3lfR2V0TG9iYnlTdGF0dXNfUmVxdWVzdBIOCgZhcHBf",
            "aWQYASABKA0SFQoNc3RlYW1pZF9sb2JieRgCIAEoBhIXCg9jbGFpbV9vd25l",
            "cnNoaXAYAyABKAgSGAoQY2xhaW1fbWVtYmVyc2hpcBgEIAEoCBITCgt2ZXJz",
            "aW9uX251bRgFIAEoDSKTAQouTG9iYnlNYXRjaG1ha2luZ0xlZ2FjeV9HZXRM",
            "b2JieVN0YXR1c19SZXNwb25zZRIOCgZhcHBfaWQYASABKA0SFQoNc3RlYW1p",
            "ZF9sb2JieRgCIAEoBhI6Cgxsb2JieV9zdGF0dXMYAyABKA4yDS5FTG9iYnlT",
            "dGF0dXM6FWtfRUxvYmJ5U3RhdHVzSW52YWxpZCqBAQoMRUxvYmJ5U3RhdHVz",
            "EhkKFWtfRUxvYmJ5U3RhdHVzSW52YWxpZBAAEhgKFGtfRUxvYmJ5U3RhdHVz",
            "RXhpc3RzEAESHgoaa19FTG9iYnlTdGF0dXNEb2VzTm90RXhpc3QQAhIcChhr",
            "X0VMb2JieVN0YXR1c05vdEFNZW1iZXIQAzLGAQoWTG9iYnlNYXRjaG1ha2lu",
            "Z0xlZ2FjeRKFAQoOR2V0TG9iYnlTdGF0dXMSLi5Mb2JieU1hdGNobWFraW5n",
            "TGVnYWN5X0dldExvYmJ5U3RhdHVzX1JlcXVlc3QaLy5Mb2JieU1hdGNobWFr",
            "aW5nTGVnYWN5X0dldExvYmJ5U3RhdHVzX1Jlc3BvbnNlIhKCtRgOR2V0TG9i",
            "YnlTdGF0dXMaJIK1GCBMb2JieSBtYXRjaG1ha2luZyBsZWdhY3kgc2Vydmlj",
            "ZUIlgAEBqgIfU3RlYW0uTWVzc2FnZXMuTG9iYnlNYXRjaG1ha2luZw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Steam.Messages.Base.SteammessagesBaseReflection.Descriptor, global::Steam.Messages.Unified.Base.SteammessagesUnifiedBaseSteamclientReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Steam.Messages.LobbyMatchmaking.ELobbyStatus), }, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Steam.Messages.LobbyMatchmaking.LobbyMatchmakingLegacy_GetLobbyStatus_Request), global::Steam.Messages.LobbyMatchmaking.LobbyMatchmakingLegacy_GetLobbyStatus_Request.Parser, new[]{ "AppId", "SteamidLobby", "ClaimOwnership", "ClaimMembership", "VersionNum" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Steam.Messages.LobbyMatchmaking.LobbyMatchmakingLegacy_GetLobbyStatus_Response), global::Steam.Messages.LobbyMatchmaking.LobbyMatchmakingLegacy_GetLobbyStatus_Response.Parser, new[]{ "AppId", "SteamidLobby", "LobbyStatus" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum ELobbyStatus {
    [pbr::OriginalName("k_ELobbyStatusInvalid")] KElobbyStatusInvalid = 0,
    [pbr::OriginalName("k_ELobbyStatusExists")] KElobbyStatusExists = 1,
    [pbr::OriginalName("k_ELobbyStatusDoesNotExist")] KElobbyStatusDoesNotExist = 2,
    [pbr::OriginalName("k_ELobbyStatusNotAMember")] KElobbyStatusNotAmember = 3,
  }

  #endregion

  #region Messages
  public sealed partial class LobbyMatchmakingLegacy_GetLobbyStatus_Request : pb::IMessage<LobbyMatchmakingLegacy_GetLobbyStatus_Request>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<LobbyMatchmakingLegacy_GetLobbyStatus_Request> _parser = new pb::MessageParser<LobbyMatchmakingLegacy_GetLobbyStatus_Request>(() => new LobbyMatchmakingLegacy_GetLobbyStatus_Request());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<LobbyMatchmakingLegacy_GetLobbyStatus_Request> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Steam.Messages.LobbyMatchmaking.SteammessagesLobbymatchmakingSteamclientReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public LobbyMatchmakingLegacy_GetLobbyStatus_Request() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public LobbyMatchmakingLegacy_GetLobbyStatus_Request(LobbyMatchmakingLegacy_GetLobbyStatus_Request other) : this() {
      _hasBits0 = other._hasBits0;
      appId_ = other.appId_;
      steamidLobby_ = other.steamidLobby_;
      claimOwnership_ = other.claimOwnership_;
      claimMembership_ = other.claimMembership_;
      versionNum_ = other.versionNum_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public LobbyMatchmakingLegacy_GetLobbyStatus_Request Clone() {
      return new LobbyMatchmakingLegacy_GetLobbyStatus_Request(this);
    }

    /// <summary>Field number for the "app_id" field.</summary>
    public const int AppIdFieldNumber = 1;
    private readonly static uint AppIdDefaultValue = 0;

    private uint appId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint AppId {
      get { if ((_hasBits0 & 1) != 0) { return appId_; } else { return AppIdDefaultValue; } }
      set {
        _hasBits0 |= 1;
        appId_ = value;
      }
    }
    /// <summary>Gets whether the "app_id" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasAppId {
      get { return (_hasBits0 & 1) != 0; }
    }
    /// <summary>Clears the value of the "app_id" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearAppId() {
      _hasBits0 &= ~1;
    }

    /// <summary>Field number for the "steamid_lobby" field.</summary>
    public const int SteamidLobbyFieldNumber = 2;
    private readonly static ulong SteamidLobbyDefaultValue = 0UL;

    private ulong steamidLobby_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ulong SteamidLobby {
      get { if ((_hasBits0 & 2) != 0) { return steamidLobby_; } else { return SteamidLobbyDefaultValue; } }
      set {
        _hasBits0 |= 2;
        steamidLobby_ = value;
      }
    }
    /// <summary>Gets whether the "steamid_lobby" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasSteamidLobby {
      get { return (_hasBits0 & 2) != 0; }
    }
    /// <summary>Clears the value of the "steamid_lobby" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearSteamidLobby() {
      _hasBits0 &= ~2;
    }

    /// <summary>Field number for the "claim_ownership" field.</summary>
    public const int ClaimOwnershipFieldNumber = 3;
    private readonly static bool ClaimOwnershipDefaultValue = false;

    private bool claimOwnership_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool ClaimOwnership {
      get { if ((_hasBits0 & 4) != 0) { return claimOwnership_; } else { return ClaimOwnershipDefaultValue; } }
      set {
        _hasBits0 |= 4;
        claimOwnership_ = value;
      }
    }
    /// <summary>Gets whether the "claim_ownership" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasClaimOwnership {
      get { return (_hasBits0 & 4) != 0; }
    }
    /// <summary>Clears the value of the "claim_ownership" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearClaimOwnership() {
      _hasBits0 &= ~4;
    }

    /// <summary>Field number for the "claim_membership" field.</summary>
    public const int ClaimMembershipFieldNumber = 4;
    private readonly static bool ClaimMembershipDefaultValue = false;

    private bool claimMembership_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool ClaimMembership {
      get { if ((_hasBits0 & 8) != 0) { return claimMembership_; } else { return ClaimMembershipDefaultValue; } }
      set {
        _hasBits0 |= 8;
        claimMembership_ = value;
      }
    }
    /// <summary>Gets whether the "claim_membership" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasClaimMembership {
      get { return (_hasBits0 & 8) != 0; }
    }
    /// <summary>Clears the value of the "claim_membership" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearClaimMembership() {
      _hasBits0 &= ~8;
    }

    /// <summary>Field number for the "version_num" field.</summary>
    public const int VersionNumFieldNumber = 5;
    private readonly static uint VersionNumDefaultValue = 0;

    private uint versionNum_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint VersionNum {
      get { if ((_hasBits0 & 16) != 0) { return versionNum_; } else { return VersionNumDefaultValue; } }
      set {
        _hasBits0 |= 16;
        versionNum_ = value;
      }
    }
    /// <summary>Gets whether the "version_num" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasVersionNum {
      get { return (_hasBits0 & 16) != 0; }
    }
    /// <summary>Clears the value of the "version_num" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearVersionNum() {
      _hasBits0 &= ~16;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as LobbyMatchmakingLegacy_GetLobbyStatus_Request);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(LobbyMatchmakingLegacy_GetLobbyStatus_Request other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (AppId != other.AppId) return false;
      if (SteamidLobby != other.SteamidLobby) return false;
      if (ClaimOwnership != other.ClaimOwnership) return false;
      if (ClaimMembership != other.ClaimMembership) return false;
      if (VersionNum != other.VersionNum) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (HasAppId) hash ^= AppId.GetHashCode();
      if (HasSteamidLobby) hash ^= SteamidLobby.GetHashCode();
      if (HasClaimOwnership) hash ^= ClaimOwnership.GetHashCode();
      if (HasClaimMembership) hash ^= ClaimMembership.GetHashCode();
      if (HasVersionNum) hash ^= VersionNum.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (HasAppId) {
        output.WriteRawTag(8);
        output.WriteUInt32(AppId);
      }
      if (HasSteamidLobby) {
        output.WriteRawTag(17);
        output.WriteFixed64(SteamidLobby);
      }
      if (HasClaimOwnership) {
        output.WriteRawTag(24);
        output.WriteBool(ClaimOwnership);
      }
      if (HasClaimMembership) {
        output.WriteRawTag(32);
        output.WriteBool(ClaimMembership);
      }
      if (HasVersionNum) {
        output.WriteRawTag(40);
        output.WriteUInt32(VersionNum);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (HasAppId) {
        output.WriteRawTag(8);
        output.WriteUInt32(AppId);
      }
      if (HasSteamidLobby) {
        output.WriteRawTag(17);
        output.WriteFixed64(SteamidLobby);
      }
      if (HasClaimOwnership) {
        output.WriteRawTag(24);
        output.WriteBool(ClaimOwnership);
      }
      if (HasClaimMembership) {
        output.WriteRawTag(32);
        output.WriteBool(ClaimMembership);
      }
      if (HasVersionNum) {
        output.WriteRawTag(40);
        output.WriteUInt32(VersionNum);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (HasAppId) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(AppId);
      }
      if (HasSteamidLobby) {
        size += 1 + 8;
      }
      if (HasClaimOwnership) {
        size += 1 + 1;
      }
      if (HasClaimMembership) {
        size += 1 + 1;
      }
      if (HasVersionNum) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(VersionNum);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(LobbyMatchmakingLegacy_GetLobbyStatus_Request other) {
      if (other == null) {
        return;
      }
      if (other.HasAppId) {
        AppId = other.AppId;
      }
      if (other.HasSteamidLobby) {
        SteamidLobby = other.SteamidLobby;
      }
      if (other.HasClaimOwnership) {
        ClaimOwnership = other.ClaimOwnership;
      }
      if (other.HasClaimMembership) {
        ClaimMembership = other.ClaimMembership;
      }
      if (other.HasVersionNum) {
        VersionNum = other.VersionNum;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            AppId = input.ReadUInt32();
            break;
          }
          case 17: {
            SteamidLobby = input.ReadFixed64();
            break;
          }
          case 24: {
            ClaimOwnership = input.ReadBool();
            break;
          }
          case 32: {
            ClaimMembership = input.ReadBool();
            break;
          }
          case 40: {
            VersionNum = input.ReadUInt32();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            AppId = input.ReadUInt32();
            break;
          }
          case 17: {
            SteamidLobby = input.ReadFixed64();
            break;
          }
          case 24: {
            ClaimOwnership = input.ReadBool();
            break;
          }
          case 32: {
            ClaimMembership = input.ReadBool();
            break;
          }
          case 40: {
            VersionNum = input.ReadUInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class LobbyMatchmakingLegacy_GetLobbyStatus_Response : pb::IMessage<LobbyMatchmakingLegacy_GetLobbyStatus_Response>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<LobbyMatchmakingLegacy_GetLobbyStatus_Response> _parser = new pb::MessageParser<LobbyMatchmakingLegacy_GetLobbyStatus_Response>(() => new LobbyMatchmakingLegacy_GetLobbyStatus_Response());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<LobbyMatchmakingLegacy_GetLobbyStatus_Response> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Steam.Messages.LobbyMatchmaking.SteammessagesLobbymatchmakingSteamclientReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public LobbyMatchmakingLegacy_GetLobbyStatus_Response() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public LobbyMatchmakingLegacy_GetLobbyStatus_Response(LobbyMatchmakingLegacy_GetLobbyStatus_Response other) : this() {
      _hasBits0 = other._hasBits0;
      appId_ = other.appId_;
      steamidLobby_ = other.steamidLobby_;
      lobbyStatus_ = other.lobbyStatus_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public LobbyMatchmakingLegacy_GetLobbyStatus_Response Clone() {
      return new LobbyMatchmakingLegacy_GetLobbyStatus_Response(this);
    }

    /// <summary>Field number for the "app_id" field.</summary>
    public const int AppIdFieldNumber = 1;
    private readonly static uint AppIdDefaultValue = 0;

    private uint appId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint AppId {
      get { if ((_hasBits0 & 1) != 0) { return appId_; } else { return AppIdDefaultValue; } }
      set {
        _hasBits0 |= 1;
        appId_ = value;
      }
    }
    /// <summary>Gets whether the "app_id" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasAppId {
      get { return (_hasBits0 & 1) != 0; }
    }
    /// <summary>Clears the value of the "app_id" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearAppId() {
      _hasBits0 &= ~1;
    }

    /// <summary>Field number for the "steamid_lobby" field.</summary>
    public const int SteamidLobbyFieldNumber = 2;
    private readonly static ulong SteamidLobbyDefaultValue = 0UL;

    private ulong steamidLobby_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ulong SteamidLobby {
      get { if ((_hasBits0 & 2) != 0) { return steamidLobby_; } else { return SteamidLobbyDefaultValue; } }
      set {
        _hasBits0 |= 2;
        steamidLobby_ = value;
      }
    }
    /// <summary>Gets whether the "steamid_lobby" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasSteamidLobby {
      get { return (_hasBits0 & 2) != 0; }
    }
    /// <summary>Clears the value of the "steamid_lobby" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearSteamidLobby() {
      _hasBits0 &= ~2;
    }

    /// <summary>Field number for the "lobby_status" field.</summary>
    public const int LobbyStatusFieldNumber = 3;
    private readonly static global::Steam.Messages.LobbyMatchmaking.ELobbyStatus LobbyStatusDefaultValue = global::Steam.Messages.LobbyMatchmaking.ELobbyStatus.KElobbyStatusInvalid;

    private global::Steam.Messages.LobbyMatchmaking.ELobbyStatus lobbyStatus_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Steam.Messages.LobbyMatchmaking.ELobbyStatus LobbyStatus {
      get { if ((_hasBits0 & 4) != 0) { return lobbyStatus_; } else { return LobbyStatusDefaultValue; } }
      set {
        _hasBits0 |= 4;
        lobbyStatus_ = value;
      }
    }
    /// <summary>Gets whether the "lobby_status" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasLobbyStatus {
      get { return (_hasBits0 & 4) != 0; }
    }
    /// <summary>Clears the value of the "lobby_status" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearLobbyStatus() {
      _hasBits0 &= ~4;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as LobbyMatchmakingLegacy_GetLobbyStatus_Response);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(LobbyMatchmakingLegacy_GetLobbyStatus_Response other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (AppId != other.AppId) return false;
      if (SteamidLobby != other.SteamidLobby) return false;
      if (LobbyStatus != other.LobbyStatus) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (HasAppId) hash ^= AppId.GetHashCode();
      if (HasSteamidLobby) hash ^= SteamidLobby.GetHashCode();
      if (HasLobbyStatus) hash ^= LobbyStatus.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (HasAppId) {
        output.WriteRawTag(8);
        output.WriteUInt32(AppId);
      }
      if (HasSteamidLobby) {
        output.WriteRawTag(17);
        output.WriteFixed64(SteamidLobby);
      }
      if (HasLobbyStatus) {
        output.WriteRawTag(24);
        output.WriteEnum((int) LobbyStatus);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (HasAppId) {
        output.WriteRawTag(8);
        output.WriteUInt32(AppId);
      }
      if (HasSteamidLobby) {
        output.WriteRawTag(17);
        output.WriteFixed64(SteamidLobby);
      }
      if (HasLobbyStatus) {
        output.WriteRawTag(24);
        output.WriteEnum((int) LobbyStatus);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (HasAppId) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(AppId);
      }
      if (HasSteamidLobby) {
        size += 1 + 8;
      }
      if (HasLobbyStatus) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) LobbyStatus);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(LobbyMatchmakingLegacy_GetLobbyStatus_Response other) {
      if (other == null) {
        return;
      }
      if (other.HasAppId) {
        AppId = other.AppId;
      }
      if (other.HasSteamidLobby) {
        SteamidLobby = other.SteamidLobby;
      }
      if (other.HasLobbyStatus) {
        LobbyStatus = other.LobbyStatus;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            AppId = input.ReadUInt32();
            break;
          }
          case 17: {
            SteamidLobby = input.ReadFixed64();
            break;
          }
          case 24: {
            LobbyStatus = (global::Steam.Messages.LobbyMatchmaking.ELobbyStatus) input.ReadEnum();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            AppId = input.ReadUInt32();
            break;
          }
          case 17: {
            SteamidLobby = input.ReadFixed64();
            break;
          }
          case 24: {
            LobbyStatus = (global::Steam.Messages.LobbyMatchmaking.ELobbyStatus) input.ReadEnum();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
