using System.Collections.Generic;
using richClosure.Packet_Sniffing.Factories.ApplicationFactories;
using richClosure.Packets.InternetLayer;
using richClosure.Packets.TransportLayer;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace richClosure.Packet_Sniffing.Factories.TransportFactories
{
    class TcpPacketFactory : IAbstractFactory
    {
        private BinaryReader _binaryReader;
        private Dictionary<string, object> _valueDictionary;

        public TcpPacketFactory(BinaryReader binaryReader, Dictionary<string, object> valueDictionary)
        {
            _binaryReader = binaryReader;
            _valueDictionary = valueDictionary;
        }

        public IPacket CreatePacket()
        {
            ReadPacketDataFromStream();
            IPacket packet = new TcpPacket(_valueDictionary);

            return packet;
        }

        private void ReadPacketDataFromStream()
        {
            _valueDictionary["PacketDisplayedProtocol"] = "TCP";

            UInt16 tcpSourcePort = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadInt16());
            UInt16 tcpDestinationPort = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadInt16());

            _valueDictionary["TcpSequenceNumber"] = (UInt32)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadInt32());
            _valueDictionary["TcpAckNumber"] = (UInt32)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadInt32());

            byte tcpDataOffsetAndNs = _binaryReader.ReadByte();

            _valueDictionary["TcpDataOffset"] = tcpDataOffsetAndNs <<= 4;

            byte tcpNs = tcpDataOffsetAndNs;
            tcpNs <<= 7;
            tcpNs >>= 7;
            _valueDictionary["TcpNs"] = tcpNs;

            byte tcpFlags = _binaryReader.ReadByte();


            StringBuilder tcpFlagsBinStr = new StringBuilder();
            tcpFlagsBinStr.Append(Convert.ToString(tcpFlags, 2));

            while (tcpFlagsBinStr.Length != 8)
            {
                tcpFlagsBinStr = tcpFlagsBinStr.Insert(0, "0");
            }

            tcpFlagsBinStr = tcpFlagsBinStr.Insert(0, tcpNs.ToString());
            int tcpFlagsInt = Convert.ToInt32(tcpFlagsBinStr.ToString());

            var tcpFlagsObj = CreateTcpFlagsObject(tcpFlagsInt);

            _valueDictionary["TcpFlags"] = tcpFlagsObj;

            _valueDictionary["TcpWindowSize"] = (UInt16)IPAddress.NetworkToHostOrder(
                                _binaryReader.ReadInt16());
            _valueDictionary["TcpChecksum"] = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadInt16());
            _valueDictionary["TcpUrgentPointer"] = (UInt16)IPAddress.NetworkToHostOrder(
                                            _binaryReader.ReadInt16());

            //TODO
            if (Convert.ToInt32(_valueDictionary["TcpDataOffset"]) > 5)
            {

            }

            _valueDictionary["TcpPorts"] = new Dictionary<string, string>()
            {
                {"dst", tcpDestinationPort.ToString()},
                {"src", tcpSourcePort.ToString()}
            };
        }

        private TcpFlags CreateTcpFlagsObject(int tcpFlagsInt)
        {
            TcpFlags tcpFlagsObj = new TcpFlags();

            if ((tcpFlagsInt & 1) != 0)
            {
                tcpFlagsObj.FIN.IsSet = true;
            }
            if ((tcpFlagsInt & 2) != 0)
            {
                tcpFlagsObj.SYN.IsSet = true;
            }
            if ((tcpFlagsInt & 4) != 0)
            {
                tcpFlagsObj.RST.IsSet = true;
            }
            if ((tcpFlagsInt & 8) != 0)
            {
                tcpFlagsObj.PSH.IsSet = true;
            }
            if ((tcpFlagsInt & 16) != 0)
            {
                tcpFlagsObj.ACK.IsSet = true;
            }
            if ((tcpFlagsInt & 32) != 0)
            {
                tcpFlagsObj.URG.IsSet = true;
            }
            if ((tcpFlagsInt & 64) != 0)
            {
                tcpFlagsObj.ECE.IsSet = true;
            }
            if ((tcpFlagsInt & 128) != 0)
            {
                tcpFlagsObj.CWR.IsSet = true;
            }
            if ((tcpFlagsInt & 256) != 0)
            {
                tcpFlagsObj.NS.IsSet = true;
            }

            return tcpFlagsObj;
        }

        public AppProtocolEnum CheckForAppLayerPorts(IPacket packet)
        {
            TcpPacket tcpPac = packet as TcpPacket;

            if (tcpPac.TcpPorts.Any(x => x.Value.Equals(53)))
            {
                return AppProtocolEnum.DNS;
            }
            else if (tcpPac.TcpPorts.Any(x => x.Value.Equals(80)))
            {
                return AppProtocolEnum.HTTP;
            }
            else if (tcpPac.TcpPorts.Any(x => x.Value.Equals(443)))
            {
                return AppProtocolEnum.TLS;
            }
            else
            {
                return AppProtocolEnum.NoAppProtocol;
            }

        }
    }
}
