using System;
using System.Diagnostics;
using System.Net;

namespace P2PChat.Models
{
    public class Message
    {
        public string Text
        {
            get; set;
        }

        public Message(string text)
        {
            this.Text = text;
        }

        public ChatPartner Parse()
        {
            string[] parsing = this.Text.Split(new char[] { ' ' });
            if (IPAddress.TryParse(parsing[1], out IPAddress address))
            {
                if (int.TryParse(parsing[2], out int port))
                {
                    ChatPartner partner = new ChatPartner(parsing[0], address, port);
                    for (int i = 0; i < 3; i++)
                    {
                        parsing[i] = string.Empty;
                    }
                    this.Text = string.Join(" ", parsing);
                    return partner;
                }
            }

            Debug.WriteLine("Couldn't parse IpAddress.");
            return null;
        }
    }
}