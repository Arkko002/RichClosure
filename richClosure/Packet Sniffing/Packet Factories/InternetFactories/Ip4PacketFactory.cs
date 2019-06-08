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
            _valueDictionary["AppProtocol"] = AppProtocolEnum.NoAppProtocol;
            _valueDictionary["PacketDisplayedProtocol"] = "IPv4";
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

            _valueDictionary["Ip4HeaderLength"] = ipHeaderLength;


            _valueDictionary["Ip4Dscp"] = _binaryReader.ReadByte();


            _valueDictionary["IpTotalLength"] = (UInt16)IPAddress.NetworkToHostOrder(
                _binaryReader.ReadInt16());
            _valueDictionary["Ip4Identification"] = (UInt16)IPAddress.NetworkToHostOrder(
                _binaryReader.ReadInt16());

            UInt16 ipFlagsAndOffset = (UInt16)IPAddress.NetworkToHostOrder(
                _binaryReader.ReadInt16());
            _valueDictionary["Ip4TimeToLive"] = _binaryReader.ReadByte();
            _valueDictionary["IpProtocol"] = _binaryReader.ReadByte();
            _valueDictionary["Ip4HeaderChecksum"] = (UInt16)IPAddress.NetworkToHostOrder(
                _binaryReader.ReadInt16());

            uint ipSourceIpAddress = (uint)_binaryReader.ReadInt32();
            uint ipDestinationIpAddress = (uint)_binaryReader.ReadInt32();

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

            _valueDictionary["Ip4Flags"] = ipFlagsObj;

            int ipOffset = ipFlagsAndOffset << 3;
            ipOffset >>= 3;

            _valueDictionary["Ip4Offset"] = ipOffset;

           
            _valueDictionary["PacketData"] = BitConverter.ToString(_buffer, 0, Convert.ToInt32(_valueDictionary["IpTotalLength"]));
        }
    }
}
