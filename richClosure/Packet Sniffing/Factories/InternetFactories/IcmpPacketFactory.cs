using richClosure.Packets.InternetLayer;
using System;
using System.IO;
using System.Net;

namespace richClosure.Packet_Sniffing.Factories
{
    class IcmpPacketFactory : IAbstractPacketFactory
    {
        public IPacket CreatePacket(IPacket packet, BinaryReader binaryReader)
        {
            byte icmpType = binaryReader.ReadByte();
            byte icmpCode = binaryReader.ReadByte();

            UInt16 icmpChecksum = (UInt16)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadInt16());
            UInt32 icmpRest = (UInt32)IPAddress.NetworkToHostOrder(
                                            binaryReader.ReadInt32());

            IpPacket pac = packet as IpPacket;

            IPacket icmpPacket = new IcmpPacket(pac)
            {
                IcmpType = icmpType,
                IcmpCode = icmpCode,
                IcmpChecksum = icmpChecksum,
                IcmpRest = icmpRest.ToString(),
                PacketDisplayedProtocol = "ICMP"
            };

            return icmpPacket;
            
        }
    }
}
