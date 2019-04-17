using richClosure.Packets.ApplicationLayer;
using richClosure.Packets.TransportLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace richClosure.Packet_Sniffing.Factories.ApplicationFactories
{
    //TODO
    class DnsPacketFactory : IAbstractFactory
    {
        private BinaryReader _binaryReader;
        private Dictionary<string, object> _valueDictionary;

        public DnsPacketFactory(BinaryReader binaryReader, Dictionary<string, object> valueDictionary)
        {
            _binaryReader = binaryReader;
            _valueDictionary = valueDictionary;
        }
        public IPacket CreatePacket()
        {

            UdpPacket pac = packet as UdpPacket;
            IPacket dnsPacket = new DnsUdpPacket(pac)
            {
                DnsIdentification = dnsIdentification,
                DnsFlags = dnsFlagsObj,
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

        private void ReadPacketDataFromStream()
        {
            _valueDictionary["DnsIdentification"] = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadUInt16());
            UInt16 dnsFlagsAndCodes = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadUInt16());

            UInt16 dnsQuestions = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadUInt16());
            UInt16 dnsAnswersRR = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadUInt16());
            UInt16 dnsAuthRR = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadUInt16());
            UInt16 dnsAdditionalRR = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadUInt16());

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

            _valueDictionary["DnsQR"] = dnsQR;

            _valueDictionary["DnsOpcode"] = Convert.ToByte(dnsFlagsBinStr.Substring(1, 4), 10);

            _valueDictionary["DnsFLags"] = GetDnsFlags(dnsFlagsBinStr);

            _valueDictionary["DnsRcode"] = Convert.ToByte(dnsFlagsBinStr.Substring(12, 4), 10);

            List<DnsQuery> dnsQueries = new List<DnsQuery>();

            for (int i = 1; i <= dnsQuestions; i++)
            {
                StringBuilder stringBuilder = new StringBuilder();
                bool reading = true;

                while (reading)
                {
                    byte ch = _binaryReader.ReadByte();

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
                UInt16 dnsQueryType = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());
                UInt16 dnsQueryClass = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());

                dnsQueries.Add(new DnsQuery
                {
                    DnsQueryName = dnsQueryName,
                    DnsQueryType = (DnsTypeEnum)dnsQueryType,
                    DnsQueryClass = (DnsClassEnum)dnsQueryClass
                });
            }

            List<DnsRecord> dnsAnswers = new List<DnsRecord>();
            ParseDnsRecordHeader(dnsAnswers, dnsAnswersRR, dnsQueries[dnsQueries.Count - 1].DnsQueryName);

            List<DnsRecord> dnsAuths = new List<DnsRecord>();
            ParseDnsRecordHeader(dnsAuths, dnsAuthRR, dnsQueries[dnsQueries.Count - 1].DnsQueryName);

            List<DnsRecord> dnsAdditionals = new List<DnsRecord>();
            ParseDnsRecordHeader(dnsAdditionals, dnsAdditionalRR, dnsQueries[dnsQueries.Count - 1].DnsQueryName);


        }

        private DnsFlags GetDnsFlags(string flagsBinStr)
        {
            string dnsFlags = flagsBinStr.Substring(5, 7);
            int dnsFlagsInt = Convert.ToInt32(dnsFlags);
            DnsFlags dnsFlagsObj = new DnsFlags();

            if ((dnsFlagsInt & 1) != 0)
            {
                dnsFlagsObj.CD.IsSet = true;
            }
            if ((dnsFlagsInt & 2) != 0)
            {
                dnsFlagsObj.AD.IsSet = true;
            }
            if ((dnsFlagsInt & 4) != 0)
            {
                dnsFlagsObj.Z.IsSet = true;
            }
            if ((dnsFlagsInt & 8) != 0)
            {
                dnsFlagsObj.RA.IsSet = true;
            }
            if ((dnsFlagsInt & 16) != 0)
            {
                dnsFlagsObj.RA.IsSet = true;
            }
            if ((dnsFlagsInt & 32) != 0)
            {
                dnsFlagsObj.TC.IsSet = true;
            }
            if ((dnsFlagsInt & 64) != 0)
            {
                dnsFlagsObj.AA.IsSet = true;
            }

            return dnsFlagsObj;
        }

        private void ParseDnsRecordHeader(List<DnsRecord> recordList, int recordsCount, string queryName)
        {
            for (int procRecIndex = 1; procRecIndex <= recordsCount; procRecIndex++)
            {
                StringBuilder stringBuilder = new StringBuilder();
                string dnsRecordName;

                byte[] byteChar = _binaryReader.ReadBytes(2);

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

                DnsTypeEnum dnsRecordType = (DnsTypeEnum)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());
                DnsClassEnum dnsRecordClass = (DnsClassEnum)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());
                UInt32 dnsRecordTTL = (UInt32)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt32());
                UInt16 dnsRDataLength = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());

                byte[] byteData = _binaryReader.ReadBytes(dnsRDataLength);
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

