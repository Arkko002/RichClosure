using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Internet;
using PacketSniffer.Packets.Internet.Ip;

namespace PacketSniffer.Factories.Internet.Ip
{
    internal class Ip6PacketFactory : IIpPacketFactory
    {
        private readonly IPacketFrame _frame;
        private readonly IPacket _previousHeader;

        public Ip6PacketFactory(IPacket previousHeader, IPacketFrame frame)
        {
            _previousHeader = previousHeader;
            _frame = frame;
        }

        public IPacket CreatePacket(BinaryReader binaryReader)
        {
            UInt32 dataBatch = (UInt32)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());

            string dataBatchBin = Convert.ToString(dataBatch, 2);

            var version = Convert.ToByte(dataBatchBin.Substring(0, 4), 10);
            var trafficClass = Convert.ToByte(dataBatchBin.Substring(4, 8), 10);
            var flowLabel = Convert.ToUInt32(dataBatchBin.Substring(12, 20), 10);

            var payloadLength = (UInt16)IPAddress.NetworkToHostOrder(
                          binaryReader.ReadInt16());
            var nextHeader = (IpProtocol)binaryReader.ReadByte();
            var hopLimit = binaryReader.ReadByte();

            var sourceAdr = new IPAddress(binaryReader.ReadBytes(16));
            var destinationAdr = new IPAddress(binaryReader.ReadBytes(16));

            var nextProtocol = (PacketProtocol)Enum.Parse(typeof(PacketProtocol), Enum.GetName(nextHeader));
            
            var packet = new Ip6Packet(trafficClass, flowLabel, hopLimit, version, _previousHeader, sourceAdr, destinationAdr,
                nextHeader, payloadLength, nextProtocol);
            _frame.Packet = packet;
            _frame.DestinationAddress = destinationAdr.ToString();
            _frame.SourceAddress = sourceAdr.ToString();

            return packet;
        }
    }
}
