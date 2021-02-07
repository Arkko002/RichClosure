namespace PacketSniffer.Packets.Transport_Layer
{
    public interface ITransportPacket
    {
        ushort DestinationPort { get; }
        ushort SourcePort { get; }
    }
}