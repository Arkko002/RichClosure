using System.IO;

namespace richClosure.Packet_Sniffing.Factories
{
    interface IAbstractPacketFactory
    {
        IPacket CreatePacket(IPacket packet, BinaryReader binaryReader);
    }
}
