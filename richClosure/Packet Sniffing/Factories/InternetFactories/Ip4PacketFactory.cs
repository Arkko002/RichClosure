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

        public Ip4PacketFactory(BinaryReader binaryReader, byte[] buffer, ulong packetId)
        {
            _binaryReader = binaryReader;
            _buffer = buffer;
            _packetId = packetId;
        }

        public IPacket CreatePacket()
        {
            var valueDictionary = ReadPacketDataFromStream();

            IpPacket ip4Packet = new IpPacket();
            ip4Packet.SetIpProperties(valueDictionary);

            return ip4Packet;
        }

        private Dictionary<string, object> ReadPacketDataFromStream()
        {
            Dictionary<string, object> valueDictionary = new Dictionary<string, object>();

            valueDictionary["PacketId"] = _packetId;
            valueDictionary["DateTimeCaptured"] = DateTime.Now.ToString("yyyy-MM-dd / HH:mm:ss.fff",
                CultureInfo.InvariantCulture);

            byte ipVersionAndHeaderLength = _binaryReader.ReadByte();

            byte ipVersion = ipVersionAndHeaderLength;
            ipVersion >>= 4;

            valueDictionary["IpVersion"] = ipVersion;

            byte ipHeaderLength = ipVersionAndHeaderLength;
            ipHeaderLength <<= 4;
            ipHeaderLength >>= 4;
            ipHeaderLength *= 4;

            valueDictionary["Ip4HearderLength"] = ipHeaderLength;


            valueDictionary["IpDscp"] = _binaryReader.ReadByte();


            valueDictionary["IpTotalLength"] = (UInt16)IPAddress.NetworkToHostOrder(
                _binaryReader.ReadUInt16());
            valueDictionary["IpIdentification"] = (UInt16)IPAddress.NetworkToHostOrder(
                _binaryReader.ReadUInt16());

            UInt16 ipFlagsAndOffset = (UInt16)IPAddress.NetworkToHostOrder(
                _binaryReader.ReadUInt16());
            valueDictionary["IpTimeToLive"] = _binaryReader.ReadByte();
            valueDictionary["IpProtocol"] = _binaryReader.ReadByte();
            valueDictionary["IpChecksum"] = (UInt16)IPAddress.NetworkToHostOrder(
                _binaryReader.ReadUInt16());

            uint ipSourceIpAddress = _binaryReader.ReadUInt32();
            uint ipDestinationIpAddress = _binaryReader.ReadUInt32();

            valueDictionary["Ip4SourceAdr"] = new IPAddress(ipSourceIpAddress);
            valueDictionary["Ip4DestinationAdr"] = new IPAddress(ipDestinationIpAddress);
            
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

            valueDictionary["IpFlags"] = ipFlagsObj;

            int ipOffset = ipFlagsAndOffset << 3;
            ipOffset >>= 3;

            valueDictionary["IpOffset"] = ipOffset;

            valueDictionary["PacketData"] = BitConverter.ToString(_buffer, 0, (int)valueDictionary["IpTotalLength"]);

            return valueDictionary;
        }
    }
}
