using richClosure.Commands;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Windows.Input;
using richClosure.Packet_Sniffing;
using richClosure.Packets;

namespace richClosure.ViewModels
{
    public class PacketSnifferViewModel : IViewModel
    {
        private readonly PacketSniffer _packetSniffer;
        public ObservableCollection<IPacket> ModelCollection { get; }

        public NetworkInterface NetworkInterface { get; set; }

        public ICommand StartSniffingCommand { get; }
        public ICommand StopSniffingCommand { get; }

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
