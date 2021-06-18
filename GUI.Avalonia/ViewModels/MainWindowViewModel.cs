using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Threading;
using PacketDotNet;
using PacketSniffer;
using ReactiveUI;
using richClosure.Avalonia.Models;
using SharpPcap;
using Swordfish.NET.Collections;

namespace richClosure.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public IPcapSniffer PacketSniffer { get; }

        private PacketWrapper _selectedPacket;
        public PacketWrapper SelectedPacket
        {
            get => _selectedPacket;
            set => this.RaiseAndSetIfChanged(ref _selectedPacket, value); 
        }
        public ConcurrentObservableCollection<PacketWrapper> Packets { get; }
        private int _id;
        
        public ReactiveCommand<ICaptureDevice, Unit> StartSniffingCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowInterfaceCommand { get; }
        public ReactiveCommand<Unit, Unit> StopSniffingCommand { get; }
        public Interaction<InterfaceSelectionViewModel, ICaptureDevice?> ShowInterfaceInteraction { get; }

        
        public MainWindowViewModel(IPcapSniffer packetSniffer)
        {
            PacketSniffer = packetSniffer;
            Packets = new ConcurrentObservableCollection<PacketWrapper>();
            _id = 0;
            PacketSniffer.Packets.CollectionChanged += PacketsOnCollectionChanged;
            
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

        //TODO Limit Sharppcap dependency to PacketSniffer
        private async void PacketsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            _id++;
            foreach (var newPacket in e.NewItems)
            {
               await Dispatcher.UIThread.InvokeAsync(async () => Packets.Add(new PacketWrapper(newPacket as Packet, _id))); 
            }
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