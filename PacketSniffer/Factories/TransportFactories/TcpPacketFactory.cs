using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Transport_Layer;

namespace PacketSniffer.Factories.TransportFactories
{
    internal class TcpPacketByteFactory : IAbstractFactory
    {
        private readonly BinaryReader _binaryReader;
        private readonly Dictionary<string, object> _valueDictionary;

        public TcpPacketByteFactory(BinaryReader binaryReader, Dictionary<string, object> valueDictionary)
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

            // TODO
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
                tcpFlagsObj.Fin.IsSet = true;
            }
            if ((tcpFlagsInt & 2) != 0)
            {
                tcpFlagsObj.Syn.IsSet = true;
            }
            if ((tcpFlagsInt & 4) != 0)
            {
                tcpFlagsObj.Rst.IsSet = true;
            }
            if ((tcpFlagsInt & 8) != 0)
            {
                tcpFlagsObj.Psh.IsSet = true;
            }
            if ((tcpFlagsInt & 16) != 0)
            {
                tcpFlagsObj.Ack.IsSet = true;
            }
            if ((tcpFlagsInt & 32) != 0)
            {
                tcpFlagsObj.Urg.IsSet = true;
            }
            if ((tcpFlagsInt & 64) != 0)
            {
                tcpFlagsObj.Ece.IsSet = true;
            }
            if ((tcpFlagsInt & 128) != 0)
            {
                tcpFlagsObj.Cwr.IsSet = true;
            }
            if ((tcpFlagsInt & 256) != 0)
            {
                tcpFlagsObj.Ns.IsSet = true;
            }

            return tcpFlagsObj;
        }

        public AppProtocolEnum CheckForAppLayerPorts(IPacket packet)
        {
            TcpPacket tcpPac = packet as TcpPacket;

            if (tcpPac.TcpPorts.Any(x => x.Value.Equals(53)))
            {
                return AppProtocolEnum.Dns;
            }
            else if (tcpPac.TcpPorts.Any(x => x.Value.Equals(80)))
            {
                return AppProtocolEnum.Http;
            }
            else if (tcpPac.TcpPorts.Any(x => x.Value.Equals(443)))
            {
                return AppProtocolEnum.Tls;
            }
            else
            {
                return AppProtocolEnum.NoAppProtocol;
            }

        }
    }
}
