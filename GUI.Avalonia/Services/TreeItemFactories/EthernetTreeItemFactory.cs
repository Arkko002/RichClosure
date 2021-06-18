using System.Collections.Generic;
using Avalonia.Controls;
using PacketDotNet;
using PacketSniffer.Packets;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class EthernetTreeItemFactory 
    {
        public TreeViewItem CreateTreeViewItem(EthernetPacket packet)
        {
            var treeItem = new TreeViewItem();
            treeItem.Header =
                $"Ethernet  Type: {packet.Type}  Src. Address: {packet.SourceHardwareAddress}  Dst. Address: {packet.DestinationHardwareAddress}";

            var typeItem = new TreeViewItem();
            typeItem.Header = $"Type: {packet.Type}";
            var srcItem = new TreeViewItem();
            srcItem.Header = $"Src. Address: {packet.SourceHardwareAddress}";
            var dstItem = new TreeViewItem();
            dstItem.Header = $"Dst. Address: {packet.DestinationHardwareAddress}";
            
            treeItem.Items = new List<TreeViewItem>()
            {
                typeItem,
                srcItem,
                dstItem
            };

            return treeItem;
        }
    }
}