using System.IO;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Link;

namespace PacketSniffer.Factories.Link
{
    public class EthernetPacketFactory : ILinkFactory
    {
        private readonly BinaryReader _binaryReader;

        public EthernetPacketFactory(BinaryReader binaryReader)
        {
            _binaryReader = binaryReader;
        }

        public IPacket CreatePacket()
        {
            throw new System.NotImplementedException();
        }
    }
}