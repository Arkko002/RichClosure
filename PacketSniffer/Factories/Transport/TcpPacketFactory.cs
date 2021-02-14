using System;
using System.IO;
using System.Net;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Transport;

namespace PacketSniffer.Factories.Transport
{
    internal class TcpPacketFactory : ITransportPacketFactory 
    {
        private readonly BinaryReader _binaryReader;
        private readonly IPacketFrame _frame;
        private readonly IPacket _previousHeader;

        public TcpPacketFactory(BinaryReader binaryReader, IPacketFrame frame, IPacket previousHeader)
        {
            _binaryReader = binaryReader;
            _frame = frame;
            _previousHeader = previousHeader;
        }


        public IPacket CreatePacket()
        {
            var sourcePort = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadUInt16());
            var destinationPort = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadUInt16());

            var sequenceNumber = (UInt32)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadUInt32());
            var ackNumber = (UInt32)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadUInt32());

            byte dataOffsetAndNs = _binaryReader.ReadByte();

            var dataOffset = dataOffsetAndNs <<= 4;

            byte tcpFlags = _binaryReader.ReadByte();
            bool ns = (dataOffsetAndNs & 1) == 0;
            bool fin = (tcpFlags & 1) == 0;
            bool syn = (tcpFlags & 2) == 0;
            bool rst = (tcpFlags & 4) == 0;
            bool psh = (tcpFlags & 8) == 0;
            bool ack = (tcpFlags & 16) == 0;
            bool urg = (tcpFlags & 32) == 0;
            bool ece = (tcpFlags & 64) == 0;
            bool cwr = (tcpFlags & 128) == 0;


            var windowSize = (UInt16)IPAddress.NetworkToHostOrder(
                                _binaryReader.ReadUInt16());
            var checksum = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadUInt16());
            var urgentPointer = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadUInt16());

            // TODO
            if (dataOffset > 5)
            {

            }

            //TODO Port heuristics for NextProtocol
            var packet = new TcpPacket(destinationPort, sourcePort, sequenceNumber, ackNumber, dataOffset,
                urgentPointer, windowSize, checksum, _previousHeader, fin, syn, rst, psh, ack, urg, ece, cwr, ns, PacketProtocol.NoProtocol);

            _frame.Packet = packet;
            _frame.DestinationPort = destinationPort;
            _frame.SourcePort = sourcePort;

            return packet;
        }
    }
}
