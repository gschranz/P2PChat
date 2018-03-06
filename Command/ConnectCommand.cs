using P2PChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P2PChat.Command
{
    class ConnectCommand : ICommand
    {
        private ChatPartnerVM obj;

        public event EventHandler CanExecuteChanged;
        
        public ConnectCommand(ChatPartnerVM obj)
        {
            this.obj = obj;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            obj.FireConnect();
        }
    }
}
