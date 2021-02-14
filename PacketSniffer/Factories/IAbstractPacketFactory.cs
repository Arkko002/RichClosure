using PacketSniffer.Packets;

namespace PacketSniffer.Factories
{
    public interface IAbstractPacketFactory
    {
        IPacket CreatePacket();
    }
}
