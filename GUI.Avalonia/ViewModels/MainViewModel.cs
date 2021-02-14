using System.Net.NetworkInformation;
using System.Reactive;
using ReactiveUI;
using PacketSniffer.Services;

namespace richClosure.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public InterfaceSelectionViewModel InterfaceSelectionViewModel { get; } 
        public PacketDataGridViewModel PacketDataGridViewModel { get; set; }
        public IPacketSniffer PacketSniffer { get; }
        public ReactiveCommand<Unit, Unit> StartSniffingCommand { get; }
        public MainWindowViewModel(PacketDataGridViewModel packetDataGridViewModel,
            InterfaceSelectionViewModel interfaceSelectionViewModel, 
            IPacketSniffer packetSniffer) 
        {
            PacketDataGridViewModel = packetDataGridViewModel;
            InterfaceSelectionViewModel = interfaceSelectionViewModel;
            PacketSniffer = packetSniffer;

            //TODO
            StartSniffingCommand = ReactiveCommand.Create(() => StartSniffing());
        }
        
        public void StartSniffing()
        {
            PacketSniffer.SniffPackets(InterfaceSelectionViewModel.SelectedInterface);
        }

        public void StopSniffing()
        {
            PacketSniffer.StopSniffing();
        }
    }
}
