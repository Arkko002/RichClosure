using System;
using System.IO;
using System.Net;
using System.Text;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Application.Tls;

namespace PacketSniffer.Factories.Application
{
    internal class TlsPacketFactory : IApplicationPacketFactory
    {
        private readonly IPacketFrame _frame;
        private readonly IPacket _previousHeader;

        public TlsPacketFactory(IPacket previousHeader, IPacketFrame frame)
        {
            _previousHeader = previousHeader;
            _frame = frame;
        }

        public IPacket CreatePacket(BinaryReader binaryReader)
        {
            //TODO
            var Type = (TlsContentType)binaryReader.ReadByte();

            UInt16 tlsVersion = (UInt16) IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Todo ???
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

            var Version = tlsVersionFinal;

            var DataLength = (UInt16) IPAddress.NetworkToHostOrder(binaryReader.ReadUInt16());

            var tlsData = new StringBuilder();
            tlsData.Append(BitConverter.ToString(binaryReader.ReadBytes(DataLength)));
            
            var packet = new TlsPacket(Type, Version, DataLength, tlsData.ToString(), _previousHeader, PacketProtocol.NoProtocol);
            _frame.Packet = packet;

            return packet;
        }
    }
}
