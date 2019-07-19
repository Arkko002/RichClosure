using PacketSniffer.Packets;

namespace PacketSniffer.Packet_Sniffing.Packet_Factories.AbstractFactories
{
    internal interface IAbstractFactory
    {
        IPacket CreatePacket();
    }
}
