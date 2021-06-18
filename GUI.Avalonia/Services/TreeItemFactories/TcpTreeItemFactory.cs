using System.Collections.Generic;
using Avalonia.Controls;
using PacketDotNet;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class TcpTreeItemFactory 
    {
        public TreeViewItem CreateTreeViewItem(TcpPacket packet)
        {
            var tcpItem = new TreeViewItem
            {
                Header = "TCP Dest. Port: " + packet.DestinationPort +
                         ", Src. Port: " + packet.SourcePort
            };

            var childItems = new List<TreeViewItem>();

            childItems.Add(new TreeViewItem { Header = "Dest. port: " + packet.DestinationPort });
            childItems.Add(new TreeViewItem { Header = "Src. port: " + packet.SourcePort });
            childItems.Add(new TreeViewItem { Header = "Seq. Number: " + packet.SequenceNumber });
            childItems.Add(new TreeViewItem { Header = "Ack Number: " + packet.AcknowledgmentNumber });
            childItems.Add(new TreeViewItem { Header = "Urg. Pointer: " + packet.UrgentPointer });
            childItems.Add(new TreeViewItem { Header = "Data Offset: " + packet.DataOffset });
            childItems.Add(new TreeViewItem { Header = "Window Size: " + packet.WindowSize });
            childItems.Add(new TreeViewItem { Header = "Checksum: " + packet.Checksum });

            TreeViewItem tcpFlagsItem = new TreeViewItem { Header = "TCP Flags: " + packet.Flags };
            var tcpFlagsChildItems = new List<TreeViewItem>();
                
            //TODO Extract flags
            childItems.Add(new TreeViewItem { Header = "Flags: " + packet.Flags});

            tcpFlagsItem.Items = tcpFlagsChildItems;
            
            childItems.Add(tcpFlagsItem);
            tcpItem.Items = childItems;

            return tcpItem;
        }
    }
}