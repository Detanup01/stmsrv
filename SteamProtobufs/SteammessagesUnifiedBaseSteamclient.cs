// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: steammessages_unified_base.steamclient.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Steam.Messages.Unified.Base {

  /// <summary>Holder for reflection information generated from steammessages_unified_base.steamclient.proto</summary>
  public static partial class SteammessagesUnifiedBaseSteamclientReflection {

    #region Descriptor
    /// <summary>File descriptor for steammessages_unified_base.steamclient.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static SteammessagesUnifiedBaseSteamclientReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CixzdGVhbW1lc3NhZ2VzX3VuaWZpZWRfYmFzZS5zdGVhbWNsaWVudC5wcm90",
            "bxogZ29vZ2xlL3Byb3RvYnVmL2Rlc2NyaXB0b3IucHJvdG8iDAoKTm9SZXNw",
            "b25zZSpdChNFUHJvdG9FeGVjdXRpb25TaXRlEiAKHGtfRVByb3RvRXhlY3V0",
            "aW9uU2l0ZVVua25vd24QABIkCiBrX0VQcm90b0V4ZWN1dGlvblNpdGVTdGVh",
            "bUNsaWVudBACOjQKC2Rlc2NyaXB0aW9uEh0uZ29vZ2xlLnByb3RvYnVmLkZp",
            "ZWxkT3B0aW9ucxjQhgMgASgJOj4KE3NlcnZpY2VfZGVzY3JpcHRpb24SHy5n",
            "b29nbGUucHJvdG9idWYuU2VydmljZU9wdGlvbnMY0IYDIAEoCTp1ChZzZXJ2",
            "aWNlX2V4ZWN1dGlvbl9zaXRlEh8uZ29vZ2xlLnByb3RvYnVmLlNlcnZpY2VP",
            "cHRpb25zGNiGAyABKA4yFC5FUHJvdG9FeGVjdXRpb25TaXRlOhxrX0VQcm90",
            "b0V4ZWN1dGlvblNpdGVVbmtub3duOjwKEm1ldGhvZF9kZXNjcmlwdGlvbhIe",
            "Lmdvb2dsZS5wcm90b2J1Zi5NZXRob2RPcHRpb25zGNCGAyABKAk6OAoQZW51",
            "bV9kZXNjcmlwdGlvbhIcLmdvb2dsZS5wcm90b2J1Zi5FbnVtT3B0aW9ucxjQ",
            "hgMgASgJOkMKFmVudW1fdmFsdWVfZGVzY3JpcHRpb24SIS5nb29nbGUucHJv",
            "dG9idWYuRW51bVZhbHVlT3B0aW9ucxjQhgMgASgJQiNIAYABAKoCG1N0ZWFt",
            "Lk1lc3NhZ2VzLlVuaWZpZWQuQmFzZQ=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.Reflection.DescriptorReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Steam.Messages.Unified.Base.EProtoExecutionSite), }, new pb::Extension[] { SteammessagesUnifiedBaseSteamclientExtensions.Description, SteammessagesUnifiedBaseSteamclientExtensions.ServiceDescription, SteammessagesUnifiedBaseSteamclientExtensions.ServiceExecutionSite, SteammessagesUnifiedBaseSteamclientExtensions.MethodDescription, SteammessagesUnifiedBaseSteamclientExtensions.EnumDescription, SteammessagesUnifiedBaseSteamclientExtensions.EnumValueDescription }, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Steam.Messages.Unified.Base.NoResponse), global::Steam.Messages.Unified.Base.NoResponse.Parser, null, null, null, null, null)
          }));
    }
    #endregion

  }
  /// <summary>Holder for extension identifiers generated from the top level of steammessages_unified_base.steamclient.proto</summary>
  public static partial class SteammessagesUnifiedBaseSteamclientExtensions {
    public static readonly pb::Extension<global::Google.Protobuf.Reflection.FieldOptions, string> Description =
      new pb::Extension<global::Google.Protobuf.Reflection.FieldOptions, string>(50000, pb::FieldCodec.ForString(400002, ""));
    public static readonly pb::Extension<global::Google.Protobuf.Reflection.ServiceOptions, string> ServiceDescription =
      new pb::Extension<global::Google.Protobuf.Reflection.ServiceOptions, string>(50000, pb::FieldCodec.ForString(400002, ""));
    public static readonly pb::Extension<global::Google.Protobuf.Reflection.ServiceOptions, global::Steam.Messages.Unified.Base.EProtoExecutionSite> ServiceExecutionSite =
      new pb::Extension<global::Google.Protobuf.Reflection.ServiceOptions, global::Steam.Messages.Unified.Base.EProtoExecutionSite>(50008, pb::FieldCodec.ForEnum(400064, x => (int) x, x => (global::Steam.Messages.Unified.Base.EProtoExecutionSite) x, global::Steam.Messages.Unified.Base.EProtoExecutionSite.KEprotoExecutionSiteUnknown));
    public static readonly pb::Extension<global::Google.Protobuf.Reflection.MethodOptions, string> MethodDescription =
      new pb::Extension<global::Google.Protobuf.Reflection.MethodOptions, string>(50000, pb::FieldCodec.ForString(400002, ""));
    public static readonly pb::Extension<global::Google.Protobuf.Reflection.EnumOptions, string> EnumDescription =
      new pb::Extension<global::Google.Protobuf.Reflection.EnumOptions, string>(50000, pb::FieldCodec.ForString(400002, ""));
    public static readonly pb::Extension<global::Google.Protobuf.Reflection.EnumValueOptions, string> EnumValueDescription =
      new pb::Extension<global::Google.Protobuf.Reflection.EnumValueOptions, string>(50000, pb::FieldCodec.ForString(400002, ""));
  }

  #region Enums
  public enum EProtoExecutionSite {
    [pbr::OriginalName("k_EProtoExecutionSiteUnknown")] KEprotoExecutionSiteUnknown = 0,
    [pbr::OriginalName("k_EProtoExecutionSiteSteamClient")] KEprotoExecutionSiteSteamClient = 2,
  }

  #endregion

  #region Messages
  public sealed partial class NoResponse : pb::IMessage<NoResponse>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<NoResponse> _parser = new pb::MessageParser<NoResponse>(() => new NoResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<NoResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Steam.Messages.Unified.Base.SteammessagesUnifiedBaseSteamclientReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public NoResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public NoResponse(NoResponse other) : this() {
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public NoResponse Clone() {
      return new NoResponse(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as NoResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(NoResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
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
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(NoResponse other) {
      if (other == null) {
        return;
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
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
