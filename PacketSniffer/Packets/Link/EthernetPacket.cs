namespace PacketSniffer.Packets.Link
{
    public class EthernetPacket : ILinkPacket
    {
        public PacketProtocol? NextProtocol { get; }
        public short? Length { get; }
        
        public string DestinationAddress { get; }
        public string SourceAddress { get; }
        
        public int FrameCheck { get; }
    }
}