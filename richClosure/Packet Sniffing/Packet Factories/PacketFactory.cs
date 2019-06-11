using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using richClosure.Packet_Sniffing.Packet_Factories.ApplicationFactories;
using richClosure.Packet_Sniffing.Packet_Factories.InternetFactories;
using richClosure.Packet_Sniffing.Packet_Factories.TransportFactories;
using richClosure.Packets;
using richClosure.Packets.Internet_Layer;

namespace richClosure.Packet_Sniffing.Packet_Factories
{
    // TODO Stop returning nulls
    // TODO Check data types, get rid of useless casting
    // TODO Open-closed this
    public class PacketFactory : IAbstractFactory
    {
        private ulong _packetId;
        private readonly BinaryReader _binaryReader;
        private readonly byte[] _buffer;
        private readonly Dictionary<string, object> _valueDictionary = new Dictionary<string, object>();

        public PacketFactory(BinaryReader binaryReader, byte[] buffer)
        {
            _binaryReader = binaryReader;
            _packetId = 0;
            _buffer = buffer;
        }

        public IPacket CreatePacket()
        {
            _packetId++;

            byte ipVersion = GetPacketIpVersionAndResetStreamPosition();

            IAbstractFactory ipFactory = CreateIpFactory(ipVersion);
            IPacket ipPacket = ipFactory.CreatePacket();

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
                    ErrorLogger.LogError(DateTime.Now.ToString(CultureInfo.CurrentCulture), "Unsuported IP Version(" + ipVersion + ")", GetType(),
                        ErrorLogger.ErrorSeverity.Low, string.Empty);
                    throw new ArgumentException();
            }
        }

        private IAbstractFactory CreateProtocolFactory(IPacket basePacket)
        {

            switch (basePacket.IpProtocol)
            {
                case IpProtocolEnum.Icmp:
                    return new IcmpPacketFactory(_binaryReader, _valueDictionary);

                case IpProtocolEnum.Tcp:
                    return new TcpPacketFactory(_binaryReader, _valueDictionary);

                case IpProtocolEnum.Udp:
                    return new UdpPacketFactory(_binaryReader, _valueDictionary);

                default:
                    ErrorLogger.LogError(DateTime.Now.ToString(CultureInfo.CurrentCulture), "Unsuported IP protocol (" + basePacket.IpProtocol + ")", GetType(),
                        ErrorLogger.ErrorSeverity.Low, string.Empty);
                    throw new ArgumentException();
            }
        }

        private IAbstractFactory CreateApplicationFactory(IPacket basePacket)
        {
            switch (basePacket.IpProtocol)
            {
                case IpProtocolEnum.Udp:
                    return CreateUdpApplicationFactory(basePacket);

                case IpProtocolEnum.Tcp:
                    return CreateTcpApplicationFactory(basePacket);

                default:
                    ErrorLogger.LogError(DateTime.Now.ToString(CultureInfo.CurrentCulture), "Unknown Error in PacketFactory", GetType(),
                        ErrorLogger.ErrorSeverity.Medium, string.Empty);
                    return null;
            }
        }

        private IAbstractFactory CreateTcpApplicationFactory(IPacket basePacket)
        {
            TcpPacketFactory tcpFactory = new TcpPacketFactory(_binaryReader, _valueDictionary);
            switch (tcpFactory.CheckForAppLayerPorts(basePacket))
            {
                case AppProtocolEnum.Dns:
                    return new DnsPacketFactory(_binaryReader, _valueDictionary);

                case AppProtocolEnum.Http:
                    return new HttpPacketFactory(_binaryReader, _valueDictionary);

                case AppProtocolEnum.Tls:
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
                case AppProtocolEnum.Dns:
                    return new DnsPacketFactory(_binaryReader, _valueDictionary);

                case AppProtocolEnum.Dhcp:
                    return new DhcpPacketFactory(_binaryReader, _valueDictionary);

                case AppProtocolEnum.NoAppProtocol:
                    return null;

                default:
                    return null;
            }
        }
    }
}
