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
        //IPacket
        public ulong PacketId { get; private set; }
        public string PacketData { get; private set; }
        public string TimeDateCaptured { get; private set; }
        public string EthDestinationMacAdr { get; private set; }
        public string EthSourceMacAdr { get; private set; }
        public uint EthProtocol { get; private set; }
        public byte IpVersion { get; private set; }
        public ushort IpTotalLength { get; private set; }
        public AppProtocolEnum IpAppProtocol { get; private set; }
        public string PacketDisplayedProtocol { get; private set; }
        public string PacketComment { get; private set; }

        //IP4
        public byte Ip4HeaderLength { get; private set; }
        public Dictionary<string, string> Ip4Adrs { get; private set; }
        public byte Ip4Dscp { get; private set; }
        public ushort Ip4Identification { get; private set; }
        public ushort Ip4Offset { get; private set; }
        public IpFlags Ip4Flags { get; private set; }
        public byte Ip4TimeToLive { get; private set; }
        public IpProtocolEnum IpProtocol { get; private set; }
        public uint Ip4HeaderChecksum { get; private set; }


        //IP6
        public byte Ip6TrafficClass { get; private set; }
        public UInt32 Ip6FlowLabel { get; private set; }
        public byte Ip6HopLimit { get; private set; }
        public Dictionary<string, string> Ip6Adrs { get; private set; }

        public void SetIpProperties(Dictionary<string, object> valuesDictionary)
        {
            if (!valuesDictionary.ContainsKey("IpVersion"))
            {
                throw new ArgumentException();
            }

            AssignVersionlessProperties(valuesDictionary);

            switch (valuesDictionary["IpVersion"])
            {
                case 4:
                    AssignIp4Properties(valuesDictionary);
                    break;

                case 6:
                    AssignIp6Properties(valuesDictionary);
                    break;
            }
        }

        private void AssignVersionlessProperties(Dictionary<string, object> valueDictionary)
        {
            PacketId = (ulong) valueDictionary["PacketId"];
            PacketData = (string) valueDictionary["PacketData"];
            TimeDateCaptured = (string) valueDictionary["TimeDateCaptured"];
            EthDestinationMacAdr = (string) valueDictionary["EthDestinationMacAdr"];
            EthSourceMacAdr = (string) valueDictionary["EthSourceMacAdr"];
            EthProtocol = (uint) valueDictionary["EthProtocol"];
            IpVersion = (byte) valueDictionary["IpVersion"];
            IpTotalLength = (ushort) valueDictionary["IpTotalLength"];
            IpAppProtocol = (AppProtocolEnum) valueDictionary["IpAppProtocol"];
            PacketDisplayedProtocol = (string) valueDictionary["PacketDisplayedProtocol"];
            PacketComment = (string) valueDictionary["PacketComment"];
        }

        private void AssignIp4Properties(Dictionary<string, object> valueDictionary)
        {
            Ip4HeaderLength = (byte) valueDictionary["Ip4HeaderLength"];

            Ip4Adrs = new Dictionary<string, string>
            {
                {"src", valueDictionary["Ip4SourceAdr"].ToString()},
                {"dst", valueDictionary["Ip4DestinationAdr"].ToString()}
            };

            Ip4Dscp = (byte) valueDictionary["Ip4Dscp"];
            Ip4Identification = (ushort) valueDictionary["Ip4Identification"];
            Ip4Offset = (ushort) valueDictionary["Ip4Offset"];
            Ip4Flags = (IpFlags) valueDictionary["Ip4Flags"];
            Ip4TimeToLive = (byte) valueDictionary["Ip4TimeToLive"];
            IpProtocol = (IpProtocolEnum) valueDictionary["IpProtocol"];
            Ip4HeaderChecksum = (uint) valueDictionary["Ip4HeaderChecksum"];
        }

        private void AssignIp6Properties(Dictionary<string, object> valueDictionary)
        {
            Ip6TrafficClass = (byte) valueDictionary["Ip6TrafficClass"];
            Ip6FlowLabel = (uint) valueDictionary["Ip6FlowLabel"];
            Ip6HopLimit = (byte) valueDictionary["Ip6HopLimit"];
            Ip6Adrs = (Dictionary<string, string>) valueDictionary["Ip6Adrs"];
        }
    }
}