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
    public class PacketFactory : IAbstractFactory
    {
        private ulong _packetId;
        private BinaryReader _binaryReader;
        private byte[] _buffer;

        public PacketFactory(byte[] buffer)
        {
            _packetId = 0;
            _buffer = buffer;

            _binaryReader = CreateBinaryReaderFromBuffer(_buffer);
        }

        public IPacket CreatePacket()
        {
            _packetId++;
            var linkFactory = new LinkFactory(_binaryReader);
            var packet = linkFactory.CreatePacket();

            while (packet.NextProtocol != PacketProtocol.NoProtocol)
            {
                var factory = CreatePacketFactory(packet);
                var newPacket = factory.CreatePacket();
                packet = newPacket;
            }

            //TODO Wrap the packet in PacketFrame
            return packet;
        }

        private BinaryReader CreateBinaryReaderFromBuffer(byte[] buffer)
        {
            MemoryStream memoryStream = new MemoryStream(buffer);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            return binaryReader;
        }

        //TODO Get rid of switch completely
        private IAbstractFactory? CreatePacketFactory(IPacket packet)
        {
            switch (packet.NextProtocol)
            {
                case PacketProtocol.Ethernet:
                    return new EthernetPacketFactory(_binaryReader);
                case PacketProtocol.IPv4:
                    return new Ip4PacketFactory(_binaryReader, packet);
                case PacketProtocol.IPv6:
                    return new Ip6PacketFactory(_binaryReader, packet);
                case PacketProtocol.ICMP:
                    return new IcmpPacketFactory(_binaryReader, packet);
                case PacketProtocol.TCP:
                    return new TcpPacketFactory(_binaryReader, packet);
                case PacketProtocol.UDP:
                    return new UdpPacketFactory(_binaryReader, packet);
                case PacketProtocol.DNS:
                    return new DnsPacketFactory(_binaryReader, packet);
                case PacketProtocol.DHCP:
                    return new DhcpPacketFactory(_binaryReader, packet);
                case PacketProtocol.HTTP:
                    return new HttpPacketFactory(_binaryReader, packet);
                case PacketProtocol.TLS:
                    return new TlsPacketFactory(_binaryReader, packet);
                case PacketProtocol.NoProtocol:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
