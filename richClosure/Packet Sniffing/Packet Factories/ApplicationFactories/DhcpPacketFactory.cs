using richClosure.Packets.ApplicationLayer;
using richClosure.Packets.TransportLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace richClosure.Packet_Sniffing.Factories.ApplicationFactories
{
    class DhcpPacketFactory : IAbstractFactory
    {
        private BinaryReader _binaryReader;
        private Dictionary<string, object> _valueDictionary;

        public DhcpPacketFactory(BinaryReader binaryReader, Dictionary<string, object> valueDictionary)
        {
            _binaryReader = binaryReader;
            _valueDictionary = valueDictionary;
        }

        public IPacket CreatePacket()
        {
            ReadPacketDataFromStream();
            IPacket dhcpPacket = new DhcpPacket(_valueDictionary);

            return dhcpPacket;
        }

        private void ReadPacketDataFromStream()
        {
            _valueDictionary["AppProtocol"] = AppProtocolEnum.DHCP;
            _valueDictionary["PacketDisplayedProtocol"] = "DHCP";

            _valueDictionary["DhcpOpcode"] = _binaryReader.ReadByte();
            _valueDictionary["DhcpHardType"] = _binaryReader.ReadByte();
            _valueDictionary["dhcpHardAdrLength"] = _binaryReader.ReadByte();
            _valueDictionary["dhcpHops"] = _binaryReader.ReadByte();

            _valueDictionary["dhcpTransId"] = (UInt32)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt32());
            _valueDictionary["dhcpSeconds"] = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());
            UInt16 dhcpFlags = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());

            string flagsFinal;
            if (dhcpFlags > 0)
            {
                flagsFinal = "Broadcast";
            }
            else
            {
                flagsFinal = "No Flags";
            }

            _valueDictionary["DhcpFlags"] = flagsFinal;

            UInt32 dhcpClientIp = (UInt32)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt32());
            UInt32 dhcpYourIp = (UInt32)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt32());
            UInt32 dhcpServerIp = (UInt32)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt32());
            UInt32 dhcpGatewayIp = (UInt32)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt32());

            _valueDictionary["DhcpClientIp"] = new IPAddress(dhcpClientIp).ToString();
            _valueDictionary["DhcpYourIp"] = new IPAddress(dhcpYourIp).ToString();
            _valueDictionary["DhcpServerIp"] = new IPAddress(dhcpServerIp).ToString();
            _valueDictionary["DhcpGatewayIp"] = new IPAddress(dhcpGatewayIp).ToString(); ;

            byte[] dhcpClientHardAdr = _binaryReader.ReadBytes((int)_valueDictionary["dhcpHardAdrLength"]);
            _valueDictionary["DhcpClientHardAdr"] = BitConverter.ToString(dhcpClientHardAdr);

            byte[] dhcpServerName = _binaryReader.ReadBytes(64);
            _valueDictionary["DhcpServerName"] = ConvertNameToString(dhcpServerName);

            byte[] padding = _binaryReader.ReadBytes(16 - (int)_valueDictionary["dhcpHardAdrLength"]);

            byte[] dhcpBootFilename = _binaryReader.ReadBytes(128);
            _valueDictionary["DhcpBootFilename"] = ConvertNameToString(dhcpBootFilename);

            //TODO
            string dhcpOptions;
        }

        private string ConvertNameToString(byte[] dhcpServerName)
        {
            StringBuilder serverNameStrBld = new StringBuilder();
            foreach (byte b in dhcpServerName)
            {
                if (b >= 33 && b <= 126)
                {
                    char ch = Convert.ToChar(b);
                    serverNameStrBld.Append(ch);
                }
                else
                {
                    serverNameStrBld.Append(".");
                }
            }

            return serverNameStrBld.ToString();
        }
    }
}
