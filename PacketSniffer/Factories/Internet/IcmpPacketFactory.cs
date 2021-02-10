using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Internet;

namespace PacketSniffer.Factories.Internet
{
    internal class IcmpPacketFactory : IInternetPacketFactory
    {
        private readonly BinaryReader _binaryReader;
        private IPacket _previousHeader;
        
        public IcmpPacketFactory(BinaryReader binaryReader, IPacket previousHeader)
        {
            _binaryReader = binaryReader;
            _previousHeader = previousHeader;
        }

        public IPacket CreatePacket()
        {
            var type = _binaryReader.ReadByte();
            var code = _binaryReader.ReadByte();
            var checksum = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());
            var rest = (UInt32)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt32());

            return new IcmpPacket(type, code, checksum, rest, _previousHeader, PacketProtocol.NoProtocol);
        }
    }
}
