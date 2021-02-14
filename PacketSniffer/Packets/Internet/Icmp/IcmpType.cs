namespace PacketSniffer.Packets.Internet.Icmp
{
    public enum IcmpType
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
}