using System;

namespace P2PChat
{
    public class UDPDataReceivedEventArgs : EventArgs
    {
        public byte[] Data
        {
            get; set;
        }

        public UDPDataReceivedEventArgs(byte[] data)
        {
            this.Data = data;
        }
    }
}