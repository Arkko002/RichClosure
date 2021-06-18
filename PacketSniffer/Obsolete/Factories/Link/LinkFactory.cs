using System;
using System.IO;
using PacketSniffer.Packets;

namespace PacketSniffer.Factories.Link
{
    //TODO should be responsible for determining link layer header and passing it down to specialised factory
    public class LinkFactory : ILinkFactory
    {
        private readonly IPacketFrame _frame;

        public LinkFactory(IPacketFrame frame)
        {
            _frame = frame;
        }

        public IPacket CreatePacket(BinaryReader binaryReader)
        {
            throw new NotImplementedException();
        }
    }
}