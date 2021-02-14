using System;
using System.Collections.Generic;
using Avalonia.Controls;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Internet.Ip;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class Ipv4TreeItemFactory : IAbstractTreeItemFactory
    {
        public TreeViewItem CreateTreeViewItem(IPacket packet)
        {
            Ip4Packet ip4Packet = packet as Ip4Packet;

            TreeViewItem ipItem = new() { Header = "IP4 Layer, " + "Dest: " + ip4Packet.DestinationAddress + ", Src: " + ip4Packet.SourceAddress };

            var childItems = new List<TreeViewItem>();
            
            childItems.Add(new TreeViewItem { Header = "Version: " + ip4Packet.Version });
            childItems.Add(new TreeViewItem { Header = "Header Length: " + ip4Packet.HeaderLength });
            childItems.Add(new TreeViewItem { Header = "Protocol: " + Enum.GetName(ip4Packet.NextProtocol) });
            childItems.Add(new TreeViewItem { Header = "Dest. Address: " + ip4Packet.DestinationAddress });
            childItems.Add(new TreeViewItem { Header = "Src. Address: " + ip4Packet.SourceAddress });
            childItems.Add(new TreeViewItem { Header = "DSCP: " + ip4Packet.Dscp });
            childItems.Add(new TreeViewItem { Header = "Total Length: " + ip4Packet.TotalLength });
            childItems.Add(new TreeViewItem { Header = "Identification: " + ip4Packet.Identification });
            childItems.Add(new TreeViewItem { Header = "Offset: " + ip4Packet.Offset });

            TreeViewItem ipFlagsItem = new TreeViewItem();
            //TODO
            // ipFlagsItem.Items.Add(new TreeViewItem { Header = "DF - " + ip4Packet.Df });
            // ipFlagsItem.Items.Add(new TreeViewItem { Header = "MF - " + ip4Packet.Ip4Flags.Mf });
            // ipFlagsItem.Items.Add(new TreeViewItem { Header = "Res. - " + ip4Packet.Ip4Flags.Res });
            // ipItem.Items.Add(ipFlagsItem);

            childItems.Add(new TreeViewItem { Header = "TTL: " + ip4Packet.TimeToLive });
            childItems.Add(new TreeViewItem { Header = "Header Checksum: " + ip4Packet.HeaderChecksum });

            ipItem.Items = childItems;

            return ipItem;
        }
    }
}