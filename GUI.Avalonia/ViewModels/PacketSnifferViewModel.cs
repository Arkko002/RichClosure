using System.Net.NetworkInformation;
using System.Windows.Input;
using PacketSniffer;
using PacketSniffer.Services;
using richClosure.Avalonia.Commands;

namespace richClosure.Avalonia.ViewModels
{
    public class PacketSnifferViewModel : ViewModelBase 
    {
        private readonly IPacketSniffer _packetSniffer;

        public NetworkInterface NetworkInterface { get; set; }

        public ICommand StartSniffingCommand { get; }
        public ICommand StopSniffingCommand { get; }

        public PacketSnifferViewModel(IPacketSniffer packetSniffer)
        {
            _packetSniffer = packetSniffer;

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
