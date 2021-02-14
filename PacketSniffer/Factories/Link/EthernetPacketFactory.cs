using System.IO;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Link;

namespace PacketSniffer.Factories.Link
{
    public class EthernetPacketFactory : ILinkFactory
    {

        public EthernetPacketFactory()
        {
        }

        public IPacket CreatePacket(BinaryReader binaryReader)
        {
            throw new System.NotImplementedException();
        }
    }
}