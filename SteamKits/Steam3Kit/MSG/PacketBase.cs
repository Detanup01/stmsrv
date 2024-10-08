﻿using Google.Protobuf;

namespace Steam3Kit.MSG
{
    /// <summary>
    /// Represents a simple unified interface into client messages recieved from the network.
    /// This is contrasted with <see cref="IClientMsg"/> in that this interface is packet body agnostic
    /// and only allows simple access into the header. This interface is also immutable, and the underlying
    /// data cannot be modified.
    /// </summary>
    public interface IPacketMsg
    {
        /// <summary>
        /// Gets a value indicating whether this packet message is protobuf backed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is protobuf backed; otherwise, <c>false</c>.
        /// </value>
        bool IsProto { get; }
        /// <summary>
        /// Gets the network message type of this packet message.
        /// </summary>
        /// <value>
        /// The message type.
        /// </value>
        EMsg MsgType { get; }

        /// <summary>
        /// Gets the target job id for this packet message.
        /// </summary>
        /// <value>
        /// The target job id.
        /// </value>
        ulong TargetJobID { get; }
        /// <summary>
        /// Gets the source job id for this packet message.
        /// </summary>
        /// <value>
        /// The source job id.
        /// </value>
        ulong SourceJobID { get; }

        /// <summary>
        /// Gets the underlying data that represents this client message.
        /// </summary>
        /// <returns>The data.</returns>
        byte[] GetData();
    }

    static class IPacketMsgExtensions
    {
        public static EMsg GetMsgTypeWithNullCheck(this IPacketMsg msg, string name)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(name);
            }

            return msg.MsgType;
        }
    }

    /// <summary>
    /// Represents a protobuf backed packet message.
    /// </summary>
    public sealed class PacketClientMsgProtobuf : IPacketMsg
    {
        /// <summary>
        /// Gets a value indicating whether this packet message is protobuf backed.
        /// This type of message is always protobuf backed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is protobuf backed; otherwise, <c>false</c>.
        /// </value>
        public bool IsProto => true;
        /// <summary>
        /// Gets the network message type of this packet message.
        /// </summary>
        /// <value>
        /// The message type.
        /// </value>
        public EMsg MsgType { get; }

        /// <summary>
        /// Gets the target job id for this packet message.
        /// </summary>
        /// <value>
        /// The target job id.
        /// </value>
        public ulong TargetJobID => Header.Proto.JobidTarget;
        /// <summary>
        /// Gets the source job id for this packet message.
        /// </summary>
        /// <value>
        /// The source job id.
        /// </value>
        public ulong SourceJobID => Header.Proto.JobidSource;
        /// <summary>
        /// Gets the header for this packet message.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public MsgHdrProtoBuf Header;
        /// <summary>
        /// Gets the offset in payload to the body after the header.
        /// </summary>
        /// <value>
        /// The offset in payload after the header.
        /// </value>
        public long BodyOffset;

        byte[] payload;


        /// <summary>
        /// Initializes a new instance of the <see cref="PacketClientMsgProtobuf"/> class.
        /// </summary>
        /// <param name="eMsg">The network message type for this packet message.</param>
        /// <param name="data">The data.</param>
        public PacketClientMsgProtobuf(EMsg eMsg, byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            MsgType = eMsg;
            payload = data;

            Header = new MsgHdrProtoBuf();

            // we need to pull out the job ids, so we deserialize the protobuf header
            using MemoryStream ms = new MemoryStream(data);
            Header.Deserialize(ms);
            BodyOffset = ms.Position;
        }


        /// <summary>
        /// Gets the underlying data that represents this client message.
        /// </summary>
        /// <returns>The data.</returns>
        public byte[] GetData()
        {
            return payload;
        }

        public byte[] GetBody()
        {
            return payload[(int)BodyOffset..];
        }

        public T GetBody<T>() where T : IMessage<T>, new()
        {
            MessageParser<T> parser = new(() => new T());
            return parser.ParseFrom(GetBody());
        }

        public void MergeBody(IMessage message)
        {
            message.MergeFrom(GetBody());
        }
    }

    /// <summary>
    /// Represents a packet message with extended header information.
    /// </summary>
    public sealed class PacketClientMsg : IPacketMsg
    {
        /// <summary>
        /// Gets a value indicating whether this packet message is protobuf backed.
        /// This type of message is never protobuf backed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is protobuf backed; otherwise, <c>false</c>.
        /// </value>
        public bool IsProto => false;
        /// <summary>
        /// Gets the network message type of this packet message.
        /// </summary>
        /// <value>
        /// The message type.
        /// </value>
        public EMsg MsgType { get; }

        /// <summary>
        /// Gets the target job id for this packet message.
        /// </summary>
        /// <value>
        /// The target job id.
        /// </value>
        public ulong TargetJobID => Header.TargetJobID;
        /// <summary>
        /// Gets the source job id for this packet message.
        /// </summary>
        /// <value>
        /// The source job id.
        /// </value>
        public ulong SourceJobID => Header.SourceJobID;
        /// <summary>
        /// Gets the header for this packet message.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public ExtendedClientMsgHdr Header;
        /// <summary>
        /// Gets the offset in payload to the body after the header.
        /// </summary>
        /// <value>
        /// The offset in payload after the header.
        /// </value>
        public long BodyOffset;

        byte[] payload;


        /// <summary>
        /// Initializes a new instance of the <see cref="PacketClientMsg"/> class.
        /// </summary>
        /// <param name="eMsg">The network message type for this packet message.</param>
        /// <param name="data">The data.</param>
        public PacketClientMsg(EMsg eMsg, byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            MsgType = eMsg;
            payload = data;

            Header = new ExtendedClientMsgHdr();

            // deserialize the extended header to get our hands on the job ids
            using MemoryStream ms = new MemoryStream(data);
            Header.Deserialize(ms);
            BodyOffset = ms.Position;
        }


        /// <summary>
        /// Gets the underlying data that represents this client message.
        /// </summary>
        /// <returns>The data.</returns>
        public byte[] GetData()
        {
            return payload;
        }

        public byte[] GetBody()
        {
            return payload[(int)BodyOffset..];
        }
    }

    /// <summary>
    /// Represents a packet message with basic header information.
    /// </summary>
    public sealed class PacketMsg : IPacketMsg
    {
        /// <summary>
        /// Gets a value indicating whether this packet message is protobuf backed.
        /// This type of message is never protobuf backed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is protobuf backed; otherwise, <c>false</c>.
        /// </value>
        public bool IsProto => false;
        /// <summary>
        /// Gets the network message type of this packet message.
        /// </summary>
        /// <value>
        /// The message type.
        /// </value>
        public EMsg MsgType { get; }

        /// <summary>
        /// Gets the target job id for this packet message.
        /// </summary>
        /// <value>
        /// The target job id.
        /// </value>
        public ulong TargetJobID => Header.TargetJobID;
        /// <summary>
        /// Gets the source job id for this packet message.
        /// </summary>
        /// <value>
        /// The source job id.
        /// </value>
        public ulong SourceJobID => Header.SourceJobID;
        /// <summary>
        /// Gets the header for this packet message.
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        internal MsgHdr Header;
        /// <summary>
        /// Gets the offset in payload to the body after the header.
        /// </summary>
        /// <value>
        /// The offset in payload after the header.
        /// </value>
        internal long BodyOffset;

        byte[] payload;


        /// <summary>
        /// Initializes a new instance of the <see cref="PacketMsg"/> class.
        /// </summary>
        /// <param name="eMsg">The network message type for this packet message.</param>
        /// <param name="data">The data.</param>
        public PacketMsg(EMsg eMsg, byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            MsgType = eMsg;
            payload = data;

            Header = new MsgHdr();

            // deserialize the header to get our hands on the job ids
            using MemoryStream ms = new MemoryStream(data);
            Header.Deserialize(ms);
            BodyOffset = ms.Position;
        }


        /// <summary>
        /// Gets the underlying data that represents this client message.
        /// </summary>
        /// <returns>The data.</returns>
        public byte[] GetData()
        {
            return payload;
        }

        public byte[] GetBody()
        {
            return payload[(int)BodyOffset..];
        }
    }
}
