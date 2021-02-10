namespace PacketSniffer.Packets
{
    public class PacketFrame : IPacketFrame
    {
        public ulong PacketId { get; }
        public string TimeDateCaptured { get; }
        public string PacketComment { get; }
        public string DestinationAddress { get; }
        public string SourceAddress { get; }
        public IPacket Packet { get; }
    }
}