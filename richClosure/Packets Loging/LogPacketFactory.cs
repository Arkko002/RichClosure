using System;
using System.Collections.Generic;
using richClosure.Packets.TransportLayer;
using richClosure.Packets.InternetLayer;


namespace richClosure
{
    class LogPacketFactory
    {
        public IPacket CreatePacket(Dictionary<string, string> packetDict)
        {
            UInt32.TryParse(packetDict["PacketId"], out uint packetId);
            string packetData = packetDict["PacketData"];
            packetData = packetData.Replace(" ", String.Empty);
            string timeCaptured = packetDict["TimeCaptured"];

            string ethDestinationMacAdr;
            string ethSourceMacAdr;
            uint ethProtocol;

            if (packetDict.ContainsKey("EthDestinationMacAdr"))
            {
                ethDestinationMacAdr = packetDict["EthDestinationMacAdr"];
                ethSourceMacAdr = packetDict["EthSourceMacAdr"];
                ethProtocol = Convert.ToUInt32(packetDict["EthProtocol"]);
            }
            else
            {
                ethDestinationMacAdr = "UNINCLUDED";
                ethSourceMacAdr = "UNINCLUDED";
                ethProtocol = 0;
            }

            Byte.TryParse(packetDict["IpVersion"], out byte ipVersion);
            Byte.TryParse(packetDict["IpHeaderLength"], out byte ipHeaderLength);
            string ipSourceAdr = packetDict["IpSourceAdr"];
            string ipDestinationAdr = packetDict["IpDestinationAdr"];
            Enum.TryParse(packetDict["IpFlags"], out IpFlagsEnum ipFlags);
            Byte.TryParse(packetDict["IpTimeToLive"], out byte ipTimeToLive);
            Enum.TryParse(packetDict["IpProtocol"], out IpProtocolEnum ipProtocol);
            Byte.TryParse(packetDict["IpDscp"], out byte ipDscp);
            UInt16.TryParse(packetDict["IpIdentification"], out ushort ipIdentification);
            UInt16.TryParse(packetDict["IpOffset"], out ushort ipOffset);
            UInt16.TryParse(packetDict["IpTotalLength"], out ushort ipTotalLength);
            UInt32.TryParse(packetDict["IpHeaderChecksum"], out uint ipHeaderChecksum);

            //IpPacket ip4Packet = new IpPacket()
            //{
            //    PacketId = packetId,
            //    PacketData = packetData,
            //    TimeDateCaptured = timeCaptured,
            //    EthDestinationMacAdr = ethDestinationMacAdr,
            //    EthSourceMacAdr = ethSourceMacAdr,
            //    EthProtocol = ethProtocol,
            //    IpVersion = ipVersion,
            //    Ip4HeaderLength = ipHeaderLength,
            //    Ip4SourceAdr = ipSourceAdr,
            //    Ip4DestinationAdr = ipDestinationAdr,
            //    Ip4Flags = ipFlags,
            //    Ip4TimeToLive = ipTimeToLive,
            //    IpProtocol = ipProtocol,
            //    Ip4Dscp = ipDscp,
            //    IpTotalLength = ipTotalLength,
            //    Ip4Identification = ipIdentification,
            //    Ip4Offset = ipOffset,
            //    Ip4HeaderChecksum = ipHeaderChecksum,
            //};

            //switch (ipProtocol)
            //{
            //    case IpProtocolEnum.TCP:
            //        return CreateTcpPacket(packetDict, ip4Packet);

            //    case IpProtocolEnum.UDP:
            //        return CreateUdpPacket(packetDict, ip4Packet);

            //    case IpProtocolEnum.ICMP:
            //        return CreateIcmpPacket(packetDict, ip4Packet);

            //    default:
            //        return ip4Packet;
            //}

            return null;
        }

//        private IPacket CreateTcpPacket(Dictionary<string, string> packetDict, IpPacket ip4Packet)
//        {
//            UInt32.TryParse(packetDict["TcpSequenceNumber"], out uint tcpSequenceNumber);
//            UInt32.TryParse(packetDict["TcpAckNumber"], out uint tcpAckNumber);
//            Byte.TryParse(packetDict["TcpDataOffset"], out byte tcpDataOffset);
//            UInt16.TryParse(packetDict["TcpWindowSize"], out ushort tcpWindowSize);
//            UInt16.TryParse(packetDict["TcpUrgentPointer"], out ushort tcpUrgentPointer);
//            UInt16.TryParse(packetDict["TcpChecksum"], out ushort tcpChecksum);
//            UInt16.TryParse(packetDict["TcpDestinationPort"], out ushort tcpDestinationPort);
//            UInt16.TryParse(packetDict["TcpSourcePort"], out ushort tcpSourcePort);
//            string tcpFlags = packetDict["TcpFlags"];

//            IPacket tcpPacket = new TcpPacket(ip4Packet)
//            {
//                TcpSequenceNumber = tcpSequenceNumber,
//                TcpAckNumber = tcpAckNumber,
//                TcpDataOffset = tcpDataOffset,
//                TcpWindowSize = tcpWindowSize,
//                TcpUrgentPointer = tcpUrgentPointer,
//                TcpChecksum = tcpChecksum,
//                TcpDestinationPort = tcpDestinationPort,
//                TcpSourcePort = tcpSourcePort,
//                TcpFlags = tcpFlags,
//            };

//            return tcpPacket;
//        }

//        private IPacket CreateUdpPacket(Dictionary<string, string> packetDict, IpPacket ip4Packet)
//        {
//            UInt16.TryParse(packetDict["UdpChecksum"], out UInt16 udpChecksum);
//            UInt16.TryParse(packetDict["UdpLength"], out UInt16 udpLength);
//            UInt16.TryParse(packetDict["UdpSourcePort"], out UInt16 udpSourcePort);
//            UInt16.TryParse(packetDict["UdpDestinationPort"], out UInt16 udpDestinationPort);

//            IPacket udpPacket = new UdpPacket(ip4Packet)
//            {
//                UdpChecksum = udpChecksum,
//                UdpLength = udpLength,
//                UdpSourcePort = udpSourcePort,
//                UdpDestinationPort = udpDestinationPort,
//            };

//            return udpPacket;
//        }

//        private IPacket CreateIcmpPacket(Dictionary<string, string> packetDict, IpPacket ip4Packet)
//        {
//            string icmpRest = packetDict["IcmpRest"];
//            Byte.TryParse(packetDict["IcmpType"], out byte icmpType);
//            Byte.TryParse(packetDict["IcmpCode"], out byte icmpCode);
//            UInt32.TryParse(packetDict["IcmpChecksum"], out uint icmpChecksum);

//            IPacket icmpPacket = new IcmpPacket(ip4Packet)
//            {
//                IcmpChecksum = icmpChecksum,
//                IcmpCode = icmpCode,
//                IcmpType = icmpType,
//                IcmpRest = icmpRest
//            };

//            return icmpPacket;
//        }
    }
}
