using richClosure.Packets.ApplicationLayer;
using richClosure.Packets.TransportLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace richClosure.Packet_Sniffing.Factories.ApplicationFactories
{
    class DnsPacketFactory : IAbstractPacketFactory
    {
        public IPacket CreatePacket(IPacket packet, BinaryReader binaryReader)
        {
            UInt16 dnsIdentification = (UInt16)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            UInt16 dnsFlagsAndCodes = (UInt16)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            UInt16 dnsQuestions = (UInt16)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            UInt16 dnsAnswersRR = (UInt16)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            UInt16 dnsAuthRR = (UInt16)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
            UInt16 dnsAdditionalRR = (UInt16)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            string dnsFlagsBinStr = Convert.ToString(dnsFlagsAndCodes, 2);

            while (dnsFlagsBinStr.Length != 16)
            {
                dnsFlagsBinStr = dnsFlagsBinStr.Insert(0, "0");
            }

            string dnsQR = String.Empty;

            if (dnsFlagsBinStr[0] == 0)
            {
                dnsQR = "Query";
            }
            else
            {
                dnsQR = "Response";
            }

            byte dnsOpcode = Convert.ToByte(dnsFlagsBinStr.Substring(1, 4), 10);

            string dnsFlags = dnsFlagsBinStr.Substring(5, 7);

            StringBuilder flagsBuilder = new StringBuilder();

            for (int i = 0; i < dnsFlags.Length; i++)
            {
                if (dnsFlags[i] == 1)
                {
                    flagsBuilder.Append((DnsFlagsEnum)i);
                }
            }

            if (flagsBuilder.Length == 0)
            {
                flagsBuilder.Append("No Flags");
            }

            string dnsFlagsFinal = flagsBuilder.ToString();

            byte dnsRcode = Convert.ToByte(dnsFlagsBinStr.Substring(12, 4), 10);

            List<DnsQuery> dnsQueries = new List<DnsQuery>();

            for (int i = 1; i <= dnsQuestions; i++)
            {
                StringBuilder stringBuilder = new StringBuilder();
                bool reading = true;

                while (reading)
                {
                    byte ch = binaryReader.ReadByte();

                    if (ch >= 33 && ch <= 126)
                    {
                        stringBuilder.Append(Convert.ToChar(ch));
                    }
                    else
                    {
                        stringBuilder.Append(".");
                    }

                    if (ch == 0x0)
                    {
                        reading = false;
                    }
                }
                string dnsQueryName = stringBuilder.ToString();
                UInt16 dnsQueryType = (UInt16)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                UInt16 dnsQueryClass = (UInt16)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                dnsQueries.Add(new DnsQuery
                {
                    DnsQueryName = dnsQueryName,
                    DnsQueryType = (DnsTypeEnum)dnsQueryType,
                    DnsQueryClass = (DnsClassEnum)dnsQueryClass
                });
            }

            List<DnsRecord> dnsAnswers = new List<DnsRecord>();
            ParseDnsRecordHeader(dnsAnswers, dnsAnswersRR, binaryReader, dnsQueries[dnsQueries.Count - 1].DnsQueryName);

            List<DnsRecord> dnsAuths = new List<DnsRecord>();
            ParseDnsRecordHeader(dnsAuths, dnsAuthRR, binaryReader, dnsQueries[dnsQueries.Count - 1].DnsQueryName);

            List<DnsRecord> dnsAdditionals = new List<DnsRecord>();
            ParseDnsRecordHeader(dnsAdditionals, dnsAdditionalRR, binaryReader, dnsQueries[dnsQueries.Count - 1].DnsQueryName);


            UdpPacket pac = packet as UdpPacket;
            IPacket dnsPacket = new DnsUdpPacket(pac)
            {
                DnsIdentification = dnsIdentification,
                DnsFlags = dnsFlagsFinal,
                DnsOpcode = (DnsOpcodeEnum)dnsOpcode,
                DnsQR = dnsQR,
                DnsRcode = (DnsRcodeEnum)dnsRcode,
                DnsQuestions = dnsQuestions,
                DnsAnswersRR = dnsAnswersRR,
                DnsAuthRR = dnsAuthRR,
                DnsAdditionalRR = dnsAdditionalRR,
                DnsQuerryList = dnsQueries,
                DnsAnswerList = dnsAnswers,
                DnsAuthList = dnsAuths,
                DnsAdditionalList = dnsAdditionals,
            };


            return dnsPacket;
        }

        private void ParseDnsRecordHeader(List<DnsRecord> recordList, int recordsCount, BinaryReader binaryReader, string queryName)
        {
            for (int procRecIndex = 1; procRecIndex <= recordsCount; procRecIndex++)
            {
                StringBuilder stringBuilder = new StringBuilder();
                string dnsRecordName;

                byte[] byteChar = binaryReader.ReadBytes(2);

                if (procRecIndex == 1)
                {
                    dnsRecordName = queryName;
                }
                else
                {
                    if (recordList[procRecIndex - 2].DnsRecordType == DnsTypeEnum.CNAME)
                    {
                        dnsRecordName = recordList[procRecIndex - 2].DnsRdata;
                    }
                    else
                    {
                        dnsRecordName = recordList[procRecIndex - 2].DnsRecordName;
                    }
                }

                DnsTypeEnum dnsRecordType = (DnsTypeEnum)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                DnsClassEnum dnsRecordClass = (DnsClassEnum)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                UInt32 dnsRecordTTL = (UInt32)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
                UInt16 dnsRDataLength = (UInt16)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                byte[] byteData = binaryReader.ReadBytes(dnsRDataLength);
                StringBuilder dataBuilder = new StringBuilder();

                switch (dnsRecordType)
                {
                    case DnsTypeEnum.A:

                        foreach (byte b in byteData)
                        {
                            dataBuilder.Append(b.ToString() + ".");
                        }
                        break;

                    default:
                        foreach (byte b in byteData)
                        {
                            if (b >= 33 && b <= 126)
                            {
                                dataBuilder.Append(Convert.ToChar(b));
                            }
                            else
                            {
                                dataBuilder.Append(".");
                            }
                        }
                        break;
                }

                recordList.Add(new DnsRecord
                {
                    DnsRecordName = dnsRecordName,
                    DnsRecordType = dnsRecordType,
                    DnsRecordClass = dnsRecordClass,
                    DnsTimeToLive = dnsRecordTTL,
                    DnsRdataLength = dnsRDataLength,
                    DnsRdata = dataBuilder.ToString()
                });
            }
        }
    }
}

