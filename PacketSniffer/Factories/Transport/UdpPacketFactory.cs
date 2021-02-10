using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Transport;

namespace PacketSniffer.Factories.Transport
{
    internal class UdpPacketFactory : ITransportPacketFactory
    {
        private readonly BinaryReader _binaryReader;
        private IPacket _previousHeader;

        public UdpPacketFactory(BinaryReader binaryReader, IPacket previousHeader)
        {
            _binaryReader = binaryReader;
            _previousHeader = previousHeader;
        }

        public IPacket CreatePacket()
        {
            var sourcePort = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadUInt16());
            var destinationPort = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadUInt16());
            var length = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadUInt16());
            var checksum = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadUInt16());
            
            //TODO Port heuristics for NextProtocol
            return new UdpPacket(destinationPort, sourcePort, length, checksum, _previousHeader, PacketProtocol.NoProtocol);
        }
    }
}
