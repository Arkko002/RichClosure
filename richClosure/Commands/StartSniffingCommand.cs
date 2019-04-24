using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using richClosure.ViewModels;

namespace richClosure.Views.Commands
{
    public class StartSniffingCommand : ICommand
    {
        private PacketCollectionViewModel _packetCollection;
        private PacketSniffer _packetSniffer;

        public StartSniffingCommand(PacketCollectionViewModel packetCollection, PacketSniffer packetSniffer)
        {
            _packetCollection = packetCollection;
            _packetSniffer = packetSniffer;
        }

        public bool CanExecute(object parameter)
        {
            if (!_packetSniffer.IsWorking)
            {
                return true;
            }

            return false;
        }

        public void Execute(object networkInterface)
        {
            _packetSniffer = new PacketSniffer(networkInterface as NetworkInterface, _packetCollection.ModelCollection);

            Thread packetSnifferThread = new Thread(() => _packetSniffer.SniffPackets()) { IsBackground = true };
            packetSnifferThread.Start();


            Thread factoryThread = new Thread(() => _packetSniffer.GetPacketDataFromQueue()) { IsBackground = true };
            factoryThread.Start();
        }

        public event EventHandler CanExecuteChanged;
    }
}
