namespace PacketSniffer.Packets.Transport
{
    public class UdpPacket : ITransportPacket
    {
        public ushort DestinationPort { get; }
        public ushort SourcePort { get; }
        public ushort Length { get; }
        public ushort Checksum { get; }

        public UdpPacket(ushort destinationPort, ushort sourcePort, ushort length, ushort checksum, IPacket? previousHeader, PacketProtocol nextProtocol)
        {
            PacketProtocol = PacketProtocol.UDP;
            DestinationPort = destinationPort;
            SourcePort = sourcePort;
            Length = length;
            Checksum = checksum;
            PreviousHeader = previousHeader;
            NextProtocol = nextProtocol;
        }
        public PacketProtocol PacketProtocol { get; }
        public IPacket? PreviousHeader { get; }
        public PacketProtocol NextProtocol { get; }
    }
}
