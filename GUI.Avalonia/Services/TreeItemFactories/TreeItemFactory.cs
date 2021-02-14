using System;
using Avalonia.Controls;
using PacketSniffer.Packets;

namespace richClosure.Avalonia.Services.TreeItemFactories
{
    public class TreeItemFactory : IAbstractTreeItemFactory
    {
        public TreeViewItem CreateTreeViewItem(IPacket packet)
        {
            IAbstractTreeItemFactory factory;
            switch (packet.PacketProtocol)
            {
                case PacketProtocol.NoProtocol:
                    throw new ArgumentOutOfRangeException();
                case PacketProtocol.Ethernet:
                    factory = new EthernetTreeItemFactory();
                    return factory.CreateTreeViewItem(packet);
                case PacketProtocol.ICMP:
                    factory = new IcmpTreeItemFactory();
                    return factory.CreateTreeViewItem(packet);
                case PacketProtocol.IPv4:
                    factory = new Ipv4TreeItemFactory();
                    return factory.CreateTreeViewItem(packet);
                case PacketProtocol.IPv6:
                    factory = new Ipv6TreeItemFactory();
                    return factory.CreateTreeViewItem(packet);
                case PacketProtocol.TCP:
                    factory = new TcpTreeItemFactory();
                    return factory.CreateTreeViewItem(packet);
                case PacketProtocol.UDP:
                    factory = new UdpTreeItemFactory();
                    return factory.CreateTreeViewItem(packet);
                case PacketProtocol.TLS:
                    factory = new TlsTreeItemFactory();
                    return factory.CreateTreeViewItem(packet);
                case PacketProtocol.HTTP:
                    factory = new HttpTreeItemFactory();
                    return factory.CreateTreeViewItem(packet);
                case PacketProtocol.DNS:
                    factory = new DnsTreeItemFactory();
                    return factory.CreateTreeViewItem(packet);
                case PacketProtocol.DHCP:
                    factory = new DhcpTreeItemFactory();
                    return factory.CreateTreeViewItem(packet);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}