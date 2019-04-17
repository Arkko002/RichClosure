using System.Collections.Generic;
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
        public byte IcmpType { get; private set; }
        public byte IcmpCode { get; private set; }
        public uint IcmpChecksum { get; private set; }
        public string IcmpRest { get; private set; }

        public IcmpPacket(Dictionary<string, object> valueDictionary) : base(valueDictionary) 
        {
            SetIcmpPacketValues(valueDictionary);
            SetDisplayedProtocol("ICMP");
        }

        private void SetIcmpPacketValues(Dictionary<string, object> valuesDictionary)
        {
            IcmpType = (byte)valuesDictionary["IcmpType"];
            IcmpCode = (byte)valuesDictionary["IcmpCode"];
            IcmpChecksum = (uint)valuesDictionary["IcmpChecksum"];
            IcmpRest = (string)valuesDictionary["IcmpRest"];
        }
    }
}
