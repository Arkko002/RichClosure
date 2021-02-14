using System.IO;
using PacketSniffer.Packets;

namespace PacketSniffer.Factories
{
    public interface IAbstractPacketFactory
    {
        IPacket CreatePacket(BinaryReader binaryReader);
    }
}
