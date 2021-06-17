using System;
using System.Net.NetworkInformation;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using PacketSniffer;
using PacketSniffer.Packets;
using ReactiveUI;
using richClosure.Avalonia.Views;
using Splat;

namespace richClosure.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public IPacketSniffer PacketSniffer { get; }
        public IPacketFrame SelectedPacket { get; set; }

        public ReactiveCommand<NetworkInterface, Unit> StartSniffingCommand { get; }

        public PacketHexViewModel PacketHexViewModel { get; }
        public PacketTreeViewModel PacketTreeViewModel { get; }
        public ReactiveCommand<Unit, Unit> ShowInterfaceCommand { get; }
        public ReactiveCommand<Unit, Unit> StopSniffingCommand { get; }
        public Interaction<InterfaceSelectionViewModel, NetworkInterface?> ShowInterfaceInteraction { get; }

        public MainWindowViewModel(IPacketSniffer packetSniffer)
        {
            PacketSniffer = packetSniffer;

            StartSniffingCommand = ReactiveCommand.Create<NetworkInterface>(StartSniffing);
            StopSniffingCommand = ReactiveCommand.Create(StopSniffing);


            ShowInterfaceInteraction = new Interaction<InterfaceSelectionViewModel, NetworkInterface?>();

            ShowInterfaceCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var vm = new InterfaceSelectionViewModel();

                var result = await ShowInterfaceInteraction.Handle(vm);

                if (result is not null)
                {
                    StartSniffing(result);
                }
            });
        }

        public void StartSniffing(NetworkInterface networkInterface)
        {
            PacketSniffer.SniffPackets(networkInterface);
        }

        public void StopSniffing()
        {
            PacketSniffer.StopSniffing();
        }
    }
}