using System.Collections.Generic;
using Avalonia.Controls;
using PacketDotNet;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class UdpTreeItemFactory 
    {
        public TreeViewItem CreateTreeViewItem(UdpPacket packet)
        {
            var udpItem = new TreeViewItem()
            {
                Header = ", Dest. Port: " + packet.DestinationPort +
                         ", Src. Port: " + packet.SourcePort
            };

            var childItems = new List<TreeViewItem>();
            
            childItems.Add(new TreeViewItem { Header = "Dest. Port: " + packet.DestinationPort });
            childItems.Add(new TreeViewItem { Header = "Src. Port: " + packet.SourcePort });
            childItems.Add(new TreeViewItem { Header = "Length: " + packet.Length });
            childItems.Add(new TreeViewItem { Header = "Checksum: " + packet.Checksum });

            udpItem.Items = childItems;
            return udpItem;
        }
    }
}