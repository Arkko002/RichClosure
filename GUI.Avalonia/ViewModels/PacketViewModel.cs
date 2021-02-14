using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Data;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Transport;
using ReactiveUI;
using richClosure.Avalonia.Services.TreeItemFactories;
using Splat;

namespace richClosure.Avalonia.ViewModels
{
    public class PacketViewModel : ViewModelBase, IActivatableViewModel
    {
        public ulong Id { get; private set; }
        public string DateTimeCaptured { get; private set; }

        public string Protocol { get; private set; }
        public string DestAddr { get; private set; }
        public string SrcAddr { get; private set; }
        
        public string DestPort { get; private set; }
        public string SrcPort { get; private set; }

        public string Comment { get; private set; }

        
        public PacketTreeViewModel PacketTreeViewModel { get; private set; }
        public PacketHexViewModel PacketHexViewModel { get; private set; }
        
        public ViewModelActivator Activator { get; }

        public PacketViewModel(IPacketFrame packetFrame)
        {
            Activator = new ViewModelActivator();
            this.WhenActivated(disposable =>
            {
                Id = packetFrame.PacketId;
                DateTimeCaptured = packetFrame.DateTimeCaptured.ToString(CultureInfo.InvariantCulture);
                Protocol = Enum.GetName(packetFrame.Packet.PacketProtocol);
                Comment = packetFrame.PacketComment;
                DestAddr = packetFrame.DestinationAddress;
                SrcAddr = packetFrame.SourceAddress;
                DestPort = packetFrame.DestinationPort.ToString();
                SrcPort = packetFrame.SourcePort.ToString();

                PacketTreeViewModel =
                    new PacketTreeViewModel(packetFrame, Locator.Current.GetService<IAbstractTreeItemFactory>());
                PacketHexViewModel = new PacketHexViewModel(packetFrame);

                Disposable
                .Create(() => { })
                .DisposeWith(disposable);
            });
        }
        
    }
}
