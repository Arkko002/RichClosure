using System;
using System.Collections.Generic;
using System.IO;
using PacketSniffer.Factories.ApplicationFactories;
using PacketSniffer.Factories.InternetFactories;
using PacketSniffer.Factories.TransportFactories;
using PacketSniffer.Packets;

namespace PacketSniffer.Factories
{
    // TODO Stop returning nulls
    // TODO Check data types, get rid of useless casting
    // TODO Open-closed this
    internal class PacketByteFactory : IAbstractByteFactory
    {
        private ulong _packetId;
        private BinaryReader _binaryReader;
        private byte[] _buffer;
        private readonly Dictionary<string, object> _valueDictionary = new Dictionary<string, object>();

        public PacketByteFactory()
        {
            _packetId = 0;
        }
        
        public IPacket CreatePacket(byte[] buffer)
        {
            _packetId++;

            _buffer = buffer;
            _binaryReader = CreateBinaryReaderFromBuffer(buffer);

            byte ipVersion = GetPacketIpVersionAndResetStreamPosition();

            IAbstractFactory ipByteFactory = CreateIpFactory(ipVersion);
            IPacket ipPacket = ipByteFactory.CreatePacket();

            IAbstractFactory protocolByteFactory = CreateProtocolFactory(ipPacket);
            IPacket protocolPacket = protocolByteFactory.CreatePacket();

            IAbstractFactory applicationByteFactory = CreateApplicationFactory(protocolPacket);

            if (applicationByteFactory is null)
            {
                return protocolPacket;
            }

            IPacket applicationPacket = applicationByteFactory.CreatePacket();
            return applicationPacket;
        }

        private BinaryReader CreateBinaryReaderFromBuffer(byte[] buffer)
        {
            MemoryStream memoryStream = new MemoryStream(buffer);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            return binaryReader;
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
                    return new Ip4PacketByteFactory(_binaryReader, _buffer, _packetId, _valueDictionary);

                case 6:
                    return new Ip6PacketByteFactory(_binaryReader, _buffer, _packetId, _valueDictionary);

                default:
                    //TODO
//                    ErrorLogger.LogError(DateTime.Now.ToString(CultureInfo.CurrentCulture), "Unsuported IP Version(" + ipVersion + ")", GetType(),
//                        ErrorLogger.ErrorSeverity.Low, string.Empty);
                    throw new ArgumentException();
            }
        }

        private IAbstractFactory CreateProtocolFactory(IPacket basePacket)
        {

            switch (basePacket.IpProtocol)
            {
                case IpProtocolEnum.Icmp:
                    return new IcmpPacketByteFactory(_binaryReader, _valueDictionary);

                case IpProtocolEnum.Tcp:
                    return new TcpPacketByteFactory(_binaryReader, _valueDictionary);

                case IpProtocolEnum.Udp:
                    return new UdpPacketByteFactory(_binaryReader, _valueDictionary);

                default:
                    //TODO
//                    ErrorLogger.LogError(DateTime.Now.ToString(CultureInfo.CurrentCulture), "Unsuported IP protocol (" + basePacket.IpProtocol + ")", GetType(),
//                        ErrorLogger.ErrorSeverity.Low, string.Empty);
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
                    //TODO
//                    ErrorLogger.LogError(DateTime.Now.ToString(CultureInfo.CurrentCulture), "Unknown Error in PacketFactory", GetType(),
//                        ErrorLogger.ErrorSeverity.Medium, string.Empty);
                    return null;
            }
        }

        private IAbstractFactory CreateTcpApplicationFactory(IPacket basePacket)
        {
            TcpPacketByteFactory tcpByteFactory = new TcpPacketByteFactory(_binaryReader, _valueDictionary);
            switch (tcpByteFactory.CheckForAppLayerPorts(basePacket))
            {
                case AppProtocolEnum.Dns:
                    return new DnsPacketByteFactory(_binaryReader, _valueDictionary);

                case AppProtocolEnum.Http:
                    return new HttpPacketByteFactory(_binaryReader, _valueDictionary);

                case AppProtocolEnum.Tls:
                    return new TlsPacketByteFactory(_binaryReader, _valueDictionary);

                case AppProtocolEnum.NoAppProtocol:
                    return null;

                default:
                    return null;
            }
        }

        private IAbstractFactory CreateUdpApplicationFactory(IPacket basePacket)
        {
            UdpPacketByteFactory udpByteFactory = new UdpPacketByteFactory(_binaryReader, _valueDictionary);
            switch (udpByteFactory.CheckForAppLayerPorts(basePacket))
            {
                case AppProtocolEnum.Dns:
                    return new DnsPacketByteFactory(_binaryReader, _valueDictionary);

                case AppProtocolEnum.Dhcp:
                    return new DhcpPacketByteFactory(_binaryReader, _valueDictionary);

                case AppProtocolEnum.NoAppProtocol:
                    return null;

                default:
                    return null;
            }
        }
    }
}
