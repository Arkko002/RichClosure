using System;
using System.Collections.Generic;
using System.Net;

namespace PacketSniffer.Packets.Internet_Layer
{
    public class Ip4Packet : IIpPacket
    {
        
        public byte HeaderLength { get; private set; }
        public byte Dscp { get; private set; }
        public ushort Identification { get; private set; }
        public ushort Offset { get; }
        public IpFlags Flags { get; }
        public byte TimeToLive { get; }
        public uint HeaderChecksum { get; }
        
        public byte Version { get; }
        public IPAddress SourceAddress { get; }
        public IPAddress DestinationAddress { get; }

        //TODO rework initialization of variables
        public Ip4Packet(Dictionary<string, string> valueDictionary)
        {
            HeaderLength = (byte) valueDictionary["Ip4HeaderLength"];
            Ip4Adrs = (Dictionary<string, string>)valueDictionary["Ip4Adrs"];
            Dscp = (byte) valueDictionary["Ip4Dscp"];
            Identification = (ushort) valueDictionary["Ip4Identification"];
            Offset = Convert.ToUInt16(valueDictionary["Ip4Offset"]);
            Flags = (IpFlags) valueDictionary["Ip4Flags"];
            TimeToLive = (byte) valueDictionary["Ip4TimeToLive"];
            HeaderChecksum = Convert.ToUInt32(valueDictionary["Ip4HeaderChecksum"]);
        }
    }
}