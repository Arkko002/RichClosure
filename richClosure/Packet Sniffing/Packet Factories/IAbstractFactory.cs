using richClosure.Packets;

namespace richClosure.Packet_Sniffing.Packet_Factories
{
    interface IAbstractFactory
    {
        IPacket CreatePacket();
    }
}
