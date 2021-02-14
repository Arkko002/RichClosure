using System;
using System.Collections.Generic;
using Avalonia.Controls;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Internet.Ip;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class Ipv6TreeItemFactory : IAbstractTreeItemFactory
    {
        public TreeViewItem CreateTreeViewItem(IPacket packet)
        {
            Ip6Packet ip6Packet = packet as Ip6Packet;
            TreeViewItem ip6Item = new TreeViewItem { Header = "IPv6 Layer, " + "Dest: " + ip6Packet.DestinationAddress + ", Src: " + ip6Packet.SourceAddress };

            var childItems = new List<TreeViewItem>();
            
            childItems.Add(new TreeViewItem { Header = "Version: " + ip6Packet.Version });
            childItems.Add(new TreeViewItem { Header = "Traffic Class: " + ip6Packet.TrafficClass });
            childItems.Add(new TreeViewItem { Header = "Flow Label: " + ip6Packet.FlowLabel });
            childItems.Add(new TreeViewItem { Header = "Payload Length: " + ip6Packet.PayloadLength });
            childItems.Add(new TreeViewItem { Header = "Next Header: " + Enum.GetName(ip6Packet.NextProtocol) });
            childItems.Add(new TreeViewItem { Header = "Hop Limit: " + ip6Packet.HopLimit });
            childItems.Add(new TreeViewItem { Header = "Src. Adr.: " + ip6Packet.SourceAddress });
            childItems.Add(new TreeViewItem { Header = "Dest. Adr.: " + ip6Packet.DestinationAddress });

            ip6Item.Items = childItems;

            return ip6Item;
        }
    }
}