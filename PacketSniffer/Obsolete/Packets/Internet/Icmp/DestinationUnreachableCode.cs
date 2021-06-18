namespace PacketSniffer.Packets.Internet.Icmp
{
    public enum DestinationUnreachableCode
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
}