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
        private IPacket _basePacket;

        public Ip4PacketFactory(BinaryReader binaryReader, IPacket basePacket)
        {
            _binaryReader = binaryReader;
            _basePacket = basePacket;
        }

        public IPacket CreatePacket()
        {
            IPacket icmpPacket = new IcmpPacket(_basePacket)
            {
                IcmpType = icmpType,
                IcmpCode = icmpCode,
                IcmpChecksum = icmpChecksum,
                IcmpRest = icmpRest.ToString(),
                PacketDisplayedProtocol = "ICMP"
            };

            return icmpPacket;           
        }

        private Dictionary<string, object> ReadPacketDataFromStream()
        {
            var valueDictionary = new Dictionary<string, object>();

            valueDictionary["IcmpType"] = _binaryReader.ReadByte();
            valueDictionary["IcmpCode"] = _binaryReader.ReadByte();

            valueDictionary["IcmpChecksum"] = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadInt16());
            valueDictionary["icmpRest"] = (UInt32)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadInt32());

            return valueDictionary;
        }
    }
}
