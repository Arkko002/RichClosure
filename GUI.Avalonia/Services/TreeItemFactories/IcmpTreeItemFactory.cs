using System;
using System.Collections.Generic;
using Avalonia.Controls;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Internet;
using PacketSniffer.Packets.Internet.Icmp;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class IcmpTreeItemFactory : IAbstractTreeItemFactory
    {
        public TreeViewItem CreateTreeViewItem(IPacket packet)
        {
            IcmpPacket icmpPacket = packet as IcmpPacket;
            //TODO IcmpCode as Enum
            var icmpItem = new TreeViewItem() {Header = "ICMP, Type: " + Enum.GetName(icmpPacket.Type) + ", Code: " + icmpPacket.Code};

            var childItems = new List<TreeViewItem>();
            childItems.Add(new TreeViewItem { Header = "ICMP Type: " +  Enum.GetName(icmpPacket.Type)});

            switch (icmpPacket.Type)
            {
                case IcmpType.DestinationUnreachable:
                    childItems.Add(new TreeViewItem { Header = "ICMP Code: " +  Enum.GetName((DestinationUnreachableCode)icmpPacket.Code)});
                    break;
                case IcmpType.RedirectMessage:
                    childItems.Add(new TreeViewItem { Header = "ICMP Code: " +  Enum.GetName((RedirectMessageCode)icmpPacket.Code)});
                    break;
                case IcmpType.TimeExceeded:
                    childItems.Add(new TreeViewItem { Header = "ICMP Code: " +  Enum.GetName((TimeExceededCode)icmpPacket.Code)});
                    break;
                case IcmpType.ParameterProblemBadIpHeader:
                    childItems.Add(new TreeViewItem { Header = "ICMP Code: " +  Enum.GetName((ParameterProblemCode)icmpPacket.Code)});
                    break;
                default:
                    childItems.Add(new TreeViewItem { Header = "ICMP Code: " +  Enum.GetName(icmpPacket.Type)});
                    break;
            }

            childItems.Add(new TreeViewItem { Header = "ICMP Checksum: " + icmpPacket.Checksum });
            childItems.Add(new TreeViewItem { Header = "ICMP Rest: " + icmpPacket.Rest });

            icmpItem.Items = childItems;
            return icmpItem;
        }
    }
}