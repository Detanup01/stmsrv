// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: steammessages_gamenetworking.steamclient.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Steam.Messages.GameNetworking {

  /// <summary>Holder for reflection information generated from steammessages_gamenetworking.steamclient.proto</summary>
  public static partial class SteammessagesGamenetworkingSteamclientReflection {

    #region Descriptor
    /// <summary>File descriptor for steammessages_gamenetworking.steamclient.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static SteammessagesGamenetworkingSteamclientReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ci5zdGVhbW1lc3NhZ2VzX2dhbWVuZXR3b3JraW5nLnN0ZWFtY2xpZW50LnBy",
            "b3RvGhhzdGVhbW1lc3NhZ2VzX2Jhc2UucHJvdG8aLHN0ZWFtbWVzc2FnZXNf",
            "dW5pZmllZF9iYXNlLnN0ZWFtY2xpZW50LnByb3RvIlAKJkNHYW1lTmV0d29y",
            "a2luZ19BbGxvY2F0ZUZha2VJUF9SZXF1ZXN0Eg4KBmFwcF9pZBgBIAEoDRIW",
            "Cg5udW1fZmFrZV9wb3J0cxgCIAEoDSJOCidDR2FtZU5ldHdvcmtpbmdfQWxs",
            "b2NhdGVGYWtlSVBfUmVzcG9uc2USDwoHZmFrZV9pcBgBIAEoBxISCgpmYWtl",
            "X3BvcnRzGAIgAygNImEKKkNHYW1lTmV0d29ya2luZ19SZWxlYXNlRmFrZUlQ",
            "X05vdGlmaWNhdGlvbhIOCgZhcHBfaWQYASABKA0SDwoHZmFrZV9pcBgCIAEo",
            "BxISCgpmYWtlX3BvcnRzGAMgAygNMtICCg5HYW1lTmV0d29ya2luZxKMAQoO",
            "QWxsb2NhdGVGYWtlSVASJy5DR2FtZU5ldHdvcmtpbmdfQWxsb2NhdGVGYWtl",
            "SVBfUmVxdWVzdBooLkNHYW1lTmV0d29ya2luZ19BbGxvY2F0ZUZha2VJUF9S",
            "ZXNwb25zZSIngrUYI0NsaWVudCBpcyBhc2tpbmcgdG8gbGVhc2UgYSBGYWtl",
            "SVAuEoYBChNOb3RpZnlSZWxlYXNlRmFrZUlQEisuQ0dhbWVOZXR3b3JraW5n",
            "X1JlbGVhc2VGYWtlSVBfTm90aWZpY2F0aW9uGgsuTm9SZXNwb25zZSI1grUY",
            "MUNsaWVudCBpbmZvcm1zIHNlcnZlciBpdCBpcyBkb25lIHdpdGggdGhlIEZh",
            "a2VJUC4aKIK1GCRTZXJ2aWNlcyB0aGF0IHN1cHBvcnQgUDJQIG5ldHdvcmtp",
            "bmdCI4ABAaoCHVN0ZWFtLk1lc3NhZ2VzLkdhbWVOZXR3b3JraW5n"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Steam.Messages.Base.SteammessagesBaseReflection.Descriptor, global::Steam.Messages.Unified.Base.SteammessagesUnifiedBaseSteamclientReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Steam.Messages.GameNetworking.CGameNetworking_AllocateFakeIP_Request), global::Steam.Messages.GameNetworking.CGameNetworking_AllocateFakeIP_Request.Parser, new[]{ "AppId", "NumFakePorts" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Steam.Messages.GameNetworking.CGameNetworking_AllocateFakeIP_Response), global::Steam.Messages.GameNetworking.CGameNetworking_AllocateFakeIP_Response.Parser, new[]{ "FakeIp", "FakePorts" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Steam.Messages.GameNetworking.CGameNetworking_ReleaseFakeIP_Notification), global::Steam.Messages.GameNetworking.CGameNetworking_ReleaseFakeIP_Notification.Parser, new[]{ "AppId", "FakeIp", "FakePorts" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class CGameNetworking_AllocateFakeIP_Request : pb::IMessage<CGameNetworking_AllocateFakeIP_Request>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<CGameNetworking_AllocateFakeIP_Request> _parser = new pb::MessageParser<CGameNetworking_AllocateFakeIP_Request>(() => new CGameNetworking_AllocateFakeIP_Request());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<CGameNetworking_AllocateFakeIP_Request> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Steam.Messages.GameNetworking.SteammessagesGamenetworkingSteamclientReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CGameNetworking_AllocateFakeIP_Request() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CGameNetworking_AllocateFakeIP_Request(CGameNetworking_AllocateFakeIP_Request other) : this() {
      _hasBits0 = other._hasBits0;
      appId_ = other.appId_;
      numFakePorts_ = other.numFakePorts_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CGameNetworking_AllocateFakeIP_Request Clone() {
      return new CGameNetworking_AllocateFakeIP_Request(this);
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

    /// <summary>Field number for the "num_fake_ports" field.</summary>
    public const int NumFakePortsFieldNumber = 2;
    private readonly static uint NumFakePortsDefaultValue = 0;

    private uint numFakePorts_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint NumFakePorts {
      get { if ((_hasBits0 & 2) != 0) { return numFakePorts_; } else { return NumFakePortsDefaultValue; } }
      set {
        _hasBits0 |= 2;
        numFakePorts_ = value;
      }
    }
    /// <summary>Gets whether the "num_fake_ports" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasNumFakePorts {
      get { return (_hasBits0 & 2) != 0; }
    }
    /// <summary>Clears the value of the "num_fake_ports" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearNumFakePorts() {
      _hasBits0 &= ~2;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as CGameNetworking_AllocateFakeIP_Request);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(CGameNetworking_AllocateFakeIP_Request other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (AppId != other.AppId) return false;
      if (NumFakePorts != other.NumFakePorts) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (HasAppId) hash ^= AppId.GetHashCode();
      if (HasNumFakePorts) hash ^= NumFakePorts.GetHashCode();
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
      if (HasNumFakePorts) {
        output.WriteRawTag(16);
        output.WriteUInt32(NumFakePorts);
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
      if (HasNumFakePorts) {
        output.WriteRawTag(16);
        output.WriteUInt32(NumFakePorts);
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
      if (HasNumFakePorts) {
        size += 1 + pb::CodedOutputStream.ComputeUInt32Size(NumFakePorts);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(CGameNetworking_AllocateFakeIP_Request other) {
      if (other == null) {
        return;
      }
      if (other.HasAppId) {
        AppId = other.AppId;
      }
      if (other.HasNumFakePorts) {
        NumFakePorts = other.NumFakePorts;
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
          case 16: {
            NumFakePorts = input.ReadUInt32();
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
          case 16: {
            NumFakePorts = input.ReadUInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class CGameNetworking_AllocateFakeIP_Response : pb::IMessage<CGameNetworking_AllocateFakeIP_Response>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<CGameNetworking_AllocateFakeIP_Response> _parser = new pb::MessageParser<CGameNetworking_AllocateFakeIP_Response>(() => new CGameNetworking_AllocateFakeIP_Response());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<CGameNetworking_AllocateFakeIP_Response> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Steam.Messages.GameNetworking.SteammessagesGamenetworkingSteamclientReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CGameNetworking_AllocateFakeIP_Response() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CGameNetworking_AllocateFakeIP_Response(CGameNetworking_AllocateFakeIP_Response other) : this() {
      _hasBits0 = other._hasBits0;
      fakeIp_ = other.fakeIp_;
      fakePorts_ = other.fakePorts_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CGameNetworking_AllocateFakeIP_Response Clone() {
      return new CGameNetworking_AllocateFakeIP_Response(this);
    }

    /// <summary>Field number for the "fake_ip" field.</summary>
    public const int FakeIpFieldNumber = 1;
    private readonly static uint FakeIpDefaultValue = 0;

    private uint fakeIp_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint FakeIp {
      get { if ((_hasBits0 & 1) != 0) { return fakeIp_; } else { return FakeIpDefaultValue; } }
      set {
        _hasBits0 |= 1;
        fakeIp_ = value;
      }
    }
    /// <summary>Gets whether the "fake_ip" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasFakeIp {
      get { return (_hasBits0 & 1) != 0; }
    }
    /// <summary>Clears the value of the "fake_ip" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearFakeIp() {
      _hasBits0 &= ~1;
    }

    /// <summary>Field number for the "fake_ports" field.</summary>
    public const int FakePortsFieldNumber = 2;
    private static readonly pb::FieldCodec<uint> _repeated_fakePorts_codec
        = pb::FieldCodec.ForUInt32(16);
    private readonly pbc::RepeatedField<uint> fakePorts_ = new pbc::RepeatedField<uint>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<uint> FakePorts {
      get { return fakePorts_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as CGameNetworking_AllocateFakeIP_Response);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(CGameNetworking_AllocateFakeIP_Response other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (FakeIp != other.FakeIp) return false;
      if(!fakePorts_.Equals(other.fakePorts_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (HasFakeIp) hash ^= FakeIp.GetHashCode();
      hash ^= fakePorts_.GetHashCode();
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
      if (HasFakeIp) {
        output.WriteRawTag(13);
        output.WriteFixed32(FakeIp);
      }
      fakePorts_.WriteTo(output, _repeated_fakePorts_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (HasFakeIp) {
        output.WriteRawTag(13);
        output.WriteFixed32(FakeIp);
      }
      fakePorts_.WriteTo(ref output, _repeated_fakePorts_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (HasFakeIp) {
        size += 1 + 4;
      }
      size += fakePorts_.CalculateSize(_repeated_fakePorts_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(CGameNetworking_AllocateFakeIP_Response other) {
      if (other == null) {
        return;
      }
      if (other.HasFakeIp) {
        FakeIp = other.FakeIp;
      }
      fakePorts_.Add(other.fakePorts_);
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
          case 13: {
            FakeIp = input.ReadFixed32();
            break;
          }
          case 18:
          case 16: {
            fakePorts_.AddEntriesFrom(input, _repeated_fakePorts_codec);
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
          case 13: {
            FakeIp = input.ReadFixed32();
            break;
          }
          case 18:
          case 16: {
            fakePorts_.AddEntriesFrom(ref input, _repeated_fakePorts_codec);
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class CGameNetworking_ReleaseFakeIP_Notification : pb::IMessage<CGameNetworking_ReleaseFakeIP_Notification>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<CGameNetworking_ReleaseFakeIP_Notification> _parser = new pb::MessageParser<CGameNetworking_ReleaseFakeIP_Notification>(() => new CGameNetworking_ReleaseFakeIP_Notification());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<CGameNetworking_ReleaseFakeIP_Notification> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Steam.Messages.GameNetworking.SteammessagesGamenetworkingSteamclientReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CGameNetworking_ReleaseFakeIP_Notification() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CGameNetworking_ReleaseFakeIP_Notification(CGameNetworking_ReleaseFakeIP_Notification other) : this() {
      _hasBits0 = other._hasBits0;
      appId_ = other.appId_;
      fakeIp_ = other.fakeIp_;
      fakePorts_ = other.fakePorts_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CGameNetworking_ReleaseFakeIP_Notification Clone() {
      return new CGameNetworking_ReleaseFakeIP_Notification(this);
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

    /// <summary>Field number for the "fake_ip" field.</summary>
    public const int FakeIpFieldNumber = 2;
    private readonly static uint FakeIpDefaultValue = 0;

    private uint fakeIp_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint FakeIp {
      get { if ((_hasBits0 & 2) != 0) { return fakeIp_; } else { return FakeIpDefaultValue; } }
      set {
        _hasBits0 |= 2;
        fakeIp_ = value;
      }
    }
    /// <summary>Gets whether the "fake_ip" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasFakeIp {
      get { return (_hasBits0 & 2) != 0; }
    }
    /// <summary>Clears the value of the "fake_ip" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearFakeIp() {
      _hasBits0 &= ~2;
    }

    /// <summary>Field number for the "fake_ports" field.</summary>
    public const int FakePortsFieldNumber = 3;
    private static readonly pb::FieldCodec<uint> _repeated_fakePorts_codec
        = pb::FieldCodec.ForUInt32(24);
    private readonly pbc::RepeatedField<uint> fakePorts_ = new pbc::RepeatedField<uint>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<uint> FakePorts {
      get { return fakePorts_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as CGameNetworking_ReleaseFakeIP_Notification);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(CGameNetworking_ReleaseFakeIP_Notification other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (AppId != other.AppId) return false;
      if (FakeIp != other.FakeIp) return false;
      if(!fakePorts_.Equals(other.fakePorts_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (HasAppId) hash ^= AppId.GetHashCode();
      if (HasFakeIp) hash ^= FakeIp.GetHashCode();
      hash ^= fakePorts_.GetHashCode();
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
      if (HasFakeIp) {
        output.WriteRawTag(21);
        output.WriteFixed32(FakeIp);
      }
      fakePorts_.WriteTo(output, _repeated_fakePorts_codec);
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
      if (HasFakeIp) {
        output.WriteRawTag(21);
        output.WriteFixed32(FakeIp);
      }
      fakePorts_.WriteTo(ref output, _repeated_fakePorts_codec);
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
      if (HasFakeIp) {
        size += 1 + 4;
      }
      size += fakePorts_.CalculateSize(_repeated_fakePorts_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(CGameNetworking_ReleaseFakeIP_Notification other) {
      if (other == null) {
        return;
      }
      if (other.HasAppId) {
        AppId = other.AppId;
      }
      if (other.HasFakeIp) {
        FakeIp = other.FakeIp;
      }
      fakePorts_.Add(other.fakePorts_);
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
          case 21: {
            FakeIp = input.ReadFixed32();
            break;
          }
          case 26:
          case 24: {
            fakePorts_.AddEntriesFrom(input, _repeated_fakePorts_codec);
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
          case 21: {
            FakeIp = input.ReadFixed32();
            break;
          }
          case 26:
          case 24: {
            fakePorts_.AddEntriesFrom(ref input, _repeated_fakePorts_codec);
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
