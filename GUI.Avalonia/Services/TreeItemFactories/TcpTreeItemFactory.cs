using System.Collections.Generic;
using Avalonia.Controls;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Transport;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class TcpTreeItemFactory : IAbstractTreeItemFactory
    {
        public TreeViewItem CreateTreeViewItem(IPacket packet)
        {
            TcpPacket tcpPacket = packet as TcpPacket;
            var tcpItem = new TreeViewItem
            {
                Header = ", Dest. Port: " + tcpPacket.DestinationPort +
                         ", Src. Port: " + tcpPacket.SourcePort
            };

            var childItems = new List<TreeViewItem>();

            childItems.Add(new TreeViewItem { Header = "Dest. port: " + tcpPacket.DestinationPort });
            childItems.Add(new TreeViewItem { Header = "Src. port: " + tcpPacket.SourcePort });
            childItems.Add(new TreeViewItem { Header = "Seq. Number: " + tcpPacket.SequenceNumber });
            childItems.Add(new TreeViewItem { Header = "Ack Number: " + tcpPacket.AckNumber });
            childItems.Add(new TreeViewItem { Header = "Urg. Pointer: " + tcpPacket.UrgentPointer });
            childItems.Add(new TreeViewItem { Header = "Data Offset: " + tcpPacket.DataOffset });
            childItems.Add(new TreeViewItem { Header = "Window Size: " + tcpPacket.WindowSize });
            childItems.Add(new TreeViewItem { Header = "Checksum: " + tcpPacket.Checksum });

            TreeViewItem tcpFlagsItem = new TreeViewItem { Header = "TCP Flags" };
            var tcpFlagsChildItems = new List<TreeViewItem>();
                
            tcpFlagsChildItems.Add(new TreeViewItem { Header = "FIN - " + tcpPacket.Fin });
            tcpFlagsChildItems.Add(new TreeViewItem { Header = "SYN - " + tcpPacket.Syn });
            tcpFlagsChildItems.Add(new TreeViewItem { Header = "RST - " + tcpPacket.Rst });
            tcpFlagsChildItems.Add(new TreeViewItem { Header = "PSH - " + tcpPacket.Psh });
            tcpFlagsChildItems.Add(new TreeViewItem { Header = "ACK - " + tcpPacket.Ack });
            tcpFlagsChildItems.Add(new TreeViewItem { Header = "URG - " + tcpPacket.Urg });
            tcpFlagsChildItems.Add(new TreeViewItem { Header = "ECE - " + tcpPacket.Ece });
            tcpFlagsChildItems.Add(new TreeViewItem { Header = "CWR - " + tcpPacket.Cwr });
            tcpFlagsChildItems.Add(new TreeViewItem { Header = "NS - " + tcpPacket.Ns });

            childItems.Add(tcpFlagsItem);
            tcpItem.Items = childItems;

            return tcpItem;
        }
    }
}