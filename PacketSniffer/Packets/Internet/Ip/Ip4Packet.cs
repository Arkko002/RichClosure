using System.Net;

namespace PacketSniffer.Packets.Internet.Ip
{
    public class Ip4Packet : IIpPacket
    {
        public byte HeaderLength { get; }
        public byte Dscp { get; }
        public ushort Identification { get; }
        public ushort Offset { get; }
        public byte Flags { get; }
        public byte TimeToLive { get; }
        public uint HeaderChecksum { get; }
        
        public ushort TotalLength { get; }
        
        public byte Version { get; }
        public IpProtocol NextHeaderProtocol { get; }


        public PacketProtocol PacketProtocol { get; }
        public IPacket? PreviousHeader { get; }
        public PacketProtocol NextProtocol { get; }
        public IPAddress SourceAddress { get; }
        public IPAddress DestinationAddress { get; }

        public Ip4Packet(byte headerLength, byte dscp, ushort identification, ushort offset, byte flags, byte timeToLive,
            uint headerChecksum, byte version, IPacket? previousHeader, IPAddress sourceAddress, IPAddress destinationAddress, IpProtocol nextHeaderProtocol, ushort totalLength, PacketProtocol nextProtocol)
        {
            PacketProtocol = PacketProtocol.IPv4;
            HeaderLength = headerLength;
            Dscp = dscp;
            Identification = identification;
            Offset = offset;
            Flags = flags;
            TimeToLive = timeToLive;
            HeaderChecksum = headerChecksum;
            Version = version;
            PreviousHeader = previousHeader;
            SourceAddress = sourceAddress;
            DestinationAddress = destinationAddress;
            NextHeaderProtocol = nextHeaderProtocol;
            TotalLength = totalLength;
            NextProtocol = nextProtocol;
        }
    }
}