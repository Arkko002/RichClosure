using PacketSniffer.Packets;

namespace PacketSniffer.Factories
{
    public interface IAbstractByteFactory
    {
        IPacket CreatePacket(byte[] buffer);
    }
}