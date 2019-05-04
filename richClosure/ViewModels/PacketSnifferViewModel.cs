using richClosure.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace richClosure.ViewModels
{
    public class PacketSnifferViewModel : IViewModel
    {
        private PacketSniffer _packetSniffer;
        public ObservableCollection<IPacket> ModelCollection { get; private set; }

        public NetworkInterface NetworkInterface { get; set; }

        public ICommand StartSniffingCommand { get; private set; }
        public ICommand StopSniffingCommand { get; private set; }


        public PacketSnifferViewModel(ObservableCollection<IPacket> modelCollection)
        {
            ModelCollection = modelCollection;

            _packetSniffer = new PacketSniffer(modelCollection);

            StartSniffingCommand = new RelayCommand(x => StartSniffingPackets(), x => !_packetSniffer.IsWorking);
            StopSniffingCommand = new RelayCommand(x => StopSniffingPackets(), x => _packetSniffer.IsWorking);
        }

        private void StartSniffingPackets()
        {
            _packetSniffer.SniffPackets(NetworkInterface);
        }

        private void StopSniffingPackets()
        {
            _packetSniffer.StopSniffing();
        }
    }
}
