namespace richClosure.Packets.InternetLayer
{

    public enum IcmpTypeEnum
    {
        EchoReply = 0,
        DestinationUnreachable = 3,
        SourceQuench = 4,
        RedirectMessage = 5,
        EchoRequest = 8,
        RouterAdvertisment = 9,
        RouterSolicitation = 10,
        TimeExceeded = 11,
        ParameterProblem_BadIPHeader = 12,
        Timestamp = 13,
        TimestampReply = 14,
        InformationRequest = 15,
        InformationReply = 16,
        AdressMaskRequest = 17,
        AdressMaskReply = 18
    }

    public enum DestinationUnreachableCodeEnum
    {
        DestinationNetworkUnreachable = 0,
        DestinationHostUnreachable = 1,
        DestinationProtocolUnreachable = 2,
        DestinationPortUnreachable = 3,
        FragmentationRequired_DFFlagSet = 4,
        SourceRouteFailed = 5,
        DestinationNetworkUnknown = 6,
        DestinationHostUnknown = 7,
        SourceHostIsolated = 8,
        NetworkAdministrativelyProhibited = 9,
        HostAdministratiativelyProhibited = 10,
        NetworkUnreachableForToS = 11,
        HostUnreachableForToS = 12,
        CommunicationAdministrativelyProhibited = 13,
        HostPrecedenceViolation = 14,
        PrecedenceCutoffInEffect = 15
    }

    public enum RedirectMessageCodeEnum
    {
        RedirectDatagramForTheNetwork = 0,
        RedirectDatagramForTheHost = 1,
        RedirectDatagramForTheToSAndNetwork = 2,
        RedirectDatagramForTheToSAndHost = 3
    }

    public enum TimeExceededCodeEnum
    {
        TTL_ExpiredInTransit = 0,
        FragmentReassemblyTimeExceeded = 1
    }

    public enum ParameterProblemCodeEnum
    {
        PointerIndicatesTheError = 0,
        MissingARequiredOption = 1,
        BadLength = 2
    }

    class IcmpPacket : IpPacket
    {      
        public byte IcmpType { get; set; }
        public byte IcmpCode { get; set; }
        public uint IcmpChecksum { get; set; }
        public string IcmpRest { get; set; }

        public IcmpPacket(IPacket packet)
        {
            IpPacket pac = packet as IpPacket;

            PacketDisplayedProtocol = "ICMP";
            IpAppProtocol = pac.IpAppProtocol;
            PacketData = pac.PacketData;
            PacketId = pac.PacketId;
            TimeDateCaptured = pac.TimeDateCaptured;
            EthDestinationMacAdr = pac.EthDestinationMacAdr;
            EthSourceMacAdr = pac.EthSourceMacAdr;
            EthProtocol = pac.EthProtocol;
            IpProtocol = pac.IpProtocol;

            if (packet.IpVersion == 4)
            {

                IpVersion = pac.IpVersion;
                Ip4HeaderLength = pac.Ip4HeaderLength;
                Ip4Adrs = pac.Ip4Adrs;              
                Ip4Dscp = pac.Ip4Dscp;
                IpTotalLength = pac.IpTotalLength;
                Ip4Identification = pac.Ip4Identification;
                Ip4Offset = pac.Ip4Offset;
                Ip4Flags = pac.Ip4Flags;
                Ip4TimeToLive = pac.Ip4TimeToLive;

                Ip4HeaderChecksum = pac.Ip4HeaderChecksum;                             
            }
            else
            {
                PacketData = pac.PacketData;
                PacketId = pac.PacketId;
                IpVersion = pac.IpVersion;
                Ip6TrafficClass = pac.Ip6TrafficClass;
                Ip6FlowLabel = pac.Ip6FlowLabel;
                IpTotalLength = pac.IpTotalLength;
                IpProtocol = pac.IpProtocol;
                Ip6HopLimit = pac.Ip6HopLimit;
                Ip6Adrs = pac.Ip6Adrs;
                PacketDisplayedProtocol = "ICMP";
            }
        }
    }
}
