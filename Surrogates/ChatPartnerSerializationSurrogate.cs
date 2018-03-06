using P2PChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace P2PChat.Surrogates
{
    class ChatPartnerSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            info.AddValue("PCName", ((ChatPartner)obj).PCName);
            info.AddValue("Port", ((ChatPartner)obj).Port);
            info.AddValue("Ip", ((ChatPartner)obj).Ip);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            ChatPartner partner = (ChatPartner)obj;
            partner.PCName = info.GetString("PCName");
            partner.Port = info.GetInt16("Port");
            partner.Ip = (IPAddress)info.GetValue("Ip", IPAddress.None.GetType());
            return partner;
        }
    }
}
