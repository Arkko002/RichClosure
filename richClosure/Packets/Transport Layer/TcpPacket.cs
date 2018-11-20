using System;
using System.Collections.Generic;
using richClosure.Packets.InternetLayer;

namespace richClosure.Packets.TransportLayer
{
    class TcpFlags
    {
        public bool ACK { get; set; }
        public bool FIN { get; set; }
        public bool URG { get; set; }
        public bool PSH { get; set; }
        public bool RST { get; set; }
        public bool ECE { get; set; }
        public bool CWR { get; set; }
        public bool SYN { get; set; }
        public bool NS { get; set; }

        public override string ToString()
        {
            return base.ToString(); 
        }
    }

    class TcpPacket : IpPacket
    {

        public uint TcpSequenceNumber { get; set; }
        public uint TcpAckNumber { get; set; }
        public byte TcpDataOffset { get; set; }
        public UInt16 TcpUrgentPointer { get; set; }
        public UInt16 TcpWindowSize { get; set; }
        public UInt16 TcpChecksum { get; set; }
        public Dictionary<string, string> TcpPorts { get; set; }
        public TcpFlags TcpFlags { get; set; }


        public TcpPacket(IPacket packet)
        {
            IpPacket pac = packet as IpPacket;

            PacketId = pac.PacketId;
            PacketData = pac.PacketData;
            TimeDateCaptured = pac.TimeDateCaptured;
            IpProtocol = pac.IpProtocol;
            IpAppProtocol = pac.IpAppProtocol;
            PacketDisplayedProtocol = "TCP";
            IpVersion = pac.IpVersion;
            EthDestinationMacAdr = pac.EthDestinationMacAdr;
            EthSourceMacAdr = pac.EthSourceMacAdr;
            EthProtocol = pac.EthProtocol;
            IpVersion = pac.IpVersion;
            IpTotalLength = pac.IpTotalLength;
            IpProtocol = pac.IpProtocol;

            if (pac.IpVersion == 4)
            {
                Ip4HeaderLength = pac.Ip4HeaderLength;
                Ip4Adrs = pac.Ip4Adrs;
                Ip4Dscp = pac.Ip4Dscp;
                Ip4TimeToLive = pac.Ip4TimeToLive;
                IpTotalLength = pac.IpTotalLength;
                Ip4Identification = pac.Ip4Identification;
                Ip4Offset = pac.Ip4Offset;
                Ip4Flags = pac.Ip4Flags;
                Ip4HeaderChecksum = pac.Ip4HeaderChecksum;
            }
            else
            {
                Ip6TrafficClass = pac.Ip6TrafficClass;
                Ip6FlowLabel = pac.Ip6FlowLabel;
                Ip6HopLimit = pac.Ip6HopLimit;
                Ip6Adrs = pac.Ip6Adrs;
            }
        }
    }
}
