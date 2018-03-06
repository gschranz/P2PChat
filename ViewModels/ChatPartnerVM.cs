

namespace P2PChat.ViewModels
{
    using P2PChat.Command;
    using P2PChat.Models;
    using System;
    using System.Windows.Input;

    public class ChatPartnerVM
    {
        private ChatPartner cp;
        private ConnectCommand connectCommand;
        public event EventHandler<EventArgs> OnConnect;

        public string IpAddress
        {
            get
            {
                return this.cp.Ip.ToString();
            }
        }

        public ICommand Connect
        {
            get
            {
                return this.connectCommand;
            }
        }

        public string PCName
        {
            get
            {
                return this.cp.PCName;
            }
        }

        public string Port
        {
            get
            {
                return this.cp.Port.ToString();
            }
        }

        public ChatPartner Partner
        {
            get
            {
                return this.cp;
            }
        }

        public ChatPartnerVM(ChatPartner partner)
        {
            this.cp = partner;
            this.connectCommand = new ConnectCommand(this);
        }

        public void FireConnect()
        {
            this.OnConnect?.Invoke(this, new EventArgs());
        }
    }
}