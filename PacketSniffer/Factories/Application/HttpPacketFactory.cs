using System.Collections.Generic;
using System.IO;
using System.Text;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Application;
using PacketSniffer.Packets.Internet;
using PacketSniffer.Packets.Internet.Ip;

namespace PacketSniffer.Factories.Application
{
    internal class HttpPacketFactory : IApplicationPacketFactory
    {
        private readonly BinaryReader _binaryReader;
        private IPacket _previousHeader;

        public HttpPacketFactory(BinaryReader binaryReader, IPacket previousHeader)

        {
            _binaryReader = binaryReader;
            _previousHeader = previousHeader;
        }

        
        public IPacket CreatePacket()
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            List<byte> byteList = new List<byte>();

            //Go down two headers to access IP packet
            var ipHeader = (Ip4Packet)_previousHeader.PreviousHeader;
            //TODO Ipv6 support

            //TODO
            while (_binaryReader.BaseStream.Position < ipHeader.TotalLength)
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

            return new HttpPacket(fields, _previousHeader, PacketProtocol.NoProtocol);
        }
    }
}
