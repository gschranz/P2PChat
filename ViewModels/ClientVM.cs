using P2PChat.Command;
using P2PChat.Models;
using P2PChat.Surrogates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace P2PChat.ViewModels
{
    public class ClientVM : BaseVM
    {
        private UdpClient listener;

        private UdpClient sender;

        private MemoryStream stream;

        private BinaryFormatter formatter;

        private SurrogateSelector ss;

        private event EventHandler<EventArgs> OnPortChanged;

        private event EventHandler<UDPDataReceivedEventArgs> OnDataReceived;

        private DirectConnectCommand dirConnectCommand;

        private BroadCastCommand broadCastCommand;

        private Client client;

        public ICommand DirConnect
        {
            get
            {
                return this.dirConnectCommand;
            }
        }

        public ICommand BroadCast
        {
            get
            {
                return this.broadCastCommand;
            }
        }

        public ObservableCollection<ChatPartnerVM> DiscoveredClients
        {
            get; set;
        }

        public List<ChatVM> ConnectedClients
        {
            get; set;
        }

        public string CurrentIp
        {
            get
            {
                return this.client.Ip.ToString();
            }
            set
            {
                if (IPAddress.TryParse(value, out IPAddress address))
                {
                    this.client.Ip = address;
                }
            }
        }

        public string CurrentPort
        {
            get
            {
                return this.client.Port.ToString();
            }
            set
            {
                if (int.TryParse(value, out int port))
                {
                    this.client.Port = port;
                    this.FireOnPortChanged();
                }
                else
                {
                    MessageBox.Show("Please enter a valid port.", "Not a valid port!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public ObservableCollection<ICryptor> CryptorList
        {
            get; set;
        }

        private void FireOnPortChanged()
        {
            this.OnPortChanged?.Invoke(this, new EventArgs());
        }

        private void FireOnDataReceived(byte[] data)
        {
            this.OnDataReceived?.Invoke(this, new UDPDataReceivedEventArgs(data));
        }

        public ClientVM()
        {
            this.dirConnectCommand = new DirectConnectCommand(this);
            this.broadCastCommand = new BroadCastCommand(this);

            this.OnPortChanged += this.PortChangedCallback;
            this.OnDataReceived += this.DataReceivedCallback;

            this.DiscoveredClients = new ObservableCollection<ChatPartnerVM>();
            this.ConnectedClients = new List<ChatVM>();
            this.client = new Client(4567);

            this.formatter = new BinaryFormatter();
            this.ss = new SurrogateSelector();
            this.ss.AddSurrogate(typeof(Message), new StreamingContext(StreamingContextStates.All), new MessageSerializationSurrogate());
            this.ss.AddSurrogate(typeof(ChatPartner), new StreamingContext(StreamingContextStates.All), new ChatPartnerSerializationSurrogate());
            this.formatter.SurrogateSelector = this.ss;

            Task.Factory.StartNew(() => SimpleListener());
            Task.Factory.StartNew(() => SimpleAliveMessages());
        }

        private void DataReceivedCallback(object sender, UDPDataReceivedEventArgs e)
        {
            using (stream = new MemoryStream())
            {
                stream.Write(e.Data, 0, e.Data.Length);
                stream.Position = 0;

                try
                {
                    Message msg = (Message)this.formatter.Deserialize(stream);
                    ChatPartner partner = msg.Parse();
                    
                        foreach (var chat in this.ConnectedClients.Where(p => p.Partner == partner))
                        {
                            chat.msgHistory.Add(msg.Text);
                            return;
                        }
                        ChatPartnerVM newchatpartnervm = new ChatPartnerVM(partner);
                        newchatpartnervm.OnConnect += this.DirectConnectOnEvent;
                        this.DiscoveredClients.Add(newchatpartnervm);
                }
                catch (InvalidCastException)
                {
                    Debug.WriteLine("Invalid Cast on Broadcast.");
                }
            }
        }



        private void PortChangedCallback(object sender, EventArgs e)
        {
            this.listener.Close();
            this.sender.Close();
            Task.Factory.StartNew(() => SimpleListener());
            Task.Factory.StartNew(() => SimpleAliveMessages());
        }

        private void SimpleListener()
        {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, Convert.ToInt32(this.CurrentPort));
                listener = new UdpClient(ep);
                try
                {
                    while (true)
                    {
                        var result = listener.Receive(ref ep);
                        FireOnDataReceived(result);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            return;
        }

        private void SimpleAliveMessages()
        {
            sender = new UdpClient();
            Message msg = new Message(this.client.PCName + " " + this.client.Ip + " " + this.client.Port + " " + "Discover!");
            using (stream = new MemoryStream())
            {
                formatter.Serialize(stream, msg);
                var data = stream.ToArray();

                try
                {
                    while (true)
                    {
                        sender.EnableBroadcast = true;
                        sender.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, this.client.Port));
                        Thread.Sleep(3000);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
            return;
        }

        public void DirectConnect()
        {
            ChatWindow chatWindow = new ChatWindow();
            ChatPartner partner = new ChatPartner("New_Partner",  IPAddress.Parse(this.CurrentIp), Convert.ToInt16(this.CurrentPort));
            ChatVM chatvm = new ChatVM(partner);
            chatWindow.DataContext = chatvm;
            chatWindow.Show();
        }

        public void DirectConnectOnEvent(object sender, EventArgs args)
        {
            ChatWindow chatWindow = new ChatWindow();
            ChatPartner partner = ((ChatPartnerVM)sender).Partner;
            ChatVM chatvm = new ChatVM(partner);
            chatWindow.DataContext = chatvm;
            chatWindow.Show();
        }

        public void SendBroadCast()
        {
            MessageBox.Show("Test!");
        }
    }
}
