using Steam3Kit;

namespace DB4Steam
{
    public class JSteamUser
    {
        [LiteDB.BsonId]
        public ulong SteamID { get; set; }
    }

    public class JRegisteredUser : JSteamUser
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PublicRSA { get; set; } = string.Empty;
        public uint CreatedAt { get; set; }
    }

    public class JLoggedUser : JSteamUser
    {
        public uint ProtocolVersion { get; set; }
        public uint cellId { get; set; }
        public uint clientPackageVersion { get; set; }
        public EOSType OSType { get; set; }
        public string MachineName { get; set; } = string.Empty;

        //  Idk what are the MachineId used for so we save it for later.
        public string MachineId { get; set; } = string.Empty;
    }

    public class JSteamProfile : JSteamUser
    {
        public string PlayerName { get; set; } = "Unknown";
        public uint LastLogOff { get; set; }
        public uint LastLogOn { get; set; }
        public uint LastSeen { get; set; }
        public byte[] AvatarHash { get; set; } = [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0];
    }

}
