using System;
using System.Collections.Generic;
using System.Net;

namespace PacketSniffer.Packets.Internet_Layer
{
    public class Ip6Packet : IIpPacket
    {
        public byte TrafficClass { get; private set; }
        public UInt32 FlowLabel { get; private set; }
        public byte HopLimit { get; private set; }
        public Dictionary<string, string> Adrs { get; private set; }
        
        public byte Version { get; }
        public IPAddress SourceAddress { get; }
        public IPAddress DestinationAddress { get; }

        //TODO rework init
        public Ip6Packet(Dictionary<string, object> valueDictionary)
        {
            TrafficClass = (byte) valueDictionary["Ip6TrafficClass"];
            FlowLabel = (uint) valueDictionary["Ip6FlowLabel"];
            HopLimit = (byte) valueDictionary["Ip6HopLimit"];
            Adrs = (Dictionary<string, string>) valueDictionary["Ip6Adrs"];
        }
    }
}