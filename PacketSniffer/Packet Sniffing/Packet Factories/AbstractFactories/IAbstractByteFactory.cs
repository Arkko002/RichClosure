using PacketSniffer.Packets;

namespace PacketSniffer.Packet_Sniffing.Packet_Factories.AbstractFactories
{
    public interface IAbstractByteFactory
    {
        IPacket CreatePacket(byte[] buffer);
    }
}