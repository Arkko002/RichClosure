using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Transport_Layer;

namespace PacketSniffer.Factories.TransportFactories
{
    internal class UdpPacketByteFactory : IAbstractFactory
    {
        private readonly BinaryReader _binaryReader;
        private readonly Dictionary<string, object> _valueDictionary;

        public UdpPacketByteFactory(BinaryReader binaryReader, Dictionary<string, object> valueDictionary)
        {
            _binaryReader = binaryReader;
            _valueDictionary = valueDictionary;
        }

        public IPacket CreatePacket()
        {
            ReadPacketDataFromStream();
            IPacket packet = new UdpPacket(_valueDictionary);

            return packet;
        }

        private void ReadPacketDataFromStream()
        {
            _valueDictionary["PacketDisplayedProtocol"] = "UDP";

            UInt16 udpSourcePort = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadInt16());
            UInt16 udpDestinationPort = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadInt16());

            _valueDictionary["UdpPorts"] = new Dictionary<string, string>()
            {
                {"dst", udpDestinationPort.ToString()},
                {"src", udpSourcePort.ToString()}
            };

            _valueDictionary["UdpLength"] = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadInt16());
            _valueDictionary["UdpChecksum"] = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadInt16());
        }

        public AppProtocolEnum CheckForAppLayerPorts(IPacket packet)
        {

            UdpPacket udpPac = packet as UdpPacket;

            if (udpPac.UdpPorts.Any(x => x.Value.Equals(53)))
            {
                return AppProtocolEnum.Dns;
            }
            else if (udpPac.UdpPorts.Any(x => x.Value.Equals(67)) || (udpPac.UdpPorts.Any(x => x.Value.Equals(68))))
            {
                return AppProtocolEnum.Dhcp;
            }
            else
            {
                return AppProtocolEnum.NoAppProtocol;
            }
        }
    }
}
