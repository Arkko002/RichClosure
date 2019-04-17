using richClosure.Packet_Sniffing.Factories.InternetFactories;
using richClosure.Packet_Sniffing.Factories.TransportFactories;
using richClosure.Packet_Sniffing.Factories.ApplicationFactories;
using richClosure.Packets.InternetLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace richClosure.Packet_Sniffing.Factories
{
    class PacketFactory : IAbstractFactory
    {
        private ulong _packetId;
        private BinaryReader _binaryReader;
        private byte[] _buffer;
        private Dictionary<string, object> _valueDictionary = new Dictionary<string, object>();

        //TODO PacketFactory checks only ip version, rest is performed in other factories
        public PacketFactory(BinaryReader binaryReader, byte[] buffer)
        {
            _binaryReader = binaryReader;
            _packetId = 0;
            _buffer = buffer;
        }

        public IPacket CreatePacket()
        {
            Dictionary<string, object> valueDict = new Dictionary<string, object>();
            _packetId++;

            byte ipVersionAndHeaderLength = _binaryReader.ReadByte();

            byte ipVersion = ipVersionAndHeaderLength;
            ipVersion >>= 4;

            _binaryReader.BaseStream.Position = 0;

            IAbstractFactory ipFactory = CreateIpFactory(ipVersion);
            IpPacket ipPacket = ipFactory.CreatePacket() as IpPacket;

            IAbstractFactory protocolFactory = CreateProtocolFactory(ipPacket);
            IPacket protocolPacket = protocolFactory.CreatePacket();

            IAbstractFactory applicationFactory;
            switch (protocolPacket.PacketDisplayedProtocol)
            {
                case "UDP":
                    applicationFactory = CreateUdpApplicationFactory(protocolPacket, protocolFactory as UdpPacketFactory);
                    break;

                case "TCP":
                    applicationFactory = CreateTcpApplicationFactory(protocolPacket, protocolFactory as TcpPacketFactory);
                    break;

                default:
                    ErrorLoger.LogError(DateTime.Now.ToString(), "Unknown Error in PacketFactory", GetType(),
                        ErrorLoger.ErrorSeverity.medium, string.Empty);
                    return null;
            }

            IPacket applicationPacket = applicationFactory.CreatePacket();
        }

        private IAbstractFactory CreateIpFactory(byte ipVersion)
        {
            switch (ipVersion)
            {
                case 4:
                    return new Ip4PacketFactory(_binaryReader, _buffer, _packetId, _valueDictionary);


                case 6:
                    return new Ip6PacketFactory(_binaryReader, _buffer, _packetId, _valueDictionary);

                default:
                    ErrorLoger.LogError(DateTime.Now.ToString(), "Unsuported IP Version(" + ipVersion.ToString() + ")", GetType(),
                        ErrorLoger.ErrorSeverity.low, string.Empty);
                    throw new ArgumentException();
            }
        }

        private IAbstractFactory CreateProtocolFactory(IPacket basePacket)
        {

            switch (basePacket.IpProtocol)
            {
                case IpProtocolEnum.ICMP:
                    return new IcmpPacketFactory(_binaryReader, _valueDictionary);

                case IpProtocolEnum.TCP:
                    return new TcpPacketFactory(_binaryReader, _valueDictionary);

                case IpProtocolEnum.UDP:
                    return new UdpPacketFactory(_binaryReader, _valueDictionary);

                default:
                    ErrorLoger.LogError(DateTime.Now.ToString(), "Unsuported IP protocol (" + basePacket.IpProtocol.ToString() + ")", GetType(),
                        ErrorLoger.ErrorSeverity.low, string.Empty);
                    throw new ArgumentException();
            }
        }

        private IAbstractFactory CreateTcpApplicationFactory(IPacket basePacket, TcpPacketFactory tcpFactory)
        {
            switch (tcpFactory.CheckForAppLayerPorts(basePacket))
            {
                case AppProtocolEnum.DNS:
                    return new DnsPacketFactory();

                case AppProtocolEnum.HTTP:
                    return new HttpPacketFactory();

                case AppProtocolEnum.TLS:
                    return new TlsPacketFactory();

                case AppProtocolEnum.NoAppProtocol:
                    return null;

                default:
                    return null;
            }
        }

        private IAbstractFactory CreateUdpApplicationFactory(IPacket basePacket, UdpPacketFactory udpFactory)
        {
            switch (udpFactory.CheckForAppLayerPorts(basePacket))
            {
                case AppProtocolEnum.DNS:
                    return new DnsPacketFactory();

                case AppProtocolEnum.DHCP:
                    return new DhcpPacketFactory();

                case AppProtocolEnum.NoAppProtocol:
                    return null;

                default:
                    return null;
            }
        }
    }
}
