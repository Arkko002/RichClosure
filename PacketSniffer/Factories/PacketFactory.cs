using System;
using System.Collections.Generic;
using System.IO;
using PacketSniffer.Factories.Application;
using PacketSniffer.Factories.Internet;
using PacketSniffer.Factories.Internet.Ip;
using PacketSniffer.Factories.Link;
using PacketSniffer.Factories.Transport;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Link;

namespace PacketSniffer.Factories
{
    //TODO Rework this to loop through NextProtocols and automatize creation of factories based on it
    public class PacketFactory : IAbstractFrameFactory
    {
        private ulong _packetId;

        public PacketFactory()
        {
            _packetId = 0;
        }

        public IPacketFrame CreatePacketFrame(byte[] buffer)
        {
            _packetId++;
            var stream = new MemoryStream(buffer);
            var binaryReader = new BinaryReader(stream);
            
            var packetFrame = new PacketFrame(_packetId, DateTime.Now);
            
            var linkFactory = new LinkFactory(packetFrame);
            var packet = linkFactory.CreatePacket(binaryReader);

            while (packet.NextProtocol != PacketProtocol.NoProtocol)
            {
                var factory = CreatePacketFactory(packet, packetFrame);
                var newPacket = factory.CreatePacket(binaryReader);
                packet = newPacket;
            }

            return packetFrame;
        }

        //TODO Get rid of switch completely
        private IAbstractPacketFactory? CreatePacketFactory(IPacket packet, IPacketFrame frame)
        {
            switch (packet.NextProtocol)
            {
                case PacketProtocol.Ethernet:
                    return new EthernetPacketFactory();
                case PacketProtocol.IPv4:
                    return new Ip4PacketFactory(packet, frame);
                case PacketProtocol.IPv6:
                    return new Ip6PacketFactory(packet, frame);
                case PacketProtocol.ICMP:
                    return new IcmpPacketFactory(packet, frame);
                case PacketProtocol.TCP:
                    return new TcpPacketFactory(frame, packet);
                case PacketProtocol.UDP:
                    return new UdpPacketFactory(packet, frame);
                case PacketProtocol.DNS:
                    return new DnsPacketFactory(packet, frame);
                case PacketProtocol.DHCP:
                    return new DhcpPacketFactory(packet, frame);
                case PacketProtocol.HTTP:
                    return new HttpPacketFactory(packet, frame);
                case PacketProtocol.TLS:
                    return new TlsPacketFactory(packet, frame);
                case PacketProtocol.NoProtocol:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
