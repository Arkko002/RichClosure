namespace PacketSniffer.Packets.Link
{
    public interface ILinkPacket : IPacket
    {
        //TODO what types to use for interface addresses?
        string DestinationAddress { get; }
        string SourceAddress { get; }
    }
}