using System;
using System.Collections.Generic;
using Avalonia.Controls;
using PacketDotNet;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Internet.Ip;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class Ipv6TreeItemFactory 
    {
        public TreeViewItem CreateTreeViewItem(IPv6Packet packet)
        {
            TreeViewItem ip6Item = new() { Header = "IPv6 Layer, " + "Dest: " + packet.DestinationAddress + ", Src: " + packet.SourceAddress };

            var childItems = new List<TreeViewItem>();
            
            childItems.Add(new TreeViewItem { Header = "Version: " + packet.Version });
            childItems.Add(new TreeViewItem { Header = "Traffic Class: " + packet.TrafficClass });
            childItems.Add(new TreeViewItem { Header = "Flow Label: " + packet.FlowLabel });
            childItems.Add(new TreeViewItem { Header = "Payload Length: " + packet.PayloadLength });
            childItems.Add(new TreeViewItem { Header = "Hop Limit: " + packet.HopLimit });
            childItems.Add(new TreeViewItem { Header = "Src. Adr.: " + packet.SourceAddress });
            childItems.Add(new TreeViewItem { Header = "Dest. Adr.: " + packet.DestinationAddress });

            ip6Item.Items = childItems;

            return ip6Item;
        }
    }
}