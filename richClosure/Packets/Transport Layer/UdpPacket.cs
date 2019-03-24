using System;
using System.Collections.Generic;
using richClosure.Packets.InternetLayer;

namespace richClosure.Packets.TransportLayer
{
    class UdpPacket : IpPacket
    {
        public ushort UdpLength { get; set; }
        public ushort UdpChecksum { get; set; }
        public Dictionary<string, string> UdpPorts { get; set; }

        public UdpPacket(IPacket packet)
        {
            IpPacket pac = packet as IpPacket;

            PacketId = pac.PacketId;
            PacketData = pac.PacketData;
            TimeDateCaptured = pac.TimeDateCaptured;
            EthDestinationMacAdr = pac.EthDestinationMacAdr;
            EthSourceMacAdr = pac.EthSourceMacAdr;
            EthProtocol = pac.EthProtocol;
            IpVersion = pac.IpVersion;
            IpAppProtocol = pac.IpAppProtocol;
            PacketDisplayedProtocol = "UDP";
            IpProtocol = pac.IpProtocol;
            IpTotalLength = pac.IpTotalLength;
            IpProtocol = pac.IpProtocol;

            if (pac.IpVersion == 4)
            {
                Ip4HeaderChecksum = pac.Ip4HeaderChecksum;
                Ip4HeaderLength = pac.Ip4HeaderLength;
                Ip4Adrs = pac.Ip4Adrs;
                Ip4Dscp = pac.Ip4Dscp;
                Ip4TimeToLive = pac.Ip4TimeToLive;
                IpTotalLength = pac.IpTotalLength;
                Ip4Identification = pac.Ip4Identification;
                Ip4Offset = pac.Ip4Offset;
                Ip4Flags = pac.Ip4Flags;

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
