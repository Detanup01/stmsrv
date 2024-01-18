using Steam3Kit.Types;
using Steam3Kit.Utils;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using UtilsLib;
using static Steam3Server.Others.AppTickets;

namespace Steam3Server.Others
{
    public class AppTickets
    {
        /*
        For full readme before everything:

        Sig Len = Always 128!

        Full Len = Sig Len + Ownership Len + (IF GC == 56 else 0)

        GCLen = Sig + OWLen

        If has GC tokenLen = 20
        else set to OWLen

        unk is always zero??

        CConTime is NOT EPOCH

        IP For OT is same every req

        GC IP is different every req

        */

        public class DlcDetails
        {
            public uint AppId { get; set; }
            public List<uint> Licenses { get; set; }

            public override string ToString()
            {
                string ret = $"\nAppId: {this.AppId}, LicensesCount: {Licenses.Count}";
                if (Licenses.Count > 0)
                {
                    ret += $"Licenses: {string.Join(" ", this.Licenses)}";
                }

                return ret;
            }
        }

        public const int SigLen = 128;
        public const uint GCWriteLen = 20;
        public const uint SessionLen = 24;
        public class TicketRequest
        {
            // GC
            public bool HasGCToken { get; set; }
            public ulong GcToken { get; set; }

            public uint Version { get; set; }
            public SteamID SteamID { get; set; }
            public uint AppId { get; set; }
            public IPAddress OwnershipTicketExternalIP { get; set; }
            public IPAddress OwnershipTicketInternalIP { get; set; }
            public uint OwnershipFlags { get; set; }
            public List<uint> Licenses { get; set; }
            public List<DlcDetails> DLC { get; set; }
        }

        public class GCStruct
        {
            public ulong GcToken { get; set; }
            public SteamID GCSteamID { get; set; }
            public DateTime TokenGenerated { get; set; }
            public IPAddress SessionInternalIP { get; set; }
            public IPAddress SessionExternalIP { get; set; }
            public uint SessionConnectionTime { get; set; }
            public uint SessionConnectionCount { get; set; }

            public uint GCLen { get; set; }

            public override string ToString()
            {
                return $"\nGCToken: {GcToken}, TokenGenerated: {TokenGenerated}, InternalIP: {SessionInternalIP.ToString()}, ExternalIP: {SessionExternalIP.ToString()}, CConnectionTime: {SessionConnectionTime}, CConnectionCount: {SessionConnectionCount}, GCLen: {GCLen}";
            }
        }

        public class TicketStruct
        {
            //Values that are useful
            public int FullLen { get; set; }
            public uint TicketLength { get; set; }
            public int OwnershipLength { get; set; }
            public bool HasGC { get; set; }

            //the Ticket

            //GC related
            public GCStruct GC { get; set; }

            // normal token related
            public uint Version { get; set; }
            public SteamID SteamID { get; set; }
            public uint AppID { get; set; }
            public IPAddress OwnershipTicketExternalIP { get; set; }
            public IPAddress OwnershipTicketInternalIP { get; set; }
            public uint OwnershipFlags { get; set; }
            public DateTime OwnershipTicketGenerated { get; set; }
            public DateTime OwnershipTicketExpires { get; set; }
            public List<uint> Licenses { get; set; }
            public List<DlcDetails> DLC { get; set; }
            public byte[] Signature { get; set; }

            public uint unk { get; set; }

            public override string ToString()
            {
                string gc_specific = "";
                if (HasGC)
                {
                    gc_specific = GC.ToString();
                }



                return $"FullLen: {FullLen}, TicketLen : {TicketLength}, OwnershipLength: {OwnershipLength}, Version: {Version}, SteamID: {SteamID.ToString()}, AppId: {AppID}, " +
                    $"OTExtIP: {OwnershipTicketExternalIP.ToString()}, " +
                    $"OTIntIP: {OwnershipTicketInternalIP.ToString()}, " +
                    $"OFlags: {OwnershipFlags}, " +
                    $"OWGenTime: {OwnershipTicketGenerated}, " +
                    $"OWExp: {OwnershipTicketExpires}, " +
                    $"Licenses: {string.Join(" ", this.Licenses)}," +
                    $"DLC Count: {DLC.Count}, " +
                    $"DLC's: {string.Join(" ", this.DLC)}, " +
                    $"Signature: {BitConverter.ToString(Signature).Replace("-", "")}, SigLen: {Signature.Length} unk: {unk}" + gc_specific;
            }

            public string ToCensored()
            {
                return $"TicketLen : {TicketLength}, Version: {Version}, AppId: {AppID}, " +
                        $"OFlags: {OwnershipFlags}, " +
                        $"OWGenTime: {OwnershipTicketGenerated}, " +
                        $"OWExp: {OwnershipTicketExpires}, " +
                        $"Licenses: {string.Join(" ", this.Licenses)} " +
                        $"DLC Count: {DLC.Count}, " +
                        $"DLC's: {string.Join(" ", this.DLC)}, " +
                        $"Signature: {BitConverter.ToString(Signature).Replace("-", "")}, SigLen: {Signature.Length} unk: {unk}";
            }
        }

        public static byte[] CreateTicket(TicketRequest request)
        {
            var ticketData = WriteTicketData(request);
            var GCData = WriteGCData(request);
            using MemoryStream ms = new();
            using BinaryWriter writer = new BinaryWriter(ms);
            if (request.HasGCToken)
            {
                writer.Write(GCWriteLen);
                writer.Write(GCData);
            }
            writer.Write((uint)(ticketData.Length+4));
            writer.Write(ticketData);
            var retBytes = ms.ToArray();
            byte[] hashSig = retBytes;
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportFromPem(File.ReadAllText("Keys/AppTicket.key"));
                hashSig = rsa.SignData(hashSig, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            }
            //Console.WriteLine(hashSig.Length);
            retBytes = retBytes.Concat(hashSig).ToArray();
            return retBytes;
        }

        static byte[] WriteTicketData(TicketRequest request)
        {
            using MemoryStream memTicketData = new();
            using BinaryWriter writer = new BinaryWriter(memTicketData);
            writer.Write(request.Version);
            writer.Write(request.SteamID.ConvertToUInt64());
            writer.Write(request.AppId);
            writer.Write((uint)request.OwnershipTicketExternalIP.Address);
            writer.Write((uint)request.OwnershipTicketInternalIP.Address);
            writer.Write(request.OwnershipFlags);
            writer.Write((uint)DateTimeOffset.Now.ToUnixTimeSeconds());
            writer.Write((uint)DateTimeOffset.Now.AddDays(100).ToUnixTimeSeconds());
            writer.Write((ushort)request.Licenses.Count);
            foreach (var item in request.Licenses)
            {
                writer.Write(item);
            }
            writer.Write((ushort)request.DLC.Count);
            foreach (var item in request.DLC)
            {
                writer.Write(item.AppId);
                writer.Write((ushort)item.Licenses.Count);
                foreach (var item_lic in item.Licenses)
                {
                    writer.Write(item_lic);
                }
            }
            writer.Write((ushort)0);
            var TicketData = memTicketData.ToArray();        
            //Console.WriteLine(TicketData.Length);
            return TicketData;
        }

        static byte[] WriteGCData(TicketRequest request)
        {
            using MemoryStream memGCData = new();
            using BinaryWriter writer = new BinaryWriter(memGCData);

            writer.Write(request.GcToken);
            writer.Write(request.SteamID.ConvertToUInt64());
            writer.Write((uint)DateTimeOffset.Now.ToUnixTimeSeconds());
            writer.Write(SessionLen);
            writer.Write((uint)1); 
            writer.Write((uint)2);
            writer.Write((uint)request.OwnershipTicketExternalIP.Address);
            writer.Write((uint)request.OwnershipTicketInternalIP.Address);
            writer.Write((uint)1000);
            writer.Write((uint)1);
            //Console.WriteLine("SigLen + Pos: " + (SigLen + writer.BaseStream.Position + 2));
            writer.Write((uint)(SigLen + writer.BaseStream.Position + 2));
            var GCData = memGCData.ToArray();
            //Console.WriteLine("GCDL: " + GCData.Length);
            return GCData;
        }

        /// <summary>
        /// Getting Ticket Structure from ByteArray
        /// </summary>
        /// <param name="ticket">Ticket Data</param>
        /// <returns>The Ticket Structure</returns>
        public static TicketStruct GetTicket(byte[] ticket)
        {
            //Console.WriteLine(BitConverter.ToString(ticket).Replace("-"," "));
            TicketStruct ticketStruct = new TicketStruct();
            ticketStruct.Licenses = new();
            ticketStruct.DLC = new();
            ticketStruct.FullLen = ticket.Length;
            using var ms = new MemoryStream(ticket);
            using var ticketReader = new BinaryReader(ms, System.Text.Encoding.UTF8, true);

            try
            {
                ticketStruct.TicketLength = ticketReader.ReadUInt32();
                if (ticketStruct.TicketLength == 20)
                {
                    ticketStruct.HasGC = true;
                    ticketStruct.GC = new();
                    ticketStruct.GC.GcToken = ticketReader.ReadUInt64();
                    ticketStruct.GC.GCSteamID = new SteamID(ticketReader.ReadUInt64());
                    ticketStruct.GC.TokenGenerated = DateTimeOffset.FromUnixTimeSeconds(ticketReader.ReadUInt32()).DateTime;
                    
                    uint two_four = ticketReader.ReadUInt32();
                    if (two_four != 24)
                    {
                        throw new Exception("!24 | " + two_four);
                    }
                    //Console.WriteLine(ticketReader.ReadUInt32()); //always 1
                    //Console.WriteLine(ticketReader.ReadUInt32());   //always 2
                    ticketReader.BaseStream.Seek(8, SeekOrigin.Current);
                    ticketStruct.GC.SessionExternalIP = new IPAddress(ticketReader.ReadUInt32());
                    ticketStruct.GC.SessionInternalIP = new IPAddress(ticketReader.ReadUInt32());
                    //ticketReader.BaseStream.Seek(4, SeekOrigin.Current);
                    ticketStruct.GC.SessionConnectionTime = ticketReader.ReadUInt32();
                    ticketStruct.GC.SessionConnectionCount = ticketReader.ReadUInt32();

                    ticketStruct.GC.GCLen = ticketReader.ReadUInt32();
                    int gcoffset = (int)ms.Position;
                    //Console.WriteLine(gcoffset);
                    if (ticketStruct.GC.GCLen + gcoffset != ms.Length)
                    {
                        throw new Exception("gcoffset != ms.Length | " + ticketStruct.GC.GCLen);
                    }
                }
                else
                {
                    ticketStruct.HasGC = false;
                    ms.Seek(-4, SeekOrigin.Current);
                }

                int ownershipTicketOffset = (int)ms.Position;
                Console.WriteLine(ownershipTicketOffset);
                ticketStruct.OwnershipLength = ticketReader.ReadInt32();
                Console.WriteLine($"OTO: {ownershipTicketOffset}, OL: {ticketStruct.OwnershipLength}, MSL: {ms.Length}, +: {(ownershipTicketOffset + ticketStruct.OwnershipLength)}");
                if (ownershipTicketOffset + ticketStruct.OwnershipLength != ms.Length &&
                    ownershipTicketOffset + ticketStruct.OwnershipLength + 128 != ms.Length)
                {
                    throw new Exception("ownershipTicketOffset + ownershipTicketLength | " + $"OTO: {ownershipTicketOffset}, OL: {ticketStruct.OwnershipLength}, MSL: {ms.Length}, +: {(ownershipTicketOffset + ticketStruct.OwnershipLength)}");
                }

                ticketStruct.Version = ticketReader.ReadUInt32();
                ticketStruct.SteamID = new SteamID(ticketReader.ReadUInt64());
                ticketStruct.AppID = ticketReader.ReadUInt32();
                ticketStruct.OwnershipTicketExternalIP = new IPAddress(ticketReader.ReadUInt32());
                ticketStruct.OwnershipTicketInternalIP = new IPAddress(ticketReader.ReadUInt32());
                ticketStruct.OwnershipFlags = ticketReader.ReadUInt32();
                ticketStruct.OwnershipTicketGenerated = DateTimeOffset.FromUnixTimeSeconds(ticketReader.ReadUInt32()).DateTime;
                ticketStruct.OwnershipTicketExpires = DateTimeOffset.FromUnixTimeSeconds(ticketReader.ReadUInt32()).DateTime;
                ticketStruct.Licenses = new List<uint>();

                int licenseCount = ticketReader.ReadUInt16();
                for (int i = 0; i < licenseCount; i++)
                {
                    ticketStruct.Licenses.Add(ticketReader.ReadUInt32());
                }

                int dlcCount = ticketReader.ReadUInt16();
                for (int i = 0; i < dlcCount; i++)
                {
                    var dlc = new DlcDetails
                    {
                        AppId = ticketReader.ReadUInt32(),
                        Licenses = new List<uint>()
                    };

                    licenseCount = ticketReader.ReadUInt16();

                    for (int j = 0; j < licenseCount; j++)
                    {
                        dlc.Licenses.Add(ticketReader.ReadUInt32());
                    }

                    ticketStruct.DLC.Add(dlc);
                }

                ticketStruct.unk = ticketReader.ReadUInt16();

                if (ms.Position + 128 == ms.Length)
                {
                    ticketStruct.Signature = ticketReader.ReadBytes(128);
                }
                else
                {
                    ticketStruct.Signature = new byte[] { };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Debug.WriteDebug(ex.ToString());
                throw;
            }
            return ticketStruct;
        }

        /// <summary>
        /// Printing Ticket Data from Bytes
        /// </summary>
        /// <param name="ticket">Ticket as Bytes</param>
        public static void PrintTicket(byte[] ticket)
        {
            Console.WriteLine(GetTicket(ticket).ToCensored());
        }
    }
}
