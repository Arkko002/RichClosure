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
using SharpPcap;
using Splat;

namespace richClosure.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public IPcapSniffer PacketSniffer { get; }
        public IPacketFrame SelectedPacket { get; set; }

        public ReactiveCommand<ICaptureDevice, Unit> StartSniffingCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowInterfaceCommand { get; }
        public ReactiveCommand<Unit, Unit> StopSniffingCommand { get; }
        public Interaction<InterfaceSelectionViewModel, ICaptureDevice?> ShowInterfaceInteraction { get; }

        public MainWindowViewModel(IPcapSniffer packetSniffer)
        {
            PacketSniffer = packetSniffer;

            StartSniffingCommand = ReactiveCommand.Create<ICaptureDevice>(StartSniffing);
            StopSniffingCommand = ReactiveCommand.Create(StopSniffing);


            ShowInterfaceInteraction = new Interaction<InterfaceSelectionViewModel, ICaptureDevice?>();

            ShowInterfaceCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var vm = new InterfaceSelectionViewModel(PacketSniffer.GetAvailableDevices());

                var result = await ShowInterfaceInteraction.Handle(vm);

                if (result is not null)
                {
                    StartSniffing(result);
                }
            });
        }

        public void StartSniffing(ICaptureDevice networkInterface)
        {
            PacketSniffer.SniffPackets(networkInterface);
        }

        public void StopSniffing()
        {
            PacketSniffer.StopSniffing();
        }
    }
}