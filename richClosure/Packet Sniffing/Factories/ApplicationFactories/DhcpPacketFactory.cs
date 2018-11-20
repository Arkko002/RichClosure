using richClosure.Packets.ApplicationLayer;
using richClosure.Packets.TransportLayer;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace richClosure.Packet_Sniffing.Factories.ApplicationFactories
{
    class DhcpPacketFactory : IAbstractPacketFactory
    {
        public IPacket CreatePacket(IPacket packet, BinaryReader binaryReader)
        {
            byte dhcpOpcode = binaryReader.ReadByte();
            byte dhcpHardType = binaryReader.ReadByte();
            byte dhcpHardAdrLength = binaryReader.ReadByte();
            byte dhcpHops = binaryReader.ReadByte();

            UInt32 dhcpTransId = (UInt32)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
            UInt16 dhcpSeconds = (UInt16)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            UInt16 dhcpFlags = (UInt16)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            string flagsFinal;
            if (dhcpFlags > 0)
            {
                flagsFinal = "Broadcast";
            }
            else
            {
                flagsFinal = "No Flags";
            }

            UInt32 dhcpClientIp = (UInt32)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
            UInt32 dhcpYourIp = (UInt32)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
            UInt32 dhcpServerIp = (UInt32)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
            UInt32 dhcpGatewayIp = (UInt32)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());

            IPAddress clientIp = new IPAddress(dhcpClientIp);
            IPAddress yourIp = new IPAddress(dhcpYourIp);
            IPAddress serverIp = new IPAddress(dhcpServerIp);
            IPAddress gatewayIp = new IPAddress(dhcpGatewayIp);

            byte[] dhcpClientHardAdr = binaryReader.ReadBytes(dhcpHardAdrLength);
            string clientHardAdr = BitConverter.ToString(dhcpClientHardAdr);

            byte[] dhcpServerName = binaryReader.ReadBytes(64);

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

            byte[] padding = binaryReader.ReadBytes(16 - dhcpHardAdrLength);

            byte[] dhcpBootFilename = binaryReader.ReadBytes(128);
            string bootFilename = Encoding.Default.GetString(dhcpBootFilename);

            StringBuilder bootFilenameStrBld = new StringBuilder();
            foreach (byte b in dhcpBootFilename)
            {
                if (b >= 33 && b <= 126)
                {
                    char ch = Convert.ToChar(b);
                    bootFilenameStrBld.Append(ch);
                }
                else
                {
                    bootFilenameStrBld.Append(".");
                }
            }

            string dhcpOptions;

            UdpPacket pac = packet as UdpPacket;
            DhcpPacket dhcpPacket = new DhcpPacket(pac)
            {
                DhcpOpcode = (DhcpOpcodeEnum)dhcpOpcode,
                DhcpHardType = (DhcpHardwareTypeEnum)dhcpHardType,
                DhcpHardAdrLength = dhcpHardAdrLength,
                DhcpHopCount = dhcpHops,
                DhcpTransactionId = dhcpTransId,
                DhcpNumOfSeconds = dhcpSeconds,
                DhcpFlags = flagsFinal,
                DhcpClientIpAdr = clientIp.ToString(),
                DhcpYourIpAdr = yourIp.ToString(),
                DhcpServerIpAdr = serverIp.ToString(),
                DhcpGatewayIpAdr = gatewayIp.ToString(),
                DhcpClientHardAdr = clientHardAdr,
                DhcpServerName = serverNameStrBld.ToString(),
                DhcpBootFilename = bootFilenameStrBld.ToString(),
            };

            return dhcpPacket;
        }
    }
}
