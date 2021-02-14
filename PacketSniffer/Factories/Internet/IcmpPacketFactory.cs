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
        private readonly BinaryReader _binaryReader;
        private readonly IPacketFrame _frame;
        private readonly IPacket _previousHeader;

        public IcmpPacketFactory(BinaryReader binaryReader, IPacket previousHeader, IPacketFrame frame)
        {
            _binaryReader = binaryReader;
            _previousHeader = previousHeader;
            _frame = frame;
        }

        public IPacket CreatePacket()
        {
            //TODO Extract data from the rest of the header
            var type = (IcmpType)_binaryReader.ReadByte();
            var code = _binaryReader.ReadByte();
            var checksum = (ushort)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());
            var rest = (uint)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt32());


            var packet = new IcmpPacket(type, code, checksum, rest, _previousHeader, PacketProtocol.NoProtocol);
            _frame.Packet = packet;

            return packet;
        }
    }
}
