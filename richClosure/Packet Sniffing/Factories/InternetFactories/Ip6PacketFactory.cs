using System.Collections.Generic;
using richClosure.Packets.InternetLayer;
using System;
using System.IO;
using System.Net;

namespace richClosure.Packet_Sniffing.Factories
{
    class Ip6PacketFactory : IAbstractFactory
    {

        private BinaryReader _binaryReader;
        private byte[] _buffer;
        private ulong _packetId;

        public Ip6PacketFactory(BinaryReader binaryReader, byte[] buffer, ulong packetId)
        {
            _binaryReader = binaryReader;
            _buffer = buffer;
            _packetId = packetId;
        }
        public IPacket CreatePacket()
        {
            var valueDictionary = ReadPacketDataFromStream();
            IpPacket ip6Packet = new IpPacket();
            ip6Packet.SetIpProperties(valueDictionary);

            return ip6Packet;
        }

        private Dictionary<string, object> ReadPacketDataFromStream()
        {
            var valueDictionary = new Dictionary<string, object>();

            UInt32 dataBatch = (UInt32)IPAddress.NetworkToHostOrder(_binaryReader.ReadUInt32());

            string dataBatchBin = Convert.ToString(dataBatch, 2);

            valueDictionary["Ip6Version"] = Convert.ToByte(dataBatchBin.Substring(0, 4), 10);
            valueDictionary["Ip6TrafficClass"]  = Convert.ToByte(dataBatchBin.Substring(4, 8), 10);
            valueDictionary["Ip6FlowLabel"]  = Convert.ToUInt32(dataBatchBin.Substring(12, 20), 10);

            valueDictionary["Ip6PayloadLength"]  = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadInt16());
            valueDictionary["Ip6NextHeader"]  = _binaryReader.ReadByte();
            valueDictionary["Ip6HopLimit"]  = _binaryReader.ReadByte();

            valueDictionary["Ip6SourceAdr"]  = new IPAddress(_binaryReader.ReadBytes(16));
            valueDictionary["Ip6DestinationAdr"]  = new IPAddress(_binaryReader.ReadBytes(16));           

            return valueDictionary;
        }
    }
}
