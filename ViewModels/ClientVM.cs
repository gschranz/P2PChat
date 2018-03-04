using P2PChat.Command;
using P2PChat.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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
        private Task simpleListener;

        private CancellationTokenSource tokenSource;

        private event EventHandler<EventArgs> OnPortChanged;

        private DirectConnectCommand dirConnectCommand;

        private string _currentPort;

        public ICommand dirConnect
        {
            get
            {
                return this.dirConnectCommand;
            }
        }

        public ObservableCollection<ChatPartnerVM> DiscoveredClients
        {
            get; set;
        }
        public string currentIp { get; set; }
        public string currentPort
        {
            get
            {
                return this._currentPort;
            }
            set
            {
                this._currentPort = value;
                this.FireOnPortChanged();
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

        public ClientVM()
        {
           
            this.dirConnectCommand = new DirectConnectCommand(this);
            this.OnPortChanged += this.PortChangedCallback;
            this.currentIp = "127.0.0.1";
            this._currentPort = "4567";
            this.tokenSource = new CancellationTokenSource();
            this.simpleListener = Task.Factory.StartNew( () => SimpleListener(), tokenSource.Token);
        }

        private void PortChangedCallback(object sender, EventArgs e)
        {
            this.tokenSource.Cancel(true);
            this.simpleListener = Task.Factory.StartNew(() => SimpleListener(), tokenSource.Token);
        }

        private async void SimpleListener()
        {
                bool done = false;
                UdpClient listener = new UdpClient(new IPEndPoint(IPAddress.Any, Convert.ToInt16(this.currentPort)));
                string received_data;
                try
                {
                    tokenSource.Token.ThrowIfCancellationRequested();
                    while (!done)
                    {
                        var result = await listener.ReceiveAsync();
                        received_data = Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length);
                    }
                }
                catch (OperationCanceledException e)
                {
                    done = true;
                    MessageBox.Show(e.ToString());
                    listener.Close();
                }
                listener.Close();
                return;

        }

        public void DirConnect()
        {
            ChatWindow chatWindow = new ChatWindow();
            ChatPartner partner = new ChatPartner { Ip = IPAddress.Parse(this.currentIp), Port = Convert.ToInt16(this.currentPort) };
            ChatVM chatvm = new ChatVM(partner);
            chatWindow.DataContext = chatvm;
            chatWindow.Show();
        }
    }
}
