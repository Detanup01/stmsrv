using Steam3Kit;

namespace Steam3Server.SQL
{
    public class JStemUser
    {
        public int Id { get; set; }
        public ulong SteamID { get; set; }
    }

    public class JRegisteredUser : JStemUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        //  Sadly or not we going to save the password as plain text :(
        public string Password { get; set; }
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

}
