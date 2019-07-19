using System.Collections.Generic;
using System.IO;
using System.Text;
using PacketSniffer.Packet_Sniffing.Packet_Factories.AbstractFactories;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Application_Layer;

namespace PacketSniffer.Packet_Sniffing.Packet_Factories.ApplicationFactories
{
    internal class HttpPacketByteFactory : IAbstractFactory
    {
        private readonly BinaryReader _binaryReader;
        private readonly Dictionary<string, object> _valueDictionary;

        public HttpPacketByteFactory(BinaryReader binaryReader, Dictionary<string, object> valueDictionary)
        {
            _binaryReader = binaryReader;
            _valueDictionary = valueDictionary;
        }

        public IPacket CreatePacket()
        {
            ReadPacketDataFromStream();

            IPacket httpPacket = new HttpPacket(_valueDictionary);

            return httpPacket;
        }

        private void ReadPacketDataFromStream()
        {
            _valueDictionary["AppProtocol"] = AppProtocolEnum.Http;
            _valueDictionary["PacketDisplayedProtocol"] = "HTTP";

            Dictionary<string, string> fields = new Dictionary<string, string>();
            List<byte> byteList = new List<byte>();

            while (_binaryReader.BaseStream.Position < (uint)_valueDictionary["IpTotalLength"])
            {
                byteList.Add(_binaryReader.ReadByte());
            }

            for (int i = 0; i < byteList.Count; i++)
            {
                if (byteList[i] == 0x0d && byteList[i + 1] == 0x0a)
                {
                    if (fields.Count == 0)
                    {
                        byte[] by = byteList.GetRange(0, i).ToArray();
                        string header = Encoding.Default.GetString(by);
                        fields.Add("FirstLine", header);

                        byteList.RemoveRange(0, i + 2);
                        i = 0;
                    }
                    else
                    {
                        byte[] by = byteList.GetRange(0, i).ToArray();
                        string headerAndData = Encoding.Default.GetString(by);
                        string[] splitHeader = headerAndData.Split(':');

                        if (splitHeader[0].Contains("Date"))
                        {
                            for (int z = 2; z < splitHeader.Length; z++)
                            {
                                splitHeader[1] += ":" + splitHeader[z];
                            }
                        }

                        fields.Add(splitHeader[0], splitHeader.Length > 1 ? splitHeader[1] : string.Empty);

                        byteList.RemoveRange(0, i + 2);
                        i = 0;
                    }
                }
            }

            _valueDictionary["HttpFieldsDict"] = fields;
        }
    }
}
