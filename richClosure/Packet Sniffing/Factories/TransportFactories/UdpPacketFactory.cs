using richClosure.Packet_Sniffing.Factories.ApplicationFactories;
using richClosure.Packets.InternetLayer;
using richClosure.Packets.TransportLayer;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace richClosure.Packet_Sniffing.Factories
{
    class UdpPacketFactory : IAbstractPacketFactory
    {
        private DnsPacketFactory dnsFactory = new DnsPacketFactory();
        private DhcpPacketFactory dhcpFactory = new DhcpPacketFactory();

        public IPacket CreatePacket(IPacket packet, BinaryReader binaryReader)
        {
            UInt16 udpSourcePort = (UInt16)IPAddress.NetworkToHostOrder(
                                binaryReader.ReadInt16());
            UInt16 udpDestinationPort = (UInt16)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadInt16());
            UInt16 udpLength = (UInt16)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadInt16());
            UInt16 udpChecksum = (UInt16)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadInt16());

            IPacket udpPacket;

            IpPacket pac = packet as IpPacket;
            udpPacket = new UdpPacket(pac)
            {
                UdpChecksum = udpChecksum,
                UdpLength = udpLength,
                UdpPorts = new System.Collections.Generic.Dictionary<string, string>(),
                PacketDisplayedProtocol = "UDP"
            };

            UdpPacket udpPac = udpPacket as UdpPacket;
            udpPac.UdpPorts.Add("dst", udpDestinationPort.ToString());
            udpPac.UdpPorts.Add("src", udpSourcePort.ToString());

            switch (CheckForAppLayerPorts(udpPacket))
            {
                case AppProtocolEnum.DNS:
                    IPacket dnsPacket = dnsFactory.CreatePacket(udpPac, binaryReader);
                    return dnsPacket;

                case AppProtocolEnum.DHCP:
                    IPacket dhcpPacket = dhcpFactory.CreatePacket(udpPac, binaryReader);
                    return dhcpPacket;

                case AppProtocolEnum.NoAppProtocol:
                    return udpPacket;

                default:
                    return udpPacket;
            }
        }

        private AppProtocolEnum CheckForAppLayerPorts(IPacket packet)
        {

            UdpPacket udpPac = packet as UdpPacket;

            if (udpPac.UdpPorts.Any(x => x.Value.Equals(53)))
            {
                return AppProtocolEnum.DNS;
            }
            else if (udpPac.UdpPorts.Any(x => x.Value.Equals(67)) || (udpPac.UdpPorts.Any(x => x.Value.Equals(68))))
            {
                return AppProtocolEnum.DHCP;
            }
            else
            {
                return AppProtocolEnum.NoAppProtocol;
            }
        }
    }
}
