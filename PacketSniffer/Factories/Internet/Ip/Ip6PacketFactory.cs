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
        private readonly BinaryReader _binaryReader;
        private readonly IPacket _previousHeader;

        public Ip6PacketFactory(BinaryReader binaryReader, IPacket previousHeader)
        {
            _binaryReader = binaryReader;
            _previousHeader = previousHeader;
        }

        public IPacket CreatePacket()
        {
            UInt32 dataBatch = (UInt32)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt32());

            string dataBatchBin = Convert.ToString(dataBatch, 2);

            var version = Convert.ToByte(dataBatchBin.Substring(0, 4), 10);
            var trafficClass = Convert.ToByte(dataBatchBin.Substring(4, 8), 10);
            var flowLabel = Convert.ToUInt32(dataBatchBin.Substring(12, 20), 10);

            var payloadLength = (UInt16)IPAddress.NetworkToHostOrder(
                          _binaryReader.ReadInt16());
            var nextHeader = (IpProtocol)_binaryReader.ReadByte();
            var hopLimit = _binaryReader.ReadByte();

            var sourceAdr = new IPAddress(_binaryReader.ReadBytes(16));
            var destinationAdr = new IPAddress(_binaryReader.ReadBytes(16));

            var nextProtocol = (PacketProtocol)Enum.Parse(typeof(PacketProtocol), Enum.GetName(nextHeader));
            
            return new Ip6Packet(trafficClass, flowLabel, hopLimit, version, _previousHeader, sourceAdr, destinationAdr,
                nextHeader, payloadLength, nextProtocol);
        }
    }
}
