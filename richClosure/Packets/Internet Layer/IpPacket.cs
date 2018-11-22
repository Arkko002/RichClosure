using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace richClosure.Packets.InternetLayer
{
    class IpFlags : IFlags
    {
        public CustomBool DF = new CustomBool();
        public CustomBool MF = new CustomBool();
        public CustomBool Res = new CustomBool();
    }

    class IpPacket : IPacket
    {

        public IpPacket()
        {
        }

        //IPacket
        public ulong PacketId { get; set; }
        public string PacketData { get; set; }
        public string TimeDateCaptured { get; set; }
        public string EthDestinationMacAdr { get; set; }
        public string EthSourceMacAdr { get; set; }
        public uint EthProtocol { get; set; }
        public byte IpVersion { get; set; }
        public UInt16 IpTotalLength { get; set; }
        public AppProtocolEnum IpAppProtocol { get; set; }
        public string PacketDisplayedProtocol { get; set; }
        public string PacketComment { get; set; }

        //IP4
        public byte Ip4HeaderLength { get; set; }
        public Dictionary<string, string> Ip4Adrs { get; set; }
        public byte Ip4Dscp { get; set; }
        public UInt16 Ip4Identification { get; set; }
        public UInt16 Ip4Offset { get; set; }
        public IpFlags Ip4Flags { get; set; }
        public byte Ip4TimeToLive { get; set; }
        public IpProtocolEnum IpProtocol { get; set; }
        public uint Ip4HeaderChecksum { get; set; }


        //IP6
        public byte Ip6TrafficClass { get; set; }
        public UInt32 Ip6FlowLabel { get; set; }
        public byte Ip6HopLimit { get; set; }
        public Dictionary<string, string> Ip6Adrs { get; set; }

    }
}
