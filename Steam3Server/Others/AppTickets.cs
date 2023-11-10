using Steam3Kit.Types;
using Steam3Kit.Utils;
using System.Net;
using System.Security.Cryptography;
using UtilsLib;

namespace Steam3Server.Others
{
    public class AppTickets
    {
        public class DlcDetails
        {
            public uint AppId { get; set; }
            public List<uint> Licenses { get; set; }

            public override string ToString()
            {
                return $"\nAppId: {this.AppId}, LicensesCount: {Licenses.Count} Licenses: {string.Join(" ",this.Licenses)}";
            }
        }

        public class TicketStruct
        {
            //Values that are useful
            public uint TicketLength { get; set; }
            public bool HasGC { get; set; }

            //the Ticket

            //GC related
            public string GcToken { get; set; }
            public DateTime TokenGenerated { get; set; }
            public IPAddress SessionExternalIP { get; set; }
            public uint ClientConnectionTime { get; set; }
            public uint ClientConnectionCount { get; set; }

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
                    gc_specific = $"\nGCToken: {GcToken}, TokenGenerated: {TokenGenerated}, ExternalIP: {SessionExternalIP.ToString()}, CConnectionTime: {ClientConnectionTime}, CConnectionCount: {ClientConnectionCount}";
                }



                return $"TicketLen : {TicketLength}, Version: {Version}, SteamID: {SteamID.ToString()}, AppId: {AppID}, " +
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
        public static byte[] CreateTicket(ulong SteamID, uint AppId, uint IP_Pub, uint IP_Priv, uint TicketFlags, List<uint> Licenses, List<DlcDetails> dlcs, ushort IsVacBanned = 0, uint TicketVersion = 4)
        {
            MemoryStream ms = new();
            ms.Write(BitConverter.GetBytes(TicketVersion));
            ms.Write(BitConverter.GetBytes(SteamID));
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

            using var ms = new MemoryStream(ticket);
            using var ticketReader = new BinaryReader(ms, System.Text.Encoding.UTF8, true);

            try
            {
                ticketStruct.TicketLength = ticketReader.ReadUInt32();
                if (ticketStruct.TicketLength == 20)
                {
                    ticketStruct.HasGC = true;
                    ticketStruct.GcToken = ticketReader.ReadUInt64().ToString();
                    ticketReader.BaseStream.Seek(8, SeekOrigin.Current);
                    ticketStruct.TokenGenerated = DateTimeOffset.FromUnixTimeSeconds(ticketReader.ReadUInt32()).DateTime;

                    uint two_four = ticketReader.ReadUInt32();
                    if (two_four != 24)
                    {
                        throw new Exception("!24 | " + two_four);
                    }

                    ticketReader.BaseStream.Seek(8, SeekOrigin.Current);
                    ticketStruct.SessionExternalIP = new IPAddress(ticketReader.ReadUInt32());
                    ticketReader.BaseStream.Seek(4, SeekOrigin.Current);
                    ticketStruct.ClientConnectionTime = ticketReader.ReadUInt32();
                    ticketStruct.ClientConnectionCount = ticketReader.ReadUInt32();

                    var pos = ticketReader.ReadUInt32();
                    if (pos + ms.Position != ms.Length)
                    {
                        throw new Exception("ms.Position != ms.Length | " + pos);
                    }
                }
                else
                {
                    ticketStruct.HasGC = false;
                    ms.Seek(-4, SeekOrigin.Current);
                }

                int ownershipTicketOffset = (int)ms.Position;
                int ownershipTicketLength = ticketReader.ReadInt32();
                if (ownershipTicketOffset + ownershipTicketLength != ms.Length &&
                    ownershipTicketOffset + ownershipTicketLength + 128 != ms.Length)
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
