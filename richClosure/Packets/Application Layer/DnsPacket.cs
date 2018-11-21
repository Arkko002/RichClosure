using System;
using System.Collections.Generic;
using richClosure.Packets.TransportLayer;

namespace richClosure.Packets.ApplicationLayer
{
    enum DnsOpcodeEnum
    {
        QUERY = 0,
        IQUERY = 1,
        STATUS = 2,
        Notify = 4,
        Update = 5
    };

    enum DnsRcodeEnum
    {
        NoError = 0,
        FormatError = 1,
        ServerFailure = 2,
        NameError = 3,
        NotImplemented = 4,
        Refused = 5,
        YXDomain = 6,
        YXRRSet = 7,
        NXRRSet = 8,
        NotAuth = 9,
        NotZone = 10,
        BADVERS = 16,
        BADKEY = 17,
        BADTIME = 18,
        BADMODE = 19,
        BADNAME = 20,
        BADALG = 21,
        BADTRUNC = 22
    };

    enum DnsTypeEnum
    {
        NoType = 0,
        A = 1,
        NS = 2,
        MD = 3,
        MF = 4,
        CNAME = 5,
        SOA = 6,
        MB = 7,
        MG = 8,
        MR = 9,
        NULL = 10,
        WKS = 11,
        PTR = 12,
        HINFO = 13,
        MINFO = 14,
        MX = 15,
        TXT = 16,
        RP = 17,
        AFSDB = 18,
        X25 = 19,
        ISDN = 20,
        RT = 21,
        NSAP = 22,
        NSAP_PTR = 23,
        SIG = 24,
        KEY = 25,
        PX = 26,
        GPOS = 27,
        AAAA = 28,
        LOC = 29,
        NXT = 30,
        EID = 31,
        NIMLOC = 32,
        SRV = 33,
        ATMA = 34,
        NAPTR = 35,
        KX = 36,
        CERT = 37,
        A6 = 38,
        DNAM = 39,
        SINK = 40,
        OPT = 41,
        APL = 42,
        DS = 43,
        SSHFP = 44,
        IPSECKEY = 45,
        RRSIG = 46,
        NSEC = 47,
        DNSKEY = 48,
        DHCID = 49,
        NSEC3 = 50,
        NSEC3PARAM = 51,
        TLSA = 52,
        HIP = 55,
        NINFO = 56,
        RKEY = 57,
        TALINK = 58,
        Child_DS = 59,
        SPF = 99,
        UINFO = 100,
        UID = 101,
        GID = 102,
        UNSPEC = 103,
        TKEY = 249,
        TSIG = 250,
        IXFR = 251,
        AXFR = 252,
        MAILB = 253,
        MAILA = 254,
        URI = 256,
        CAA = 257,
        DNSSEC_Trust = 32768,
        DNSSEC_Val = 32769      
    };

    enum DnsClassEnum
    {
        Reserved = 0,
        IN = 1,
        CH = 3,
        HS = 4,
        None = 254,
        Any = 255
    };

    class DnsFlags
    {
        public CustomBool AA = new CustomBool();
        public CustomBool TC = new CustomBool();
        public CustomBool RD = new CustomBool();
        public CustomBool RA = new CustomBool();
        public CustomBool Z = new CustomBool();
        public CustomBool AD = new CustomBool();
        public CustomBool CD = new CustomBool();
    }

    class DnsTcpPacket : TcpPacket
    {
        public UInt16 DnsIdentification { get; set; }
        public string DnsQR { get; set; }
        public DnsOpcodeEnum DnsOpcode { get; set; }
        public DnsFlags DnsFlags { get; set; }
        public DnsRcodeEnum DnsRcode { get; set; }
        public UInt16 DnsQuestions { get; set; }
        public UInt16 DnsAnswersRR { get; set; }
        public UInt16 DnsAuthRR { get; set; }
        public UInt16 DnsAdditionalRR { get; set; }
        public List<DnsQuery> DnsQuerryList { get; set; }
        public List<DnsRecord> DnsAnswerList { get; set; }
        public List<DnsRecord> DnsAuthList { get; set; }
        public List<DnsRecord> DnsAdditionalList { get; set; }

        public DnsTcpPacket(TcpPacket packet) : base(packet)
        {
            TcpAckNumber = packet.TcpAckNumber;
            TcpChecksum = packet.TcpChecksum;
            TcpDataOffset = packet.TcpDataOffset;
            TcpPorts = packet.TcpPorts;
            TcpFlags = packet.TcpFlags;
            TcpSequenceNumber = packet.TcpSequenceNumber;          
            TcpUrgentPointer = packet.TcpUrgentPointer;
            TcpWindowSize = packet.TcpWindowSize;
            IpAppProtocol = AppProtocolEnum.DNS;
            PacketDisplayedProtocol = "DNS";
        }
    }

    class DnsUdpPacket : UdpPacket
    {
        public UInt16 DnsIdentification { get; set; }
        public string DnsQR { get; set; }
        public DnsOpcodeEnum DnsOpcode { get; set; }
        public DnsFlags DnsFlags { get; set; }
        public DnsRcodeEnum DnsRcode { get; set; }
        public UInt16 DnsQuestions { get; set; }
        public UInt16 DnsAnswersRR { get; set; }
        public UInt16 DnsAuthRR { get; set; }
        public UInt16 DnsAdditionalRR { get; set; }
        public List<DnsQuery> DnsQuerryList { get; set; }
        public List<DnsRecord> DnsAnswerList { get; set; }
        public List<DnsRecord> DnsAuthList { get; set; }
        public List<DnsRecord> DnsAdditionalList { get; set; }

        public DnsUdpPacket(UdpPacket packet) : base(packet)
        {
            UdpLength = packet.UdpLength;
            UdpPorts = packet.UdpPorts;
            UdpChecksum = packet.UdpChecksum;
            IpAppProtocol = AppProtocolEnum.DNS;
            PacketDisplayedProtocol = "DNS";
        }
    }

    class DnsQuery
    {
        public string DnsQueryName { get; set; }
        public DnsTypeEnum DnsQueryType { get; set; }
        public DnsClassEnum DnsQueryClass { get; set; }
    }

    class DnsRecord
    {
        public string DnsRecordName { get; set; }
        public DnsTypeEnum DnsRecordType { get; set; }
        public DnsClassEnum DnsRecordClass { get; set; }
        public UInt32 DnsTimeToLive { get; set; }
        public UInt16 DnsRdataLength { get; set; }
        public string DnsRdata { get; set; }
    }
}
