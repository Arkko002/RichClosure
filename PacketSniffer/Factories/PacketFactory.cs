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
        private BinaryReader _binaryReader;
        private byte[] _buffer;

        public PacketFactory(byte[] buffer)
        {
            _packetId = 0;
            _buffer = buffer;

            _binaryReader = CreateBinaryReaderFromBuffer(_buffer);
        }

        public IPacketFrame CreatePacketFrame()
        {
            _packetId++;
            var packetFrame = new PacketFrame(_packetId, DateTime.Now);
            
            var linkFactory = new LinkFactory(_binaryReader, packetFrame);
            var packet = linkFactory.CreatePacket();

            while (packet.NextProtocol != PacketProtocol.NoProtocol)
            {
                var factory = CreatePacketFactory(packet, packetFrame);
                var newPacket = factory.CreatePacket();
                packet = newPacket;
            }

            return packetFrame;
        }

        private BinaryReader CreateBinaryReaderFromBuffer(byte[] buffer)
        {
            MemoryStream memoryStream = new(buffer);
            BinaryReader binaryReader = new(memoryStream);

            return binaryReader;
        }

        //TODO Get rid of switch completely
        private IAbstractPacketFactory? CreatePacketFactory(IPacket packet, IPacketFrame frame)
        {
            switch (packet.NextProtocol)
            {
                case PacketProtocol.Ethernet:
                    return new EthernetPacketFactory(_binaryReader);
                case PacketProtocol.IPv4:
                    return new Ip4PacketFactory(_binaryReader, packet, frame);
                case PacketProtocol.IPv6:
                    return new Ip6PacketFactory(_binaryReader, packet, frame);
                case PacketProtocol.ICMP:
                    return new IcmpPacketFactory(_binaryReader, packet, frame);
                case PacketProtocol.TCP:
                    return new TcpPacketFactory(_binaryReader, frame, packet);
                case PacketProtocol.UDP:
                    return new UdpPacketFactory(_binaryReader, packet, frame);
                case PacketProtocol.DNS:
                    return new DnsPacketFactory(_binaryReader, packet, frame);
                case PacketProtocol.DHCP:
                    return new DhcpPacketFactory(_binaryReader, packet, frame);
                case PacketProtocol.HTTP:
                    return new HttpPacketFactory(_binaryReader, packet, frame);
                case PacketProtocol.TLS:
                    return new TlsPacketFactory(_binaryReader, packet, frame);
                case PacketProtocol.NoProtocol:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
