using richClosure.Packets.ApplicationLayer;
using richClosure.Packets.TransportLayer;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace richClosure.Packet_Sniffing.Factories.ApplicationFactories
{
    class HttpPacketFactory : IAbstractPacketFactory
    {
        public IPacket CreatePacket(IPacket packet, BinaryReader binaryReader)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            List<byte> byteList = new List<byte>();

            while (binaryReader.BaseStream.Position < packet.IpTotalLength)
            {
                byteList.Add(binaryReader.ReadByte());
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

                        if (splitHeader.Length > 1)
                        {
                            fields.Add(splitHeader[0], splitHeader[1]);
                        }
                        else
                        {
                            fields.Add(splitHeader[0], string.Empty);
                        }


                        byteList.RemoveRange(0, i + 2);
                        i = 0;
                    }
                }
            }

            if (fields.Count == 0)
            {
                return packet;
            }

            TcpPacket pac = packet as TcpPacket;
            HttpPacket httpPacket = new HttpPacket(pac)
            {
                HttpFieldsDict = fields,
            };

            return httpPacket;
            
        }
    }
}
