using PacketSniffer.Packets;

namespace PacketSniffer.Factories
{
    internal interface IAbstractFactory
    {
        IPacket CreatePacket();
    }
}
