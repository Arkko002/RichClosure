using richClosure.Packet_Sniffing.Factories.InternetFactories;
using richClosure.Packet_Sniffing.Factories.TransportFactories;
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


        //TODO PacketFactory checks only ip version, rest is performed in other factories
        public PacketFactory(BinaryReader binaryReader, byte[] buffer)
        {
            _binaryReader = binaryReader;
            _packetId = 0;
            _buffer = buffer;
        }

        public IPacket CreatePacket()
        {
            _packetId++;

            byte ipVersionAndHeaderLength = _binaryReader.ReadByte();

            byte ipVersion = ipVersionAndHeaderLength;
            ipVersion >>= 4;

            _binaryReader.BaseStream.Position = 0;

            IAbstractFactory ipFactory = CreateIpFactory(ipVersion);
            IpPacket ipPacket = ipFactory.CreatePacket() as IpPacket;

            IAbstractFactory protocolFactory = CreateProtocolFactory(ipPacket);
            

        }

        //TODO
        private IAbstractFactory CreateIpFactory(byte ipVersion)
        {
            switch (ipVersion)
            {
                case 4:
                    return new Ip4PacketFactory(_binaryReader, _buffer, _packetId);


                case 6:
                    return new Ip6PacketFactory(_binaryReader, _buffer, _packetId);

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
                    return new IcmpPacketFactory(_binaryReader, basePacket);

                case IpProtocolEnum.TCP:
                    return new TcpPacketFactory(_binaryReader, basePacket);

                case IpProtocolEnum.UDP:
                    return new UdpPacketFactory(_binaryReader, basePacket);

                default:
                    ErrorLoger.LogError(DateTime.Now.ToString(), "Unsuported IP protocol (" + ipPacket.IpProtocol.ToString() + ")", GetType(),
                        ErrorLoger.ErrorSeverity.low, string.Empty);
                    throw new ArgumentException();
            }
        }
    }
}
