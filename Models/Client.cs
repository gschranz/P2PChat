using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace P2PChat.Models
{
    public class Client
    {
        public IPAddress Ip
        {
            get; set;
        }

        public int Port
        {
            get; set;
        }

        public Client()
        {

        }
    }
}
