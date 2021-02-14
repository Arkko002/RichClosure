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
        private readonly IPacketFrame _frame;
        private readonly IPacket _previousHeader;

        public UdpPacketFactory(IPacket previousHeader, IPacketFrame frame)
        {
            _previousHeader = previousHeader;
            _frame = frame;
        }

        public IPacket CreatePacket(BinaryReader binaryReader)
        {
            var sourcePort = (UInt16)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadUInt16());
            var destinationPort = (UInt16)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadUInt16());
            var length = (UInt16)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadUInt16());
            var checksum = (UInt16)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadUInt16());
            
            //TODO Port heuristics for NextProtocol
            var packet = new UdpPacket(destinationPort, sourcePort, length, checksum, _previousHeader, PacketProtocol.NoProtocol);
            
            _frame.Packet = packet;
            _frame.DestinationPort = destinationPort;
            _frame.SourcePort = sourcePort;

            return packet;
        }
    }
}
