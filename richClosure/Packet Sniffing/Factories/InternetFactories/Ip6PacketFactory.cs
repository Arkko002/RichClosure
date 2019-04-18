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
        private Dictionary<string, object> _valueDictionary;

        public Ip6PacketFactory(BinaryReader binaryReader, byte[] buffer, ulong packetId,
         Dictionary<string, object> valueDictionary)
        {
            _binaryReader = binaryReader;
            _buffer = buffer;
            _packetId = packetId;
            _valueDictionary = valueDictionary;
        }
        public IPacket CreatePacket()
        {
            ReadPacketDataFromStream();
            IpPacket ip6Packet = new IpPacket(_valueDictionary);

            return ip6Packet;
        }

        private void ReadPacketDataFromStream()
        {
            UInt32 dataBatch = (UInt32)IPAddress.NetworkToHostOrder(_binaryReader.ReadUInt32());

            string dataBatchBin = Convert.ToString(dataBatch, 2);

            _valueDictionary["Ip6Version"] = Convert.ToByte(dataBatchBin.Substring(0, 4), 10);
            _valueDictionary["Ip6TrafficClass"] = Convert.ToByte(dataBatchBin.Substring(4, 8), 10);
            _valueDictionary["Ip6FlowLabel"] = Convert.ToUInt32(dataBatchBin.Substring(12, 20), 10);

            _valueDictionary["Ip6PayloadLength"] = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadInt16());
            _valueDictionary["Ip6NextHeader"] = _binaryReader.ReadByte();
            _valueDictionary["Ip6HopLimit"] = _binaryReader.ReadByte();

            _valueDictionary["Ip6SourceAdr"] = new IPAddress(_binaryReader.ReadBytes(16));
            _valueDictionary["Ip6DestinationAdr"] = new IPAddress(_binaryReader.ReadBytes(16));
        }
    }
}
