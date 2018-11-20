using richClosure.Packet_Sniffing.Factories.InternetFactories;
using richClosure.Packet_Sniffing.Factories.TransportFactories;
using richClosure.Packets.InternetLayer;
using System;
using System.Globalization;
using System.IO;

namespace richClosure.Packet_Sniffing.Factories
{
    class PacketFactory : IAbstractBufferFactory
    {
        ulong packetId;
        private Ip4PacketFactory ip4Factory = new Ip4PacketFactory();
        private Ip6PacketFactory ip6Factory = new Ip6PacketFactory();
        private IcmpPacketFactory icmpFactory = new IcmpPacketFactory();
        private TcpPacketFactory tcpFactory = new TcpPacketFactory();
        private UdpPacketFactory udpFactory = new UdpPacketFactory();

        public PacketFactory()
        {
            packetId = 0;
        }

        public IPacket CreatePacket(byte[] buffer, BinaryReader binaryReader)
        {

            packetId++;

            string timeDateCaptured = DateTime.Now.ToString("yyyy-MM-dd / HH:mm:ss.fff",
                                                                CultureInfo.InvariantCulture);
            byte ipVersionAndHeaderLength = binaryReader.ReadByte();

            byte ipVersion = ipVersionAndHeaderLength;
            ipVersion >>= 4;

            byte ipHeaderLength = ipVersionAndHeaderLength;
            ipHeaderLength <<= 4;
            ipHeaderLength >>= 4;
            ipHeaderLength *= 4;


            IPacket ipPacket;

            switch (ipVersion)
            {
                case 4:
                    ipPacket = ip4Factory.CreatePacket(buffer, binaryReader);
                    IpPacket ip4Pac = ipPacket as IpPacket;
                    ip4Pac.PacketId = packetId;
                    ip4Pac.TimeDateCaptured = timeDateCaptured;
                    ip4Pac.IpVersion = ipVersion;
                    ip4Pac.Ip4HeaderLength = ipHeaderLength;
                    break;

                case 6:
                    ipPacket = ip6Factory.CreatePacket(buffer, binaryReader);
                    ipPacket.PacketId = packetId;
                    ipPacket.TimeDateCaptured = timeDateCaptured;
                    break;

                default:
                    ErrorLoger.LogError(DateTime.Now.ToString(), "Unsuported IP Version(" + ipVersion.ToString() + ")", GetType(),
                                ErrorLoger.ErrorSeverity.low, string.Empty);
                    return new IpPacket();
            }

            switch (ipPacket.IpProtocol)
            {
                case IpProtocolEnum.ICMP:
                    IPacket icmpPacket = icmpFactory.CreatePacket(ipPacket, binaryReader);
                    return icmpPacket;

                case IpProtocolEnum.TCP:
                    IPacket tcpPacket = tcpFactory.CreatePacket(ipPacket, binaryReader);
                    return tcpPacket;

                case IpProtocolEnum.UDP:
                    IPacket udpPacket = udpFactory.CreatePacket(ipPacket, binaryReader);
                    return udpPacket;

                default:
                    ErrorLoger.LogError(DateTime.Now.ToString(), "Unsuported IP protocol (" + ipPacket.IpProtocol.ToString() + ")", GetType(),
                        ErrorLoger.ErrorSeverity.low, string.Empty);
                    return ipPacket;
            }
        }

    }
}
