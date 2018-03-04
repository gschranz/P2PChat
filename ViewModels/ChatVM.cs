using P2PChat.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P2PChat.ViewModels
{
    public class ChatVM : BaseVM
    {
        public ObservableCollection<string> msgHistory
        {
            get; set;
        }

        public string txtMsg
        {
            get; set;
        }

        public ChatPartner Partner
        {
            get; set;
        }

        public ChatVM(ChatPartner partner)
        {
            this.Partner = partner;
            this.msgHistory = new ObservableCollection<string>();
            Task maintask = Task.Run(() => SimpleChatting());
        }

        public void SendMsg()
        {
            SurrogateSelector ss = new SurrogateSelector();
            ss.AddSurrogate(typeof(Message), new StreamingContext(StreamingContextStates.All),
                new MessageSerializationSurrogate());

            IFormatter formatter = new BinaryFormatter
            {
                SurrogateSelector = ss
            };

            Message m1 = new Message("Hello");
        }

        private void SimpleChatting()
        {
            MessageBox.Show($"Connected to {this.Partner.Ip.ToString()}", "Connected", MessageBoxButton.OK);
        }
    }
}
