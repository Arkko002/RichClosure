using System;
using System.IO;
using System.Net;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Internet.Ip;

namespace PacketSniffer.Factories.Internet.Ip
{
    internal class Ip4PacketFactory : IIpPacketFactory
    {
        private IPacketFrame _frame;
        private IPacket _previousHeader;
        
        public Ip4PacketFactory(IPacket previousHeader, IPacketFrame frame)
        {
            _previousHeader = previousHeader;
            _frame = frame;
        }
        

        public IPacket CreatePacket(BinaryReader binaryReader)
        {
            var ipVersionAndHeaderLength = binaryReader.ReadByte();
            var ipVersion = ipVersionAndHeaderLength >> 4;

            var headerLength = ipVersionAndHeaderLength << 4;
            headerLength >>= 4;
            headerLength *= 4;

            var dscp = binaryReader.ReadByte();

            var totalLength = (ushort)IPAddress.NetworkToHostOrder(
                binaryReader.ReadUInt16());
            var identification = (ushort)IPAddress.NetworkToHostOrder(
                binaryReader.ReadUInt16());

            var timeToLive = binaryReader.ReadByte();
            var protocol = (IpProtocol) binaryReader.ReadByte();
            var headerChecksum = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadUInt16());

            IPAddress sourceIpAddress = IPAddress.Parse(binaryReader.ReadUInt32().ToString()); 
            IPAddress destinationIpAddress = IPAddress.Parse(binaryReader.ReadUInt32().ToString());

            var ipFlags = GetIpFlags(binaryReader);
            var offset = GetOffset(binaryReader);

            var nextProtocol = (PacketProtocol)Enum.Parse(typeof(PacketProtocol), Enum.GetName(protocol));
            
            var packet = new Ip4Packet((byte) headerLength, dscp, identification, offset, ipFlags, timeToLive, headerChecksum,
                (byte) ipVersion, _previousHeader, sourceIpAddress, destinationIpAddress, protocol, totalLength, nextProtocol);
            _frame.Packet = packet;
            _frame.DestinationAddress = destinationIpAddress.ToString();
            _frame.SourceAddress = sourceIpAddress.ToString();

            return packet;
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
