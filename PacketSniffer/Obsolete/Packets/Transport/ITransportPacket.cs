namespace PacketSniffer.Packets.Transport
{
    public interface ITransportPacket : IPacket
    {
        ushort DestinationPort { get; }
        ushort SourcePort { get; }
    }
}