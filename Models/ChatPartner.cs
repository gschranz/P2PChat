namespace P2PChat.Models
{
    using System.Net;
    public class ChatPartner
    {
        public IPAddress Ip
        {
            get; set;
        }

        public int Port
        {
            get; set;
        }
    }
}