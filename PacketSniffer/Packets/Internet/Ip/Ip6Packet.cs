using System;
using System.Net;

namespace PacketSniffer.Packets.Internet.Ip
{
    public class Ip6Packet : IIpPacket
    {
        public byte TrafficClass { get; }
        public UInt32 FlowLabel { get; }
        public byte HopLimit { get; }
        public byte Version { get; }
        public IpProtocol NextHeaderProtocol { get; }
        
        public ushort PayloadLength { get; }


        public PacketProtocol PacketProtocol { get; }
        public IPacket? PreviousHeader { get; }
        public PacketProtocol NextProtocol { get; }
        public IPAddress DestinationAddress { get; }
        public IPAddress SourceAddress { get; }

        public Ip6Packet(byte trafficClass, uint flowLabel, byte hopLimit, byte version, IPacket? previousHeader, IPAddress sourceAddress, IPAddress destinationAddress, IpProtocol nextHeaderProtocol, ushort payloadLength, PacketProtocol nextProtocol)
        {
            PacketProtocol = PacketProtocol.IPv6;
            TrafficClass = trafficClass;
            FlowLabel = flowLabel;
            HopLimit = hopLimit;
            Version = version;
            PreviousHeader = previousHeader;
            SourceAddress = sourceAddress;
            DestinationAddress = destinationAddress;
            NextHeaderProtocol = nextHeaderProtocol;
            PayloadLength = payloadLength;
            NextProtocol = nextProtocol;
        }
    }
}