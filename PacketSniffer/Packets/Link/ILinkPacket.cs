namespace PacketSniffer.Packets.Link
{
    public interface ILinkPacket
    {
        PacketProtocol? NextProtocol { get; }
        
        //TODO what types to use for interface addresses?
        string DestinationAddress { get; }
        string SourceAddress { get; }
    }
}