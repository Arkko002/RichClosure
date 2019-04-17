using richClosure.Packets.InternetLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;

namespace richClosure.Packet_Sniffing.Factories.InternetFactories
{
    class Ip4PacketFactory : IAbstractFactory
    {
        private BinaryReader _binaryReader;
        private byte[] _buffer;
        private ulong _packetId;
        private Dictionary<string, object> _valueDictionary;

        public Ip4PacketFactory(BinaryReader binaryReader, byte[] buffer, ulong packetId, Dictionary<string, object> valueDictionary)
        {
            _binaryReader = binaryReader;
            _buffer = buffer;
            _packetId = packetId;
            _valueDictionary = valueDictionary;
        }

        public IPacket CreatePacket()
        {
            ReadPacketDataFromStream();

            IpPacket ip4Packet = new IpPacket(_valueDictionary);
            return ip4Packet;
        }

        private void ReadPacketDataFromStream()
        {
            _valueDictionary["PacketId"] = _packetId;
            _valueDictionary["DateTimeCaptured"] = DateTime.Now.ToString("yyyy-MM-dd / HH:mm:ss.fff",
                CultureInfo.InvariantCulture);

            byte ipVersionAndHeaderLength = _binaryReader.ReadByte();

            byte ipVersion = ipVersionAndHeaderLength;
            ipVersion >>= 4;

            _valueDictionary["IpVersion"] = ipVersion;

            byte ipHeaderLength = ipVersionAndHeaderLength;
            ipHeaderLength <<= 4;
            ipHeaderLength >>= 4;
            ipHeaderLength *= 4;

            _valueDictionary["Ip4HearderLength"] = ipHeaderLength;


            _valueDictionary["IpDscp"] = _binaryReader.ReadByte();


            _valueDictionary["IpTotalLength"] = (UInt16)IPAddress.NetworkToHostOrder(
                _binaryReader.ReadUInt16());
            _valueDictionary["IpIdentification"] = (UInt16)IPAddress.NetworkToHostOrder(
                _binaryReader.ReadUInt16());

            UInt16 ipFlagsAndOffset = (UInt16)IPAddress.NetworkToHostOrder(
                _binaryReader.ReadUInt16());
            _valueDictionary["IpTimeToLive"] = _binaryReader.ReadByte();
            _valueDictionary["IpProtocol"] = _binaryReader.ReadByte();
            _valueDictionary["IpChecksum"] = (UInt16)IPAddress.NetworkToHostOrder(
                _binaryReader.ReadUInt16());

            uint ipSourceIpAddress = _binaryReader.ReadUInt32();
            uint ipDestinationIpAddress = _binaryReader.ReadUInt32();

            _valueDictionary["Ip4Adrs"] = new Dictionary<string, string>()
            {
                {"src", new IPAddress(ipSourceIpAddress).ToString()},
                {"dst", new IPAddress(ipSourceIpAddress).ToString()}
            };

            int ipFlags = ipFlagsAndOffset >> 13;
            IpFlags ipFlagsObj = new IpFlags();

            if ((ipFlags & 1) != 0)
            {
                ipFlagsObj.Res.IsSet = true;
            }
            if ((ipFlags & 2) != 0)
            {
                ipFlagsObj.DF.IsSet = true;
            }
            if ((ipFlags & 4) != 0)
            {
                ipFlagsObj.MF.IsSet = true;
            }

            _valueDictionary["IpFlags"] = ipFlagsObj;

            int ipOffset = ipFlagsAndOffset << 3;
            ipOffset >>= 3;

            _valueDictionary["IpOffset"] = ipOffset;

            _valueDictionary["PacketData"] = BitConverter.ToString(_buffer, 0, (int)_valueDictionary["IpTotalLength"]);
        }
    }
}
