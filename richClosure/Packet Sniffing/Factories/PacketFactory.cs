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

            byte ipVersion = GetPacketIpVersionAndResetStreamPosition();

            IAbstractFactory ipFactory = CreateIpFactory(ipVersion);
            IpPacket ipPacket = ipFactory.CreatePacket() as IpPacket;

            IAbstractFactory protocolFactory = CreateProtocolFactory(ipPacket);
            IPacket protocolPacket = protocolFactory.CreatePacket();

            IAbstractFactory applicationFactory = CreateApplicationFactory(protocolPacket);

            if (applicationFactory is null)
            {
                return protocolPacket;
            }

            IPacket applicationPacket = applicationFactory.CreatePacket();
            return applicationPacket;
        }

        private byte GetPacketIpVersionAndResetStreamPosition()
        {
            byte ipVersionAndHeaderLength = _binaryReader.ReadByte();

            byte ipVersion = ipVersionAndHeaderLength;
            ipVersion >>= 4;

            _binaryReader.BaseStream.Position = 0;

            return ipVersion;
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

        private IAbstractFactory CreateApplicationFactory(IPacket basePacket)
        {
            switch (basePacket.PacketDisplayedProtocol)
            {
                case "UDP":
                    return CreateUdpApplicationFactory(basePacket);

                case "TCP":
                    return CreateTcpApplicationFactory(basePacket);

                default:
                    ErrorLoger.LogError(DateTime.Now.ToString(), "Unknown Error in PacketFactory", GetType(),
                        ErrorLoger.ErrorSeverity.medium, string.Empty);
                    return null;
            }
        }

        private IAbstractFactory CreateTcpApplicationFactory(IPacket basePacket)
        {
            TcpPacketFactory tcpFactory = new TcpPacketFactory(_binaryReader, _valueDictionary);
            switch (tcpFactory.CheckForAppLayerPorts(basePacket))
            {
                case AppProtocolEnum.DNS:
                    return new DnsPacketFactory(_binaryReader, _valueDictionary);

                case AppProtocolEnum.HTTP:
                    return new HttpPacketFactory(_binaryReader, _valueDictionary);

                case AppProtocolEnum.TLS:
                    return new TlsPacketFactory(_binaryReader, _valueDictionary);

                case AppProtocolEnum.NoAppProtocol:
                    return null;

                default:
                    return null;
            }
        }

        private IAbstractFactory CreateUdpApplicationFactory(IPacket basePacket)
        {
            UdpPacketFactory udpFactory = new UdpPacketFactory(_binaryReader, _valueDictionary);
            switch (udpFactory.CheckForAppLayerPorts(basePacket))
            {
                case AppProtocolEnum.DNS:
                    return new DnsPacketFactory(_binaryReader, _valueDictionary);

                case AppProtocolEnum.DHCP:
                    return new DhcpPacketFactory(_binaryReader, _valueDictionary);

                case AppProtocolEnum.NoAppProtocol:
                    return null;

                default:
                    return null;
            }
        }
    }
}
