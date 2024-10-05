using System.Net;
using System.Net.Sockets;

namespace MainLib
{
    public class ImpSocket
    {
        public Socket sock;
        public string PubAddress = string.Empty;
        public ImpSocket(Socket? socket = null)
        {
            if (socket == null) 
                sock = new Socket(SocketType.Stream, ProtocolType.Tcp);
            else sock = socket;
        }

        public ImpSocket Accept()
        {
            sock = sock.Accept();
            var newsocket = new ImpSocket(sock);
            return newsocket;
        }

        public void Bind(string Address, int port) 
        {
            IPEndPoint ipEndPoint = new(IPAddress.Parse(Address), port);
            PubAddress = Address;
            sock.Bind(ipEndPoint);
        }
        public void Connect(string Address, int port)
        {
            PubAddress = Address;
            sock.Connect(Address, port);
            Console.WriteLine("Connecting to address");
        }
        public void Close() 
        {
            sock.Close();
        }

        public int Send(byte[] data, bool Log)
        {
            var sb = sock.Send(data);
            if (Log)
                Console.WriteLine(BitConverter.ToString(data));
            if (sb != data.Length)
                Console.WriteLine("NOTICE!!! Number of bytes sent doesn't match what we tried to send");
            return sb;
        }

        public void Listen(int con)
        {
            sock.Listen(con);
        }
    }
}
