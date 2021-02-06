using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Session_Layer;

namespace PacketSniffer.Factories.ApplicationFactories
{
    internal class TlsPacketByteFactory : IAbstractFactory
    {
        private readonly BinaryReader _binaryReader;
        private readonly Dictionary<string, object> _valueDictionary;

        public TlsPacketByteFactory(BinaryReader binaryReader, Dictionary<string, object> valueDictionary)
        {
            _binaryReader = binaryReader;
            _valueDictionary = valueDictionary;
        }
        public IPacket CreatePacket()
        {
            ReadPacketDataFromStream();
            IPacket tlsPacket = new TlsPacket(_valueDictionary);

            return tlsPacket;
        }

        private void ReadPacketDataFromStream()
        {
            _valueDictionary["AppProtocol"] = AppProtocolEnum.Tls;
            _valueDictionary["PacketDisplayedProtocol"] = "TLS";

            _valueDictionary["TlsType"] = _binaryReader.ReadByte();
            if (!Enum.IsDefined(typeof(TlsContentTypeEnum), (int)_valueDictionary["TlsType"]))
            {
                return;
            }

            UInt16 tlsVersion = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());

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

            _valueDictionary["TlsVersion"] = tlsVersionFinal;

            _valueDictionary["TlsDataLength"] = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());

            StringBuilder tlsData = new StringBuilder();
            tlsData.Append(BitConverter.ToString(_binaryReader.ReadBytes((int)_valueDictionary["TlsDataLength"])));

            _valueDictionary["TlsData"] = tlsData;
        }
    }
}
