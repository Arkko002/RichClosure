using PacketSniffer.Packets;

namespace PacketSniffer.Factories
{
    public interface IAbstractFactory
    {
        IPacket CreatePacket();
    }
}
