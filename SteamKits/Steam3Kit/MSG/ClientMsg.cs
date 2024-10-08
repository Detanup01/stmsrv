﻿using Google.Protobuf;
using Steam.Messages.Base;
using Steam3Kit.Types;
using System.Diagnostics.CodeAnalysis;

namespace Steam3Kit.MSG
{
    /// <summary>
    /// Represents a protobuf backed client message. Only contains the header information.
    /// </summary>
    public class ClientMsgProtobuf : MsgBase<MsgHdrProtoBuf>
    {
        internal ClientMsgProtobuf(int payloadReserve = 0)
            : base(payloadReserve)
        {
        }

        /// <summary>
        /// Gets a value indicating whether this client message is protobuf backed.
        /// Client messages of this type are always protobuf backed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is protobuf backed; otherwise, <c>false</c>.
        /// </value>
        public override bool IsProto => true;
        /// <summary>
        /// Gets the network message type of this client message.
        /// </summary>
        /// <value>
        /// The network message type.
        /// </value>
        public override EMsg MsgType => Header.Msg;

        /// <summary>
        /// Gets or sets the session id for this client message.
        /// </summary>
        /// <value>
        /// The session id.
        /// </value>
        public override int SessionID
        {
            get => ProtoHeader.ClientSessionid;
            set => ProtoHeader.ClientSessionid = value;
        }
        /// <summary>
        /// Gets or sets the <see cref="SteamID"/> for this client message.
        /// </summary>
        /// <value>
        /// The <see cref="SteamID"/>.
        /// </value>
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        [DisallowNull, NotNull]
        public override SteamID? SteamID
        {
            get => ProtoHeader.Steamid;
            set => ProtoHeader.Steamid = value ?? throw new ArgumentNullException(nameof(value));
        }
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

        /// <summary>
        /// Gets or sets the target job id for this client message.
        /// </summary>
        /// <value>
        /// The target job id.
        /// </value>
        public override JobID TargetJobID
        {
            get => ProtoHeader.JobidTarget;
            set => ProtoHeader.JobidTarget = value ?? throw new ArgumentNullException(nameof(value));
        }
        /// <summary>
        /// Gets or sets the source job id for this client message.
        /// </summary>
        /// <value>
        /// The source job id.
        /// </value>
        public override JobID SourceJobID
        {
            get => ProtoHeader.JobidSource;
            set => ProtoHeader.JobidSource = value ?? throw new ArgumentNullException(nameof(value));
        }


        /// <summary>
        /// Shorthand accessor for the protobuf header.
        /// </summary>
        public CMsgProtoBufHeader ProtoHeader => Header.Proto;


        internal ClientMsgProtobuf(EMsg eMsg, int payloadReserve = 64)
            : base(payloadReserve)
        {
            // set our emsg
            Header.Msg = eMsg;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientMsgProtobuf"/> class.
        /// This is a recieve constructor.
        /// </summary>
        /// <param name="msg">The packet message to build this client message from.</param>
        public ClientMsgProtobuf(IPacketMsg msg)
            : this(msg.GetMsgTypeWithNullCheck(nameof(msg)))
        {
            if (!(msg is PacketClientMsgProtobuf packetMsgProto))
            {
                throw new InvalidDataException("ClientMsgProtobuf used for non-proto message!");
            }

            Header = packetMsgProto.Header;
        }


        /// <summary>
        /// Serializes this client message instance to a byte array.
        /// </summary>
        /// <exception cref="NotSupportedException">This class is for reading Protobuf messages only. If you want to create a protobuf message, use <see cref="ClientMsgProtobuf&lt;T&gt;"/>.</exception>
        public override byte[] Serialize()
        {
            throw new NotSupportedException("ClientMsgProtobuf is for reading only. Use ClientMsgProtobuf<T> for serializing messages.");
        }
    }

    /// <summary>
    /// Represents a protobuf backed client message.
    /// </summary>
    /// <typeparam name="TBody">The body type of this message.</typeparam>
    public sealed class ClientMsgProtobuf<TBody> : ClientMsgProtobuf
        where TBody : IMessage<TBody>, new()
    {
        private TBody _body;

        /// <summary>
        /// Gets the body structure of this message.
        /// </summary>
        public TBody Body
        {
            get => _body;
            set => _body = value ?? throw new ArgumentNullException(nameof(value));
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ClientMsgProtobuf&lt;BodyType&gt;"/> class.
        /// This is a client send constructor.
        /// </summary>
        /// <param name="eMsg">The network message type this client message represents.</param>
        /// <param name="payloadReserve">The number of bytes to initialize the payload capacity to.</param>
        public ClientMsgProtobuf(EMsg eMsg, int payloadReserve = 64)
            : base(payloadReserve)
        {
            _body = new TBody();

            // set our emsg
            Header.Msg = eMsg;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientMsgProtobuf&lt;BodyType&gt;"/> class.
        /// This a reply constructor.
        /// </summary>
        /// <param name="eMsg">The network message type this client message represents.</param>
        /// <param name="msg">The message that this instance is a reply for.</param>
        /// <param name="payloadReserve">The number of bytes to initialize the payload capacity to.</param>
        public ClientMsgProtobuf(EMsg eMsg, MsgBase<MsgHdrProtoBuf> msg, int payloadReserve = 64)
            : this(eMsg, payloadReserve)
        {
            // our target is where the message came from
            Header.Proto.JobidTarget = msg.Header.Proto.JobidSource;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientMsgProtobuf&lt;BodyType&gt;"/> class.
        /// This is a recieve constructor.
        /// </summary>
        /// <param name="msg">The packet message to build this client message from.</param>
        public ClientMsgProtobuf(IPacketMsg msg)
            : this(msg.GetMsgTypeWithNullCheck(nameof(msg)))
        {
            if (!(msg is PacketClientMsgProtobuf packetMsg))
            {
                throw new InvalidDataException($"ClientMsgProtobuf<{typeof(TBody).FullName}> used for non-proto message!");
            }

            Header = packetMsg.Header;

            var data = packetMsg.GetData();
            var offset = (int)packetMsg.BodyOffset;
            using MemoryStream ms = new MemoryStream(data, offset, data.Length - offset);
            Body.MergeFrom(ms);
            // the rest of the data is the payload
            int payloadLen = (int)(ms.Length - ms.Position);

            if (payloadLen > 0)
            {
                ms.CopyTo(Payload, payloadLen);
                Payload.Seek(0, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// Serializes this client message instance to a byte array.
        /// </summary>
        /// <returns>
        /// Data representing a client message.
        /// </returns>
        public override byte[] Serialize()
        {
            using MemoryStream ms = new MemoryStream();
            Header.Serialize(ms); 
            Body.WriteTo(ms);
            Payload.WriteTo(ms);

            return ms.ToArray();
        }
    }

    /// <summary>
    /// Represents a struct backed client message.
    /// </summary>
    /// <typeparam name="TBody">The body type of this message.</typeparam>
    public sealed class ClientMsg<TBody> : MsgBase<ExtendedClientMsgHdr>
        where TBody : ISteamSerializableMessage, new()
    {
        /// <summary>
        /// Gets a value indicating whether this client message is protobuf backed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is protobuf backed; otherwise, <c>false</c>.
        /// </value>
        public override bool IsProto => false;
        /// <summary>
        /// Gets the network message type of this client message.
        /// </summary>
        /// <value>
        /// The network message type.
        /// </value>
        public override EMsg MsgType => Header.Msg;

        /// <summary>
        /// Gets or sets the session id for this client message.
        /// </summary>
        /// <value>
        /// The session id.
        /// </value>
        public override int SessionID
        {
            get => Header.SessionID;
            set => Header.SessionID = value;
        }
        /// <summary>
        /// Gets or sets the <see cref="SteamID"/> for this client message.
        /// </summary>
        /// <value>
        /// The <see cref="SteamID"/>.
        /// </value>
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
        [DisallowNull, NotNull]
        public override SteamID? SteamID
        {
            get => Header.SteamID;
            set => Header.SteamID = value ?? throw new ArgumentNullException(nameof(value));
        }
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).

        /// <summary>
        /// Gets or sets the target job id for this client message.
        /// </summary>
        /// <value>
        /// The target job id.
        /// </value>
        public override JobID TargetJobID
        {
            get => Header.TargetJobID;
            set => Header.TargetJobID = value ?? throw new ArgumentNullException(nameof(value));
        }
        /// <summary>
        /// Gets or sets the source job id for this client message.
        /// </summary>
        /// <value>
        /// The source job id.
        /// </value>
        public override JobID SourceJobID
        {
            get => Header.SourceJobID;
            set => Header.SourceJobID = value ?? throw new ArgumentNullException(nameof(value));
        }


        /// <summary>
        /// Gets the body structure of this message.
        /// </summary>
        public TBody Body { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ClientMsg&lt;BodyType&gt;"/> class.
        /// This is a client send constructor.
        /// </summary>
        /// <param name="payloadReserve">The number of bytes to initialize the payload capacity to.</param>
        public ClientMsg(int payloadReserve = 64)
            : base(payloadReserve)
        {
            Body = new TBody();

            // assign our emsg
            Header.SetEMsg(Body.GetEMsg());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientMsg&lt;BodyType&gt;"/> class.
        /// This a reply constructor.
        /// </summary>
        /// <param name="msg">The message that this instance is a reply for.</param>
        /// <param name="payloadReserve">The number of bytes to initialize the payload capacity to.</param>
        public ClientMsg(MsgBase<ExtendedClientMsgHdr> msg, int payloadReserve = 64)
            : this(payloadReserve)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            // our target is where the message came from
            Header.TargetJobID = msg.Header.SourceJobID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientMsg&lt;BodyType&gt;"/> class.
        /// This is a recieve constructor.
        /// </summary>
        /// <param name="msg">The packet message to build this client message from.</param>
        public ClientMsg(IPacketMsg msg)
            : this()
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            if (!(msg is PacketClientMsg packetMsg))
            {
                throw new InvalidDataException($"ClientMsg<{typeof(TBody).FullName}> used for proto message!");
            }

            Header = packetMsg.Header;

            var data = packetMsg.GetData();
            var offset = (int)packetMsg.BodyOffset;
            using MemoryStream ms = new MemoryStream(data, offset, data.Length - offset);

            Body.Deserialize(ms);

            // the rest of the data is the payload
            int payloadLen = (int)(ms.Length - ms.Position);

            if (payloadLen > 0)
            {
                ms.CopyTo(Payload, payloadLen);
                Payload.Seek(0, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// Serializes this client message instance to a byte array.
        /// </summary>
        /// <returns>
        /// Data representing a client message.
        /// </returns>
        public override byte[] Serialize()
        {
            using MemoryStream ms = new MemoryStream();
            Header.Serialize(ms);
            Body.Serialize(ms);
            Payload.WriteTo(ms);

            return ms.ToArray();
        }
    }

    /// <summary>
    /// Represents a struct backed message without session or client info.
    /// </summary>
    /// <typeparam name="TBody">The body type of this message.</typeparam>
    public sealed class Msg<TBody> : MsgBase<MsgHdr>
        where TBody : ISteamSerializableMessage, new()
    {
        /// <summary>
        /// Gets a value indicating whether this client message is protobuf backed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is protobuf backed; otherwise, <c>false</c>.
        /// </value>
        public override bool IsProto => false;
        /// <summary>
        /// Gets the network message type of this client message.
        /// </summary>
        /// <value>
        /// The network message type.
        /// </value>
        public override EMsg MsgType => Header.Msg;

        /// <summary>
        /// Gets or sets the session id for this client message.
        /// This type of client message does not support session ids
        /// </summary>
        /// <value>
        /// The session id.
        /// </value>
        public override int SessionID { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="SteamID"/> for this client message.
        /// This type of client message goes not support <see cref="SteamID">SteamIDs</see>.
        /// </summary>
        /// <value>
        /// The <see cref="SteamID"/>.
        /// </value>
        public override SteamID? SteamID { get; set; }

        /// <summary>
        /// Gets or sets the target job id for this client message.
        /// </summary>
        /// <value>
        /// The target job id.
        /// </value>
        public override JobID TargetJobID
        {
            get => Header.TargetJobID;
            set => Header.TargetJobID = value ?? throw new ArgumentNullException(nameof(value));
        }
        /// <summary>
        /// Gets or sets the source job id for this client message.
        /// </summary>
        /// <value>
        /// The source job id.
        /// </value>
        public override JobID SourceJobID
        {
            get => Header.SourceJobID;
            set => Header.SourceJobID = value ?? throw new ArgumentNullException(nameof(value));
        }


        /// <summary>
        /// Gets the structure body of the message.
        /// </summary>
        public TBody Body { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Msg&lt;BodyType&gt;"/> class.
        /// This is a client send constructor.
        /// </summary>
        /// <param name="payloadReserve">The number of bytes to initialize the payload capacity to.</param>
        public Msg(int payloadReserve = 0)
            : base(payloadReserve)
        {
            Body = new TBody();

            // assign our emsg
            Header.SetEMsg(Body.GetEMsg());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Msg&lt;BodyType&gt;"/> class.
        /// This a reply constructor.
        /// </summary>
        /// <param name="msg">The message that this instance is a reply for.</param>
        /// <param name="payloadReserve">The number of bytes to initialize the payload capacity to.</param>
        public Msg(MsgBase<MsgHdr> msg, int payloadReserve = 0)
            : this(payloadReserve)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            // our target is where the message came from
            Header.TargetJobID = msg.Header.SourceJobID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Msg&lt;BodyType&gt;"/> class.
        /// This is a recieve constructor.
        /// </summary>
        /// <param name="msg">The packet message to build this client message from.</param>
        public Msg(IPacketMsg msg)
            : this()
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg));
            }

            if (!(msg is PacketMsg packetMsg))
            {
                throw new InvalidDataException($"ClientMsg<{typeof(TBody).FullName}> used for proto message!");
            }

            Header = packetMsg.Header;

            var data = packetMsg.GetData();
            var offset = (int)packetMsg.BodyOffset;
            using MemoryStream ms = new MemoryStream(data, offset, data.Length - offset);

            Body.Deserialize(ms);

            // the rest of the data is the payload
            int payloadLen = (int)(ms.Length - ms.Position);

            if (payloadLen > 0)
            {
                ms.CopyTo(Payload, payloadLen);
                Payload.Seek(0, SeekOrigin.Begin);
            }
        }


        /// <summary>
        /// Serializes this client message instance to a byte array.
        /// </summary>
        /// <returns>
        /// Data representing a client message.
        /// </returns>
        public override byte[] Serialize()
        {
            using MemoryStream ms = new MemoryStream();
            Header.Serialize(ms);
            Body.Serialize(ms);
            Payload.WriteTo(ms);

            return ms.ToArray();
        }
    }

    // todo rename this
    public sealed class TestClientMsg : ClientMsgProtobuf
    {
        public IMessage Body;


        public TestClientMsg(IMessage body, EMsg eMsg, int payloadReserve = 64)
            : base(payloadReserve)
        {
            Body = body;

            // set our emsg
            Header.Msg = eMsg;
        }


        public T GetBody<T>() where T : IMessage<T>, new()
        {
            return (T)Body;
        }


        /// <summary>
        /// Serializes this client message instance to a byte array.
        /// </summary>
        /// <returns>
        /// Data representing a client message.
        /// </returns>
        public override byte[] Serialize()
        {
            using MemoryStream ms = new MemoryStream();
            Header.Serialize(ms);
            Body.WriteTo(ms);
            Payload.WriteTo(ms);

            return ms.ToArray();
        }
    }
}
