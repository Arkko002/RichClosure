using System.Collections.Generic;
using Avalonia.Controls;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Application.Dhcp;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    //TODO
    public class DhcpTreeItemFactory 
    {
        public TreeViewItem CreateTreeViewItem(IPacket packet)
        {
            DhcpPacket dhcpPacket = packet as DhcpPacket;
            var dhcpItem = new TreeViewItem() {Header = "DHCP"};

            var childItems = new List<TreeViewItem>();
            
            childItems.Add(new TreeViewItem { Header = "Opcode: " + dhcpPacket.Opcode });
            childItems.Add(new TreeViewItem { Header = "Hardware Type: " + dhcpPacket.HardType });
            childItems.Add(new TreeViewItem { Header = "Hardware Adr. Length: " + dhcpPacket.HardAdrLength });
            childItems.Add(new TreeViewItem { Header = "Hops: " + dhcpPacket.HopCount });
            childItems.Add(new TreeViewItem { Header = "Transaction ID: " + dhcpPacket.TransactionId });
            childItems.Add(new TreeViewItem { Header = "Seconds: " + dhcpPacket.NumOfSeconds });
            childItems.Add(new TreeViewItem { Header = "Flags: " + dhcpPacket.Flags });
            childItems.Add(new TreeViewItem { Header = "Client IP: " + dhcpPacket.ClientIpAdr });
            childItems.Add(new TreeViewItem { Header = "Your IP: " + dhcpPacket.YourIpAdr });
            childItems.Add(new TreeViewItem { Header = "Gateway IP: " + dhcpPacket.GatewayIpAdr });
            childItems.Add(new TreeViewItem { Header = "Client Hardware Adr.: " + dhcpPacket.ClientHardAdr });
            childItems.Add(new TreeViewItem { Header = "Server Name: " + dhcpPacket.ServerName });
            childItems.Add(new TreeViewItem { Header = "Boot Filename: " + dhcpPacket.BootFilename });

            dhcpItem.Items = childItems;
            return dhcpItem;
        }
    }
}