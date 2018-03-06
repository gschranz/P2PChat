namespace P2PChat.Models
{
    using System.Net;
    public class ChatPartner
    {
        public string PCName
        {
            get; set;
        }

        public IPAddress Ip
        {
            get; set;
        }

        public int Port
        {
            get; set;
        }

        public ChatPartner(string name, IPAddress address, int port)
        {
            this.PCName = name;
            this.Ip = address;
            this.Port = port;
        }
    }
}