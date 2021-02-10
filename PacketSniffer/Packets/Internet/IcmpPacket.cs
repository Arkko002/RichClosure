namespace PacketSniffer.Packets.Internet
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
        ParameterProblemBadIpHeader = 12,
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
        FragmentationRequiredDfFlagSet = 4,
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
        TtlExpiredInTransit = 0,
        FragmentReassemblyTimeExceeded = 1
    }

    public enum ParameterProblemCodeEnum
    {
        PointerIndicatesTheError = 0,
        MissingARequiredOption = 1,
        BadLength = 2
    }

    public class IcmpPacket : IInternetPacket
    {      
        public byte Type { get;  }
        public byte Code { get; }
        public uint Checksum { get; }
        public uint Rest { get; }

        public IcmpPacket(byte type, byte code, uint checksum, uint rest, IPacket? previousHeader, PacketProtocol nextProtocol)
        {
            PacketProtocol = PacketProtocol.ICMP;
            Type = type;
            Code = code;
            Checksum = checksum;
            Rest = rest;
            PreviousHeader = previousHeader;
            NextProtocol = nextProtocol;
        }

        public PacketProtocol PacketProtocol { get; }
        public IPacket? PreviousHeader { get; }
        public PacketProtocol NextProtocol { get; }
    }
}
