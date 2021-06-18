using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Threading;
using PacketDotNet;
using richClosure.Avalonia.Services.TreeItemFactories;
using Swordfish.NET.Collections;

namespace richClosure.Avalonia.Models
{
    public class PacketWrapper
    {
        public Packet SourcePacket { get; }
        
        public int Id { get; }
        public DateTime DateTimeCaptured { get; }
        public string Type { get; }
        public string SourceAddress { get; }
        public string DestinationAddress { get; }
        public string SourcePort { get; } = string.Empty;
        public string DestinationPort { get; } = string.Empty;
        
        public string HexString { get; }
        public string AsciiString { get; }
       
        public ObservableCollection<TreeViewItem> PacketTreeItems { get; }


        public PacketWrapper(Packet sourcePacket, int id)
        {
            Id = id;
            DateTimeCaptured = DateTime.Now;
            SourcePacket = sourcePacket;
            PacketTreeItems = new ObservableCollection<TreeViewItem>();

            var frameItem = new FrameTreeItemFactory().CreateTreeViewItem(this);
            PacketTreeItems.Add(frameItem);

            //TODO TreeViewItems
            if (sourcePacket is EthernetPacket eth)
            {
                Type = eth.Type.ToString();
                SourceAddress = eth.SourceHardwareAddress.ToString();
                DestinationAddress = eth.DestinationHardwareAddress.ToString();

                HexString = sourcePacket.PrintHex();
                //TODO
                AsciiString = "TODO";

                var ethItem = new EthernetTreeItemFactory().CreateTreeViewItem(eth);
                PacketTreeItems.Add(ethItem);

                var ip = sourcePacket.Extract<IPPacket>();
                if (ip != null)
                {
                    SourceAddress = ip.SourceAddress.ToString();
                    DestinationAddress = ip.DestinationAddress.ToString();

                    //TODO All protocols
                    switch (ip.Protocol)
                    {
                        case ProtocolType.IPv4:
                            var ip4Item =
                                new Ipv4TreeItemFactory().CreateTreeViewItem(sourcePacket.Extract<IPv4Packet>());
                            PacketTreeItems.Add(ip4Item);
                            break;

                        case ProtocolType.IPv6:
                            var ip6Item = new Ipv6TreeItemFactory().CreateTreeViewItem(sourcePacket.Extract<IPv6Packet>());
                            PacketTreeItems.Add(ip6Item);
                            break;
                    }
                    
                    var tcp = sourcePacket.Extract<TcpPacket>();
                    if (tcp != null)
                    {
                        Type += " | TCP";
                        SourcePort = tcp.SourcePort.ToString();
                        DestinationPort = tcp.DestinationPort.ToString();

                        var tcpItem = new TcpTreeItemFactory().CreateTreeViewItem(tcp);
                        PacketTreeItems.Add(tcpItem);
                    }

                    var udp = sourcePacket.Extract<UdpPacket>();
                    if (udp != null)
                    {
                        Type += " | UDP";
                        SourcePort = udp.SourcePort.ToString();
                        DestinationPort = udp.DestinationPort.ToString();

                        var udpItem = new UdpTreeItemFactory().CreateTreeViewItem(udp);
                        PacketTreeItems.Add(udpItem);
                    }
                }
            }
        }
    }
}