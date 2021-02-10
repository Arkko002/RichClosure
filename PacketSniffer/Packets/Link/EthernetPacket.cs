namespace PacketSniffer.Packets.Link
{
    //Todo
    public class EthernetPacket : ILinkPacket
    {
        public short? Length { get; }
        
        public string DestinationAddress { get; }
        public string SourceAddress { get; }
        
        public int FrameCheck { get; }
        public PacketProtocol PacketProtocol { get; }
        public IPacket? PreviousHeader { get; }
        public PacketProtocol NextProtocol { get; }
    }
}