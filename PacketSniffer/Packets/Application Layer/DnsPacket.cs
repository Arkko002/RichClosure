using System;
using System.Collections.Generic;
using PacketSniffer.Packets.Transport_Layer;

namespace PacketSniffer.Packets.Application_Layer
{
    public enum DnsOpcodeEnum
    {
        Query = 0,
        Iquery = 1,
        Status = 2,
        Notify = 4,
        Update = 5
    };

    public enum DnsRcodeEnum
    {
        NoError = 0,
        FormatError = 1,
        ServerFailure = 2,
        NameError = 3,
        NotImplemented = 4,
        Refused = 5,
        YxDomain = 6,
        YxrrSet = 7,
        NxrrSet = 8,
        NotAuth = 9,
        NotZone = 10,
        Badvers = 16,
        Badkey = 17,
        Badtime = 18,
        Badmode = 19,
        Badname = 20,
        Badalg = 21,
        Badtrunc = 22
    };

    public enum DnsTypeEnum
    {
        NoType = 0,
        A = 1,
        Ns = 2,
        Md = 3,
        Mf = 4,
        Cname = 5,
        Soa = 6,
        Mb = 7,
        Mg = 8,
        Mr = 9,
        Null = 10,
        Wks = 11,
        Ptr = 12,
        Hinfo = 13,
        Minfo = 14,
        Mx = 15,
        Txt = 16,
        Rp = 17,
        Afsdb = 18,
        X25 = 19,
        Isdn = 20,
        Rt = 21,
        Nsap = 22,
        NsapPtr = 23,
        Sig = 24,
        Key = 25,
        Px = 26,
        Gpos = 27,
        Aaaa = 28,
        Loc = 29,
        Nxt = 30,
        Eid = 31,
        Nimloc = 32,
        Srv = 33,
        Atma = 34,
        Naptr = 35,
        Kx = 36,
        Cert = 37,
        A6 = 38,
        Dnam = 39,
        Sink = 40,
        Opt = 41,
        Apl = 42,
        Ds = 43,
        Sshfp = 44,
        Ipseckey = 45,
        Rrsig = 46,
        Nsec = 47,
        Dnskey = 48,
        Dhcid = 49,
        Nsec3 = 50,
        Nsec3Param = 51,
        Tlsa = 52,
        Hip = 55,
        Ninfo = 56,
        Rkey = 57,
        Talink = 58,
        ChildDs = 59,
        Spf = 99,
        Uinfo = 100,
        Uid = 101,
        Gid = 102,
        Unspec = 103,
        Tkey = 249,
        Tsig = 250,
        Ixfr = 251,
        Axfr = 252,
        Mailb = 253,
        Maila = 254,
        Uri = 256,
        Caa = 257,
        DnssecTrust = 32768,
        DnssecVal = 32769
    };

    public enum DnsClassEnum
    {
        Reserved = 0,
        In = 1,
        Ch = 3,
        Hs = 4,
        None = 254,
        Any = 255
    };

    public class DnsFlags : IFlags
    {
        public CustomBool Aa = new CustomBool();
        public CustomBool Tc = new CustomBool();
        public CustomBool Rd = new CustomBool();
        public CustomBool Ra = new CustomBool();
        public CustomBool Z = new CustomBool();
        public CustomBool Ad = new CustomBool();
        public CustomBool Cd = new CustomBool();
    }

    public class DnsTcpPacket : TcpPacket
    {
        public ushort DnsIdentification { get; private set; }
        public string DnsQr { get; private set; }
        public DnsOpcodeEnum DnsOpcode { get; private set; }
        public DnsFlags DnsFlags { get; private set; }
        public DnsRcodeEnum DnsRcode { get; private set; }
        public ushort DnsQuestions { get; private set; }
        public ushort DnsAnswersRr { get; private set; }
        public ushort DnsAuthRr { get; private set; }
        public ushort DnsAdditionalRr { get; private set; }
        public List<DnsQuery> DnsQuerryList { get; private set; }
        public List<DnsRecord> DnsAnswerList { get; private set; }
        public List<DnsRecord> DnsAuthList { get; private set; }
        public List<DnsRecord> DnsAdditionalList { get; private set; }

        public DnsTcpPacket(Dictionary<string, object> valuesDictionary) : base(valuesDictionary)
        {
            SetDnsPacketValues(valuesDictionary);
            SetDisplayedProtocol("DNS");
        }

        private void SetDnsPacketValues(Dictionary<string, object> valuesDictionary)
        {
            DnsIdentification = (ushort)valuesDictionary["DnsIdentification"];
            DnsQr = (string)valuesDictionary["DnsQR"];
            DnsOpcode = (DnsOpcodeEnum)valuesDictionary["DnsOpcode"];
            DnsFlags = (DnsFlags)valuesDictionary["DnsFlags"];
            DnsRcode = (DnsRcodeEnum)valuesDictionary["DnsRcode"];
            DnsQuestions = (ushort)valuesDictionary["DnsQuestions"];
            DnsAnswersRr = (ushort)valuesDictionary["DnsAnswersRR"];
            DnsAuthRr = (ushort)valuesDictionary["DnsAuthRR"];
            DnsAdditionalRr = (ushort)valuesDictionary["DnsAdditionalRR"];
            DnsQuerryList = (List<DnsQuery>)valuesDictionary["DnsQuerryList"];
            DnsAnswerList = (List<DnsRecord>)valuesDictionary["DnsAnswerLIst"];
            DnsAuthList = (List<DnsRecord>)valuesDictionary["DnsAuthList"];
            DnsAdditionalList = (List<DnsRecord>)valuesDictionary["DnsAdditionalList"];
        }
    }

    public class DnsUdpPacket : UdpPacket
    {
        public ushort DnsIdentification { get; private set; }
        public string DnsQr { get; private set; }
        public DnsOpcodeEnum DnsOpcode { get; private set; }
        public DnsFlags DnsFlags { get; private set; }
        public DnsRcodeEnum DnsRcode { get; private set; }
        public ushort DnsQuestions { get; private set; }
        public ushort DnsAnswersRr { get; private set; }
        public ushort DnsAuthRr { get; private set; }
        public ushort DnsAdditionalRr { get; private set; }
        public List<DnsQuery> DnsQuerryList { get; private set; }
        public List<DnsRecord> DnsAnswerList { get; private set; }
        public List<DnsRecord> DnsAuthList { get; private set; }
        public List<DnsRecord> DnsAdditionalList { get; private set; }

        public DnsUdpPacket(Dictionary<string, object> valuesDictionary) : base(valuesDictionary)
        {
            SetDnsPacketValues(valuesDictionary);
            SetDisplayedProtocol("DNS");
        }

        private void SetDnsPacketValues(Dictionary<string, object> valuesDictionary)
        {
            DnsIdentification = (ushort)valuesDictionary["DnsIdentification"];
            DnsQr = (string)valuesDictionary["DnsQR"];
            DnsOpcode = (DnsOpcodeEnum)valuesDictionary["DnsOpcode"];
            DnsFlags = (DnsFlags)valuesDictionary["DnsFlags"];
            DnsRcode = (DnsRcodeEnum)valuesDictionary["DnsRcode"];
            DnsQuestions = (ushort)valuesDictionary["DnsQuestions"];
            DnsAnswersRr = (ushort)valuesDictionary["DnsAnswersRR"];
            DnsAuthRr = (ushort)valuesDictionary["DnsAuthRR"];
            DnsAdditionalRr = (ushort)valuesDictionary["DnsAdditionalRR"];
            DnsQuerryList = (List<DnsQuery>)valuesDictionary["DnsQuerryList"];
            DnsAnswerList = (List<DnsRecord>)valuesDictionary["DnsAnswerLIst"];
            DnsAuthList = (List<DnsRecord>)valuesDictionary["DnsAuthList"];
            DnsAdditionalList = (List<DnsRecord>)valuesDictionary["DnsAdditionalList"];
        }
    }

    public class DnsQuery
    {
        public string DnsQueryName { get; set; }
        public DnsTypeEnum DnsQueryType { get; set; }
        public DnsClassEnum DnsQueryClass { get; set; }
    }

    public class DnsRecord
    {
        public string DnsRecordName { get; set; }
        public DnsTypeEnum DnsRecordType { get; set; }
        public DnsClassEnum DnsRecordClass { get; set; }
        public UInt32 DnsTimeToLive { get; set; }
        public UInt16 DnsRdataLength { get; set; }
        public string DnsRdata { get; set; }
    }
}
