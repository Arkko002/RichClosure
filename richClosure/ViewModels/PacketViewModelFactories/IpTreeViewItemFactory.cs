using PacketSniffer.Packets;
using PacketSniffer.Packets.Internet_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace richClosure.ViewModels.PacketViewModelFactories
{
    class IpTreeViewItemFactory : IPacketTreeViewItemFactory
    {
        public TreeViewItem CreatePacketTreeViewItem(IPacket sourcePacket)
        {
            switch (sourcePacket.IpVersion)
            {
                case 4:
                    IpPacket ip4Packet = sourcePacket as IpPacket;

                    TreeViewItem ipItem = new TreeViewItem { Header = "IP4 Layer, " + "Dest: " + ip4Packet.Ip4Adrs["dst"] + ", Src: " + ip4Packet.Ip4Adrs["src"] };

                    ipItem.Items.Add(new TreeViewItem { Header = "Version: " + ip4Packet.IpVersion });
                    ipItem.Items.Add(new TreeViewItem { Header = "Header Length: " + ip4Packet.Ip4HeaderLength });
                    ipItem.Items.Add(new TreeViewItem { Header = "Protocol: " + ip4Packet.IpProtocol });
                    ipItem.Items.Add(new TreeViewItem { Header = "Dest. Address: " + ip4Packet.Ip4Adrs["dst"] });
                    ipItem.Items.Add(new TreeViewItem { Header = "Src. Address: " + ip4Packet.Ip4Adrs["src"] });
                    ipItem.Items.Add(new TreeViewItem { Header = "DSCP: " + ip4Packet.Ip4Dscp });
                    ipItem.Items.Add(new TreeViewItem { Header = "Total Length: " + ip4Packet.IpTotalLength });
                    ipItem.Items.Add(new TreeViewItem { Header = "Identification: " + ip4Packet.Ip4Identification });
                    ipItem.Items.Add(new TreeViewItem { Header = "Offset: " + ip4Packet.Ip4Offset });

                    TreeViewItem ipFlagsItem = new TreeViewItem();
                    ipFlagsItem.Items.Add(new TreeViewItem { Header = "DF - " + ip4Packet.Ip4Flags.Df });
                    ipFlagsItem.Items.Add(new TreeViewItem { Header = "MF - " + ip4Packet.Ip4Flags.Mf });
                    ipFlagsItem.Items.Add(new TreeViewItem { Header = "Res. - " + ip4Packet.Ip4Flags.Res });
                    ipItem.Items.Add(ipFlagsItem);

                    ipItem.Items.Add(new TreeViewItem { Header = "TTL: " + ip4Packet.Ip4TimeToLive });
                    ipItem.Items.Add(new TreeViewItem { Header = "Header Checksum: " + ip4Packet.Ip4HeaderChecksum });
                    return ipItem;

                case 6:
                    IpPacket ip6Packet = sourcePacket as IpPacket;
                    TreeViewItem ip6Item = new TreeViewItem { Header = "IPv6 Layer, " + "Dest: " + ip6Packet.Ip6Adrs["dst"] + ", Src: " + ip6Packet.Ip6Adrs["src"] };

                    ip6Item.Items.Add(new TreeViewItem { Header = "Version: " + ip6Packet.IpVersion });
                    ip6Item.Items.Add(new TreeViewItem { Header = "Traffic Class: " + ip6Packet.Ip6TrafficClass });
                    ip6Item.Items.Add(new TreeViewItem { Header = "Flow Label: " + ip6Packet.Ip6FlowLabel });
                    ip6Item.Items.Add(new TreeViewItem { Header = "Payload Length: " + ip6Packet.IpTotalLength });
                    ip6Item.Items.Add(new TreeViewItem { Header = "Next Header: " + ip6Packet.IpProtocol });
                    ip6Item.Items.Add(new TreeViewItem { Header = "Hop Limit: " + ip6Packet.Ip6HopLimit });
                    ip6Item.Items.Add(new TreeViewItem { Header = "Src. Adr.: " + ip6Packet.Ip6Adrs["dst"] });
                    ip6Item.Items.Add(new TreeViewItem { Header = "Dest. Adr.: " + ip6Packet.Ip6Adrs["src"] });
                    return ip6Item;
            }
        }
    }
}
