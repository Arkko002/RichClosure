using richClosure.Packets.SessionLayer;
using richClosure.Packets.TransportLayer;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace richClosure.Packet_Sniffing.Factories.ApplicationFactories
{
    class TlsPacketFactory : IAbstractPacketFactory
    {
        public IPacket CreatePacket(IPacket packet, BinaryReader binaryReader)
        {
            byte tlsType = binaryReader.ReadByte();
            if (!Enum.IsDefined(typeof(TlsContentTypeEnum), (int)tlsType))
            {
                return packet;
            }

            UInt16 tlsVersion = (UInt16)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            string tlsVersionFinal;
            if (tlsVersion == 0x0303)
            {
                tlsVersionFinal = "TLS 1.2";
            }
            else if (tlsVersion == 0x0302)
            {
                tlsVersionFinal = "TLS 1.1";
            }
            else
            {
                tlsVersionFinal = "TLS 1.0";
            }

            UInt16 tlsDataLength = (UInt16)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            StringBuilder tlsData = new StringBuilder();
            tlsData.Append(BitConverter.ToString(binaryReader.ReadBytes(tlsDataLength)));

            TcpPacket pac = packet as TcpPacket;
            IPacket tlsPacket = new TlsPacket(pac)
            {
                TlsType = (TlsContentTypeEnum)tlsType,
                TlsVersion = tlsVersionFinal,
                TlsDataLength = tlsDataLength,
                TlsEncryptedData = tlsData.ToString(),
                PacketDisplayedProtocol = tlsVersionFinal
            };

            return tlsPacket;
        }
    }
}
