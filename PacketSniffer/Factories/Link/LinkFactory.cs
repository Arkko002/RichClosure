using System.IO;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Link;

namespace PacketSniffer.Factories.Link
{
    //TODO should be responsible for determining link layer header and passing it down to specialised factory
    public class LinkFactory : ILinkFactory
    {
        private readonly BinaryReader _binaryReader;
        private readonly IPacketFrame _frame;

        public LinkFactory(BinaryReader binaryReader, IPacketFrame frame)
        {
            _binaryReader = binaryReader;
            _frame = frame;
        }

        public IPacket CreatePacket()
        {
            throw new System.NotImplementedException();
        }
    }
}