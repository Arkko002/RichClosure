using System;
using System.Collections.Generic;

namespace richClosure.Packets.Internet_Layer
{
    class IpFlags : IFlags
    {
        public CustomBool Df = new CustomBool();
        public CustomBool Mf = new CustomBool();
        public CustomBool Res = new CustomBool();
    }

    class IpPacket : IPacket
    {
        // IPacket
        public ulong PacketId { get; private set; }
        public string PacketData { get; private set; }
        public string TimeDateCaptured { get; private set; }
        public byte IpVersion { get; private set; }
        public ushort IpTotalLength { get; private set; }
        public AppProtocolEnum IpAppProtocol { get; private set; }
        public IpProtocolEnum IpProtocol { get; private set; }
        public string PacketDisplayedProtocol { get; private set; }
        public string PacketComment { get; private set; }

        // IP4
        public byte Ip4HeaderLength { get; private set; }
        public Dictionary<string, string> Ip4Adrs { get; private set; }
        public byte Ip4Dscp { get; private set; }
        public ushort Ip4Identification { get; private set; }
        public ushort Ip4Offset { get; private set; }
        public IpFlags Ip4Flags { get; private set; }
        public byte Ip4TimeToLive { get; private set; }
        public uint Ip4HeaderChecksum { get; private set; }

        // IP6
        public byte Ip6TrafficClass { get; private set; }
        public UInt32 Ip6FlowLabel { get; private set; }
        public byte Ip6HopLimit { get; private set; }
        public Dictionary<string, string> Ip6Adrs { get; private set; }

        public IpPacket(Dictionary<string, object> valuesDictionary)
        {
            SetPacketValues(valuesDictionary);
        }

        public void SetDisplayedProtocol(string protocol)
        {
            PacketDisplayedProtocol = protocol;
        }

        public void SetPacketValues(Dictionary<string, object> valuesDictionary)
        {
            if (!valuesDictionary.ContainsKey("IpVersion"))
            {
                throw new ArgumentException();
            }

            AssignVersionlessProperties(valuesDictionary);

            switch (Convert.ToInt32(valuesDictionary["IpVersion"]))
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
            TimeDateCaptured = (string) valueDictionary["DateTimeCaptured"];
            IpVersion = (byte) valueDictionary["IpVersion"];
            IpTotalLength = (ushort) valueDictionary["IpTotalLength"];
            IpAppProtocol = (AppProtocolEnum) valueDictionary["AppProtocol"];
            IpProtocol = (IpProtocolEnum)Convert.ToInt32(valueDictionary["IpProtocol"]);
            valueDictionary.TryGetValue("PacketComment", out object comment);
            PacketComment = (string) comment;
        }

        private void AssignIp4Properties(Dictionary<string, object> valueDictionary)
        {
            Ip4HeaderLength = (byte) valueDictionary["Ip4HeaderLength"];

            Ip4Adrs = (Dictionary<string, string>)valueDictionary["Ip4Adrs"];
            Ip4Dscp = (byte) valueDictionary["Ip4Dscp"];
            Ip4Identification = (ushort) valueDictionary["Ip4Identification"];
            Ip4Offset = Convert.ToUInt16(valueDictionary["Ip4Offset"]);
            Ip4Flags = (IpFlags) valueDictionary["Ip4Flags"];
            Ip4TimeToLive = (byte) valueDictionary["Ip4TimeToLive"];
            Ip4HeaderChecksum = Convert.ToUInt32(valueDictionary["Ip4HeaderChecksum"]);
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