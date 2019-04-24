using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace richClosure.Views.Commands
{
    class StopSniffingCommand : ICommand
    {
        private PacketSniffer _packetSniffer;

        public StopSniffingCommand(PacketSniffer packetSniffer)
        {
            _packetSniffer = packetSniffer;
        }

        public bool CanExecute(object parameter)
        {
            if (_packetSniffer.IsWorking)
            {
                return true;
            }

            return false;
        }

        public void Execute(object parameter)
        {
            _packetSniffer.StopWorking();
        }

        public event EventHandler CanExecuteChanged;
    }
}
