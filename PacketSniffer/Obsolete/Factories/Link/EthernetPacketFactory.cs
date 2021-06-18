using System;
using System.IO;
using PacketSniffer.Packets;

namespace PacketSniffer.Factories.Link
{
    public class EthernetPacketFactory : ILinkFactory
    {
        public IPacket CreatePacket(BinaryReader binaryReader)
        {
            throw new NotImplementedException();
        }
    }
}