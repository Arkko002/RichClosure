using System;
using System.Collections.Generic;
using Avalonia.Controls;
using PacketDotNet;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Internet.Ip;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class Ipv4TreeItemFactory 
    {
        public TreeViewItem CreateTreeViewItem(IPv4Packet packet)
        {

            TreeViewItem ipItem = new() { Header = "IP4 Layer, " + "Dest: " + packet.DestinationAddress + ", Src: " + packet.SourceAddress };

            var childItems = new List<TreeViewItem>();
            
            childItems.Add(new TreeViewItem { Header = "Version: " + packet.Version });
            childItems.Add(new TreeViewItem { Header = "Header Length: " + packet.HeaderLength });
            childItems.Add(new TreeViewItem { Header = "Protocol: " + Enum.GetName(packet.Protocol) });
            childItems.Add(new TreeViewItem { Header = "Dest. Address: " + packet.DestinationAddress });
            childItems.Add(new TreeViewItem { Header = "Src. Address: " + packet.SourceAddress });
            childItems.Add(new TreeViewItem { Header = "DSCP: " + packet.DifferentiatedServices });
            childItems.Add(new TreeViewItem { Header = "Total Length: " + packet.TotalLength });
            childItems.Add(new TreeViewItem { Header = "Identification: " + packet.Id });
            childItems.Add(new TreeViewItem { Header = "Offset: " + packet.FragmentOffset });
            //TODO
            childItems.Add(new TreeViewItem { Header = "Flags: " + packet.FragmentFlags });

            childItems.Add(new TreeViewItem { Header = "TTL: " + packet.TimeToLive });
            childItems.Add(new TreeViewItem { Header = "Checksum: " + packet.Checksum });

            ipItem.Items = childItems;

            return ipItem;
        }
    }
}