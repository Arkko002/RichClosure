using GUI.WPF.Commands;

namespace GUI.WPF.ViewModels
{
    public class PacketSnifferViewModel : IViewModel
    {
        private readonly PacketSnifferService _packetSniffer;

        public NetworkInterface NetworkInterface { get; set; }

        public ICommand StartSniffingCommand { get; }
        public ICommand StopSniffingCommand { get; }

        public PacketSnifferViewModel(PacketSnifferService packetSniffer)
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
