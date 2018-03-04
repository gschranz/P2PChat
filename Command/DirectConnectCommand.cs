using P2PChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace P2PChat.Command
{
    public class DirectConnectCommand : ICommand
    {
        private ClientVM obj;

        public event EventHandler CanExecuteChanged;

        public DirectConnectCommand(ClientVM obj)
        {
            this.obj = obj;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            obj.DirConnect();
        }
    }
}
