using System;
using System.IO;
using System.Net;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Internet;
using PacketSniffer.Packets.Internet.Ip;

namespace PacketSniffer.Factories.Internet.Ip
{
    internal class Ip4PacketFactory : IIpPacketFactory
    {
        private BinaryReader _binaryReader;
        private IPacket? _previousHeader;
        
        public Ip4PacketFactory(BinaryReader binaryReader, IPacket? previousHeader)
        {
            _previousHeader = previousHeader;
            _binaryReader = binaryReader;
        }
        

        public IPacket CreatePacket()
        {
            var ipVersionAndHeaderLength = _binaryReader.ReadByte();
            var ipVersion = ipVersionAndHeaderLength >> 4;

            var headerLength = ipVersionAndHeaderLength << 4;
            headerLength >>= 4;
            headerLength *= 4;

            var dscp = _binaryReader.ReadByte();

            var totalLength = (ushort)IPAddress.NetworkToHostOrder(
                _binaryReader.ReadUInt16());
            var identification = (ushort)IPAddress.NetworkToHostOrder(
                _binaryReader.ReadUInt16());

            var timeToLive = _binaryReader.ReadByte();
            var protocol = (IpProtocol) _binaryReader.ReadByte();
            var headerChecksum = (ushort)IPAddress.NetworkToHostOrder(_binaryReader.ReadUInt16());

            IPAddress sourceIpAddress = IPAddress.Parse(_binaryReader.ReadUInt32().ToString()); 
            IPAddress destinationIpAddress = IPAddress.Parse(_binaryReader.ReadUInt32().ToString());

            var ipFlags = GetIpFlags(_binaryReader);
            var offset = GetOffset(_binaryReader);

            var nextProtocol = (PacketProtocol)Enum.Parse(typeof(PacketProtocol), Enum.GetName(protocol));
            
            return new Ip4Packet((byte) headerLength, dscp, identification, offset, ipFlags, timeToLive, headerChecksum,
                (byte) ipVersion, _previousHeader, sourceIpAddress, destinationIpAddress, protocol, totalLength, nextProtocol);
        }

        private byte GetIpFlags(BinaryReader binaryReader)
        {
            
            UInt16 ipFlagsAndOffset = (UInt16)IPAddress.NetworkToHostOrder(
                binaryReader.ReadInt16());
            int ipFlags = ipFlagsAndOffset >> 13;

            return (byte)ipFlags;
        }

        private ushort GetOffset(BinaryReader binaryReader)
        {
            
            UInt16 ipFlagsAndOffset = (UInt16)IPAddress.NetworkToHostOrder(
                binaryReader.ReadInt16());
            int ipOffset = ipFlagsAndOffset << 3;
            ipOffset >>= 3;

            return (ushort)ipOffset;
        }
    }
}
