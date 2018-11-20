using richClosure.Packet_Sniffing.Factories.ApplicationFactories;
using richClosure.Packets.InternetLayer;
using richClosure.Packets.TransportLayer;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace richClosure.Packet_Sniffing.Factories.TransportFactories
{
    class TcpPacketFactory : IAbstractPacketFactory
    {
        private DnsPacketFactory dnsFactory = new DnsPacketFactory();
        private HttpPacketFactory httpFactory = new HttpPacketFactory();
        private TlsPacketFactory tlsFactory = new TlsPacketFactory();

        public IPacket CreatePacket(IPacket packet, BinaryReader binaryReader)
        {
            UInt16 tcpSourcePort = (UInt16)IPAddress.NetworkToHostOrder(
                                           binaryReader.ReadInt16());
            UInt16 tcpDestinationPort = (UInt16)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadInt16());

            UInt32 tcpSequenceNum = (UInt32)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadInt32());
            UInt32 tcpAckNum = (UInt32)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadInt32());

            byte tcpDataOffsetAndNs = binaryReader.ReadByte();
            byte tcpFlags = binaryReader.ReadByte();

            UInt16 tcpWindowSize = (UInt16)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadInt16());
            UInt16 tcpChecksum = (UInt16)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadInt16());
            UInt16 tcpUrgPtr = (UInt16)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadInt16());

            byte tcpDataOffset = tcpDataOffsetAndNs <<= 4;

            byte tcpNs = tcpDataOffsetAndNs;
            tcpNs <<= 7;
            tcpNs >>= 7;

            StringBuilder tcpFlagsBinStr = new StringBuilder();
            tcpFlagsBinStr.Append(Convert.ToString(tcpFlags, 2));

            while (tcpFlagsBinStr.Length != 8)
            {
                tcpFlagsBinStr = tcpFlagsBinStr.Insert(0, "0");
            }

            tcpFlagsBinStr = tcpFlagsBinStr.Insert(0, tcpNs.ToString());
            int tcpFlagsInt = Convert.ToInt32(tcpFlagsBinStr.ToString());

            TcpFlags flags = new TcpFlags();
            
            if((tcpFlagsInt & 1) != 0)
            {
                flags.FIN = true;
            }
            if((tcpFlagsInt & 2) != 0)
            {
                flags.SYN = true;
            }
            if((tcpFlagsInt & 4) != 0)
            {
                flags.RST = true;
            }
            if((tcpFlagsInt & 8) != 0)
            {
                flags.PSH = true;
            }
            if((tcpFlagsInt & 16) != 0)
            {
                flags.ACK = true;
            }
            if((tcpFlagsInt & 32) != 0)
            {
                flags.URG = true;
            }
            if((tcpFlagsInt & 64) != 0)
            {
                flags.ECE = true;
            }
            if((tcpFlagsInt & 128) != 0)
            {
                flags.CWR = true;
            }
            if((tcpFlagsInt & 256) != 0)
            {
                flags.NS = true;
            }

            if (tcpDataOffset > 5)
            {

            }

            IpPacket pac = packet as IpPacket;
            IPacket tcpPacket = new TcpPacket(pac)
            {
                TcpSequenceNumber = tcpSequenceNum,
                TcpAckNumber = tcpAckNum,
                TcpDataOffset = tcpDataOffset,
                TcpWindowSize = tcpWindowSize,
                TcpPorts = new System.Collections.Generic.Dictionary<string, string>(),
                TcpUrgentPointer = tcpUrgPtr,
                TcpChecksum = tcpChecksum,
                TcpFlags = flags,
                PacketDisplayedProtocol = "TCP"
            };

            TcpPacket tcpPac = tcpPacket as TcpPacket;

            tcpPac.TcpPorts.Add("dst", tcpDestinationPort.ToString());
            tcpPac.TcpPorts.Add("src", tcpSourcePort.ToString());

            switch (CheckForAppLayerPorts(tcpPacket))
            {
                case AppProtocolEnum.DNS:
                    IPacket dnsPacket = dnsFactory.CreatePacket(tcpPac, binaryReader);
                    return dnsPacket;

                case AppProtocolEnum.HTTP:
                    IPacket httpPacket = httpFactory.CreatePacket(tcpPac, binaryReader);
                    return httpPacket;

                case AppProtocolEnum.TLS:
                    IPacket tlsPacket = tlsFactory.CreatePacket(tcpPac, binaryReader);
                    return tlsPacket;

                case AppProtocolEnum.NoAppProtocol:
                    return tcpPac;

                default:
                    return tcpPac;
            }
        }

        private AppProtocolEnum CheckForAppLayerPorts(IPacket packet)
        {

            TcpPacket tcpPac = packet as TcpPacket;

            if (tcpPac.TcpPorts.Any(x => x.Value.Equals(53)))
            {
                return AppProtocolEnum.DNS;
            }
            else if (tcpPac.TcpPorts.Any(x => x.Value.Equals(80)))
            {
                return AppProtocolEnum.HTTP;
            }
            else if (tcpPac.TcpPorts.Any(x => x.Value.Equals(443)))
            {
                return AppProtocolEnum.TLS;
            }
            else
            {
                return AppProtocolEnum.NoAppProtocol;
            }

        }
    }
}
