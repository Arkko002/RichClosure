using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Application.Tls;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class TlsTreeItemFactory : IAbstractTreeItemFactory
    {
        public TreeViewItem CreateTreeViewItem(IPacket packet)
        {
            TlsPacket pac = packet as TlsPacket;
            var tlsItem = new TreeViewItem() {Header = "TLS " + pac.Version};

            var childItems = new List<TreeViewItem>();
            
            childItems.Add(new TreeViewItem { Header = "Content Type: " + pac.Type });
            childItems.Add(new TreeViewItem { Header = "Version: " + pac.Version });
            childItems.Add(new TreeViewItem { Header = "Data Length: " + pac.DataLength });
            childItems.Add(new TreeViewItem { Header = "Encrypted Data: " + pac.EncryptedData });

            tlsItem.Items = childItems;
            return tlsItem;
        }
    }
}