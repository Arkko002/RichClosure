using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using PacketSniffer.Packets;
using PacketSniffer.Packets.Application;
using PacketSniffer.Packets.Application.Dns;

namespace PacketSniffer.Factories.Application
{
    internal class DnsPacketFactory : IApplicationPacketFactory
    {
        private readonly BinaryReader _binaryReader;
        private readonly IPacketFrame _frame;
        private readonly IPacket _previousHeader;

        public DnsPacketFactory(BinaryReader binaryReader, IPacket previousHeader, IPacketFrame frame)
        {
            _binaryReader = binaryReader;
            _previousHeader = previousHeader;
            _frame = frame;
        }

        public IPacket CreatePacket()
        {
            var identification = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());
            var flagsAndCodes = new BitArray(IPAddress.NetworkToHostOrder(_binaryReader.ReadUInt16()));

            var qr = flagsAndCodes[0] ? DnsQr.Query : DnsQr.Response;

            var opcodeInt = 0;
            for (int i = 1; i <= 4; i++)
            {
                if (flagsAndCodes[i])
                {
                    opcodeInt += Convert.ToInt16(Math.Pow(2, i));
                }
            }
            var opcode = (DnsOpcode) opcodeInt;
            
            bool aa = flagsAndCodes[5];
            bool tc = flagsAndCodes[6];
            bool rd = flagsAndCodes[7];
            bool ra = flagsAndCodes[8];
            bool z = false;

            var rcodeInt = 0;
            for (int i = 12; i <= 15; i++)
            {
                if (flagsAndCodes[i])
                {
                    rcodeInt += Convert.ToInt16(Math.Pow(2, i));
                }
            }
            var rcode = (DnsRcode) rcodeInt;

            var questions = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());
            var answersRr = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());
            var authRr = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());
            var additionalRr = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());

            List<DnsQuery> dnsQueries = new List<DnsQuery>();

            //TODO
            for (int i = 1; i <= questions; i++)
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
                    QueryName = dnsQueryName,
                    QueryType = (DnsType)dnsQueryType,
                    QueryClass = (DnsClass)dnsQueryClass
                });
            }

            var dnsAnswers = new List<DnsRecord>();
            ParseDnsRecordHeader(dnsAnswers, answersRr, dnsQueries[dnsQueries.Count - 1].QueryName);

            var dnsAuths = new List<DnsRecord>();
            ParseDnsRecordHeader(dnsAuths, authRr, dnsQueries[dnsQueries.Count - 1].QueryName);

            var dnsAdditionals = new List<DnsRecord>();
            ParseDnsRecordHeader(dnsAdditionals, additionalRr, dnsQueries[dnsQueries.Count - 1].QueryName);

            var answers = dnsAnswers;
            var auth = dnsAuths;
            var additionals = dnsAdditionals;

            var packet = new DnsPacket(identification, qr, opcode, rcode, questions, answersRr, authRr, additionalRr,
                dnsQueries, dnsAnswers, dnsAuths, dnsAdditionals, _previousHeader, PacketProtocol.NoProtocol);
            _frame.Packet = packet;

            return packet;
        }

        private void ParseDnsRecordHeader(List<DnsRecord> recordList, int recordsCount, string queryName)
        {
            for (int procRecIndex = 1; procRecIndex <= recordsCount; procRecIndex++)
            {
                string dnsRecordName;

                byte[] byteChar = _binaryReader.ReadBytes(2);

                if (procRecIndex == 1)
                {
                    dnsRecordName = queryName;
                }
                else
                {
                    dnsRecordName = recordList[procRecIndex - 2].RecordType == DnsType.Cname ? recordList[procRecIndex - 2].Rdata : recordList[procRecIndex - 2].RecordName;
                }

                DnsType recordType = (DnsType)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());
                DnsClass recordClass = (DnsClass)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());
                UInt32 recordTtl = (UInt32)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt32());
                UInt16 rDataLength = (UInt16)IPAddress.NetworkToHostOrder(_binaryReader.ReadInt16());

                byte[] byteData = _binaryReader.ReadBytes(rDataLength);
                StringBuilder dataBuilder = new StringBuilder();

                switch (recordType)
                {
                    case DnsType.A:

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
                    RecordName = dnsRecordName,
                    RecordType = recordType,
                    RecordClass = recordClass,
                    TimeToLive = recordTtl,
                    RdataLength = rDataLength,
                    Rdata = dataBuilder.ToString()
                });
            }
        }

    }
}

