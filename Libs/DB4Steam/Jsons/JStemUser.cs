using Steam3Kit;

namespace DB4Steam
{
    public class JStemUser
    {
        //public int Id { get; set; }

        [LiteDB.BsonId]
        public ulong SteamID { get; set; }
    }

    public class JRegisteredUser : JStemUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PublicRSA { get; set; }
    }

    public class JLoggedUser : JStemUser
    {
        public uint ProtocolVersion { get; set; }
        public uint cellId { get; set; }
        public uint clientPackageVersion { get; set; }
        public EOSType OSType { get; set; }
        public string MachineName { get; set; }

        //  Idk what are the MachineId used for so we save it for later.
        public string MachineId { get; set; }
    }

    public class JFriendData : JStemUser
    {
        public string PlayerName { get; set; } = "Unknown";
        public uint LastLogOff { get; set; }
        public uint LastLogOn { get; set; }
        public uint LastSeen { get; set; }
        public byte[] AvatarHash { get; set; } = [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0];
    }

}
