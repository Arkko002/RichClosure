using System;
using System.Collections.Generic;
using Avalonia.Controls;
using PacketDotNet;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Internet.Icmp;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class IcmpTreeItemFactory 
    {
        public TreeViewItem CreateTreeViewItem(IcmpV4Packet packet)
        {
            //TODO IcmpCode as Enum
            var icmpItem = new TreeViewItem() {Header = "ICMP, Type: " + Enum.GetName(packet.TypeCode) + ", Code: " + packet.TypeCode};

            var childItems = new List<TreeViewItem>();
            childItems.Add(new TreeViewItem { Header = "ICMP Iden.: : " +  packet.Id});
            childItems.Add(new TreeViewItem { Header = "ICMP Type: " +  Enum.GetName(packet.TypeCode)});
            childItems.Add(new TreeViewItem() {Header = "ICMP Code: " + packet.TypeCode});
            childItems.Add(new TreeViewItem { Header = "ICMP Checksum: " + packet.Checksum });
            childItems.Add(new TreeViewItem { Header = "ICMP Seq.: " + packet.Sequence });
            childItems.Add(new TreeViewItem { Header = "ICMP Data: " + packet.Data });
            
            icmpItem.Items = childItems;
            return icmpItem;
        }
    }
}