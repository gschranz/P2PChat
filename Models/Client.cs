using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace P2PChat.Models
{
    public class Client
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

        public Client(int port)
        {
            this.PCName = Environment.MachineName;
            var ipaddresslist = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (var ip in ipaddresslist)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    this.Ip = ip;
                    break;
                }
            }

            this.Port = port;
        }
    }
}
