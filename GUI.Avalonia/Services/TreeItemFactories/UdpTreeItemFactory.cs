using System.Collections.Generic;
using Avalonia.Controls;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Transport;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class UdpTreeItemFactory : IAbstractTreeItemFactory
    {
        public TreeViewItem CreateTreeViewItem(IPacket packet)
        {
            UdpPacket udpPacket = packet as UdpPacket;

            var udpItem = new TreeViewItem()
            {
                Header = ", Dest. Port: " + udpPacket.DestinationPort +
                         ", Src. Port: " + udpPacket.SourcePort
            };

            var childItems = new List<TreeViewItem>();
            
            childItems.Add(new TreeViewItem { Header = "Dest. Port: " + udpPacket.DestinationPort });
            childItems.Add(new TreeViewItem { Header = "Src. Port: " + udpPacket.SourcePort });
            childItems.Add(new TreeViewItem { Header = "Length: " + udpPacket.Length });
            childItems.Add(new TreeViewItem { Header = "Checksum: " + udpPacket.Checksum });

            udpItem.Items = childItems;
            return udpItem;
        }
    }
}