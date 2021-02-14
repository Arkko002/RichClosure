using System;
using System.IO;
using System.Net;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Internet;
using PacketSniffer.Packets.Internet.Icmp;

namespace PacketSniffer.Factories.Internet
{
    internal class IcmpPacketFactory : IInternetPacketFactory
    {
        private readonly IPacketFrame _frame;
        private readonly IPacket _previousHeader;

        public IcmpPacketFactory(IPacket previousHeader, IPacketFrame frame)
        {
            _previousHeader = previousHeader;
            _frame = frame;
        }

        public IPacket CreatePacket(BinaryReader binaryReader)
        {
            //TODO Extract data from the rest of the header
            var type = (IcmpType)binaryReader.ReadByte();
            var code = binaryReader.ReadByte();
            var checksum = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            var rest = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());


            var packet = new IcmpPacket(type, code, checksum, rest, _previousHeader, PacketProtocol.NoProtocol);
            _frame.Packet = packet;

            return packet;
        }
    }
}
