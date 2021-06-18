#nullable enable
namespace PacketSniffer.Packets
{
    public interface IPacket
    {
        PacketProtocol PacketProtocol { get; }
        IPacket? PreviousHeader { get; }
        PacketProtocol NextProtocol { get; }
    }
}