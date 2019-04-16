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
        private ulong packetId;
        private BinaryReader _binaryReader;


        //TODO PacketFactory checks only ip version, rest is performed in other factories
        public PacketFactory(BinaryReader binaryReader)
        {
            _binaryReader = binaryReader;
            packetId = 0;
        }

        public IPacket CreatePacket(byte[] buffer)
        {
            packetId++;

            byte ipVersionAndHeaderLength = _binaryReader.ReadByte();

            byte ipVersion = ipVersionAndHeaderLength;
            ipVersion >>= 4;

            _binaryReader.BaseStream.Position = 0;
           
            IAbstractFactory ipFactory = CreateIpFactory(ipVersion);
            IpPacket ipPacket = ipFactory.CreatePacket(buffer, _binaryReader) as IpPacket;
            
            switch (ipPacket.IpProtocol)
            {
                case IpProtocolEnum.ICMP:
                    IPacket icmpPacket = icmpFactory.CreatePacket(ipPacket, _binaryReader);
                    return icmpPacket;

                case IpProtocolEnum.TCP:
                    IPacket tcpPacket = tcpFactory.CreatePacket(ipPacket, _binaryReader);
                    return tcpPacket;

                case IpProtocolEnum.UDP:
                    IPacket udpPacket = udpFactory.CreatePacket(ipPacket, _binaryReader);
                    return udpPacket;

                default:
                    ErrorLoger.LogError(DateTime.Now.ToString(), "Unsuported IP protocol (" + ipPacket.IpProtocol.ToString() + ")", GetType(),
                        ErrorLoger.ErrorSeverity.low, string.Empty);
                    return ipPacket;
            }
        }

        //TODO
        private IAbstractFactory CreateIpFactory(byte ipVersion)
        {
            switch (ipVersion)
            {
                case 4:
                    return new Ip4PacketFactory();


                case 6:
                    return new Ip6PacketFactory();

                default:
                    ErrorLoger.LogError(DateTime.Now.ToString(), "Unsuported IP Version(" + ipVersion.ToString() + ")", GetType(),
                        ErrorLoger.ErrorSeverity.low, string.Empty);
                    return null;
            }
        }
    }
}
