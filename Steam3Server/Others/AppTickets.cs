using Steam3Kit.Types;
using Steam3Kit.Utils;
using System.Net;
using System.Security.Cryptography;
using UtilsLib;

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

        public class TicketRequest
        {
            // GC
            public bool HasGCToken { get; set; }
            public string GcToken { get; set; }

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
            public string GcToken { get; set; }
            public DateTime TokenGenerated { get; set; }
            public IPAddress SessionExternalIP { get; set; }
            public uint ClientConnectionTime { get; set; }
            public uint ClientConnectionCount { get; set; }

            public uint GCLen { get; set; }

            public override string ToString()
            {
                return $"\nGCToken: {GcToken}, TokenGenerated: {TokenGenerated}, ExternalIP: {SessionExternalIP.ToString()}, CConnectionTime: {ClientConnectionTime}, CConnectionCount: {ClientConnectionCount}, GCLen: {GCLen}";
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
                    $"Licenses: {string.Join(" ", this.Licenses)} " +
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

        /// <summary>
        /// Creating Ticket with Given Data
        /// </summary>
        /// <param name="SteamID"></param>
        /// <param name="AppId"></param>
        /// <param name="IP_Pub"></param>
        /// <param name="IP_Priv"></param>
        /// <param name="TicketFlags"></param>
        /// <param name="Licenses"></param>
        /// <param name="dlcs"></param>
        /// <param name="IsVacBanned"></param>
        /// <param name="TicketVersion"></param>
        /// <returns>Ticket as Bytes</returns>
        public static byte[] CreateTicket(TicketRequest request)
        {
            using MemoryStream ms = new();
            using BinaryWriter writer = new BinaryWriter(ms);
            if (request.HasGCToken)
            {

            }
            else
            { 
            
            }
            writer.Write(request.Version);

            /*
            ms.Write(BitConverter.GetBytes(TicketVersion));
            ms.Write(BitConverter.GetBytes(SteamID.ConvertToUInt64()));
            ms.Write(BitConverter.GetBytes(AppId));
            ms.Write(BitConverter.GetBytes(IP_Pub));
            ms.Write(BitConverter.GetBytes(IP_Priv));
            ms.Write(BitConverter.GetBytes(TicketFlags));
            var curTime = DateTime.UtcNow;
            var endTime = curTime.AddDays(30);
            uint curuint = (uint)DateUtils.DateTimeToUnixTime(curTime);
            uint enduint = (uint)DateUtils.DateTimeToUnixTime(endTime);
            ms.Write(BitConverter.GetBytes(curuint));
            ms.Write(BitConverter.GetBytes(enduint));
            ms.Write(BitConverter.GetBytes((ushort)Licenses.Count));
            foreach (var lc in Licenses) 
            {
                ms.Write(BitConverter.GetBytes(lc));
            }
            ms.Write(BitConverter.GetBytes((ushort)dlcs.Count));
            foreach (var dlc in dlcs)
            {
                ms.Write(BitConverter.GetBytes(dlc.AppId));
                ms.Write(BitConverter.GetBytes((ushort)dlc.Licenses.Count));
                foreach (var dlc_lic in dlc.Licenses)
                {
                    ms.Write(BitConverter.GetBytes(dlc_lic));
                }
            }
            ms.Write(BitConverter.GetBytes(IsVacBanned));

            var Bytes = ms.ToArray();
            var l = Bytes.Length + 4; //    +4 Because it count itself!
            Bytes = BitConverter.GetBytes((uint)l).Concat(Bytes).ToArray();
            byte[] hashSig = Bytes;
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportFromPem(File.ReadAllText("Keys/AppTicket.key"));
                hashSig = rsa.SignData(hashSig, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            var sig = BitConverter.ToString(hashSig);
            Bytes = Bytes.Concat(hashSig).ToArray();
            return Bytes;
            */
            return ms.ToArray();
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
                    ticketStruct.GC.GcToken = ticketReader.ReadUInt64().ToString();
                    ticketReader.BaseStream.Seek(8, SeekOrigin.Current);
                    ticketStruct.GC.TokenGenerated = DateTimeOffset.FromUnixTimeSeconds(ticketReader.ReadUInt32()).DateTime;

                    uint two_four = ticketReader.ReadUInt32();
                    if (two_four != 24)
                    {
                        throw new Exception("!24 | " + two_four);
                    }

                    ticketReader.BaseStream.Seek(8, SeekOrigin.Current);
                    ticketStruct.GC.SessionExternalIP = new IPAddress(ticketReader.ReadUInt32());
                    ticketReader.BaseStream.Seek(4, SeekOrigin.Current);
                    ticketStruct.GC.ClientConnectionTime = ticketReader.ReadUInt32();
                    ticketStruct.GC.ClientConnectionCount = ticketReader.ReadUInt32();

                    ticketStruct.GC.GCLen = ticketReader.ReadUInt32();
                    int gcoffset = (int)ms.Position;
                    Console.WriteLine(gcoffset);
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
                if (ownershipTicketOffset + ticketStruct.OwnershipLength != ms.Length &&
                    ownershipTicketOffset + ticketStruct.OwnershipLength + 128 != ms.Length)
                {
                    throw new Exception("ownershipTicketOffset + ownershipTicketLength");
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
