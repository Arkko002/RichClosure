using richClosure.Packets.InternetLayer;
using System;
using System.IO;
using System.Net;

namespace richClosure.Packet_Sniffing.Factories
{
    class Ip6PacketFactory : IAbstractFactory
    {
        public IPacket CreatePacket(byte[] buffer, BinaryReader binaryReader)
        {
            UInt32 dataBatch = (UInt32)IPAddress.NetworkToHostOrder(
                                           binaryReader.ReadInt32());

            string dataBatchBin = Convert.ToString(dataBatch, 2);

            byte ip6Version = Convert.ToByte(dataBatchBin.Substring(0, 4), 10);
            byte ip6TrafficClass = Convert.ToByte(dataBatchBin.Substring(4, 8), 10);
            UInt32 ip6FlowLabel = Convert.ToUInt32(dataBatchBin.Substring(12, 20), 10);

            UInt16 ip6PayloadLength = (UInt16)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadInt16());
            byte ip6NextHeader = binaryReader.ReadByte();
            byte ip6HopLimit = binaryReader.ReadByte();

            IPAddress IP6SourceAdr = new IPAddress(binaryReader.ReadBytes(16));
            IPAddress IP6DestinationAdr = new IPAddress(binaryReader.ReadBytes(16));

            IpPacket ip6Packet = new IpPacket()
            {
                IpVersion = ip6Version,
                Ip6TrafficClass = ip6TrafficClass,
                Ip6FlowLabel = ip6FlowLabel,
                IpTotalLength = ip6PayloadLength,
                IpProtocol = (IpProtocolEnum)ip6NextHeader,
                Ip6HopLimit = ip6HopLimit,               
            };

            ip6Packet.Ip6Adrs["src"] = IP6SourceAdr.ToString();
            ip6Packet.Ip6Adrs["dst"] = IP6DestinationAdr.ToString();

            return ip6Packet;
        }
    }
}
