

namespace P2PChat.ViewModels
{
    using P2PChat.Models;

    public class ChatPartnerVM
    {
        private ChatPartner cp;

        public string IpAddress
        {
            get
            {
                return cp.Ip.ToString();
            }
        }

        public string Available
        {
            get; set;
        }

        public ChatPartnerVM(ChatPartner partner)
        {
            this.cp = partner;
        }
    }
}