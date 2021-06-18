using System.Collections.Generic;
using Avalonia.Controls;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Application;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    //TODO
    public class HttpTreeItemFactory 
    {
        public TreeViewItem CreateTreeViewItem(IPacket packet)
        {
            HttpPacket pac = packet as HttpPacket;

            var httpItem = new TreeViewItem() {Header = "HTTP"};

            var childItems = new List<TreeViewItem>();
            
            foreach (KeyValuePair<string, string> entry in pac.HttpFieldsDict)
            {
                childItems.Add(new TreeViewItem { Header = entry.Key + ": " + entry.Value });
            }

            httpItem.Items = childItems;
            return httpItem;
        }
    }
}