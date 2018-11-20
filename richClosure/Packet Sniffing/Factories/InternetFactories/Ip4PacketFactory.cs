using richClosure.Packets.InternetLayer;
using System;
using System.IO;
using System.Net;

namespace richClosure.Packet_Sniffing.Factories.InternetFactories
{
    class Ip4PacketFactory : IAbstractBufferFactory
    {
        public IPacket CreatePacket(byte[] buffer, BinaryReader binaryReader)
        {           

            byte ipDscp = binaryReader.ReadByte();

            UInt16 ipTotalLength = (UInt16)IPAddress.NetworkToHostOrder(
                                                    binaryReader.ReadInt16());
            UInt16 ipIdentification = (UInt16)IPAddress.NetworkToHostOrder(
                                                    binaryReader.ReadInt16());
            UInt16 ipFlagsAndOffset = (UInt16)IPAddress.NetworkToHostOrder(
                                                    binaryReader.ReadInt16());
            byte ipTimeToLive = binaryReader.ReadByte();
            byte ipProtocol = binaryReader.ReadByte();
            UInt16 ipChecksum = (UInt16)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadInt16());

            uint ipSourceIpAddress = (uint)binaryReader.ReadInt32();
            uint ipDestinationIpAddress = (uint)binaryReader.ReadInt32();

            IPAddress ipSourceAdr = new IPAddress(ipSourceIpAddress);
            IPAddress ipDestinationAdr = new IPAddress(ipDestinationIpAddress);


            int ipFlags = ipFlagsAndOffset >> 13;

            int ipOffset = ipFlagsAndOffset << 3;
            ipOffset >>= 3;

            string packetData = BitConverter.ToString(buffer, 0, ipTotalLength);
            //packetData = packetData.Replace("-", "");

            IpPacket ip4Packet = new IpPacket()
            {
                PacketData = packetData,
                EthDestinationMacAdr = "UNINCLUDED",
                EthSourceMacAdr = "UNINCLUDED",
                EthProtocol = 0,
                Ip4Flags = (IpFlagsEnum)ipFlags,
                Ip4TimeToLive = ipTimeToLive,
                Ip4Adrs = new System.Collections.Generic.Dictionary<string, string>(),
                IpProtocol = (IpProtocolEnum)ipProtocol,
                Ip4Dscp = ipDscp,
                IpTotalLength = ipTotalLength,
                Ip4Identification = ipIdentification,
                Ip4Offset = (ushort)ipOffset,
                Ip4HeaderChecksum = ipChecksum,
            };

            ip4Packet.Ip4Adrs["src"] = ipSourceAdr.ToString();
            ip4Packet.Ip4Adrs["dst"] = ipDestinationAdr.ToString();

            return ip4Packet;
        }
    }
}
