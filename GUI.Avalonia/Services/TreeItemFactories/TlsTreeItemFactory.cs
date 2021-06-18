using System.Collections.Generic;
using Avalonia.Controls;
using PacketDotNet;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Application.Tls;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class TlsTreeItemFactory 
    {
        //TODO SSL
        public TreeViewItem CreateTreeViewItem(LinuxSllPacket packet)
        {
            var tlsItem = new TreeViewItem() {Header = "TLS " + packet.Type};

            var childItems = new List<TreeViewItem>();
            
            childItems.Add(new TreeViewItem { Header = "Content Type: " + packet.Type });

            tlsItem.Items = childItems;
            return tlsItem;
        }
    }
}