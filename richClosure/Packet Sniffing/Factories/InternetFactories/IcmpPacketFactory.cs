using System.Collections.Generic;
using richClosure.Packets.InternetLayer;
using System;
using System.IO;
using System.Net;

namespace richClosure.Packet_Sniffing.Factories
{
    class IcmpPacketFactory : IAbstractFactory
    {
        private BinaryReader _binaryReader;
        private Dictionary<string, object> _valueDictionary;

        public Ip4PacketFactory(BinaryReader binaryReader, Dictionary<string, object> valueDictionary)
        {
            _binaryReader = binaryReader;
            _valueDictionary = valueDictionary;
        }

        public IPacket CreatePacket()
        {
            ReadPacketDataFromStream();
            IPacket icmpPacket = new IcmpPacket(_valueDictionary)
            return icmpPacket;
        }

        private Dictionary<string, object> ReadPacketDataFromStream()
        {
            _valueDictionary["IcmpType"] = _binaryReader.ReadByte();
            _valueDictionary["IcmpCode"] = _binaryReader.ReadByte();

            _valueDictionary["IcmpChecksum"] = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadInt16());
            _valueDictionary["icmpRest"] = (UInt32)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadInt32());
        }
    }
}
