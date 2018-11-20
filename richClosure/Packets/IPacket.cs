using System;
using System.Windows.Controls;

namespace richClosure
{
    public enum IpProtocolEnum
    {
        ICMP = 1,
        TCP = 6,
        UDP = 17
    };

    public enum IpFlagsEnum
    {
        NoFlags = 0,
        DontFragment = 2,
        MoreFragments = 4
    };

    public enum AppProtocolEnum
    {
        NoAppProtocol = 0,
        DNS,
        DHCP,
        HTTP,
        TLS
    };


    interface IPacket
    {
        byte IpVersion { get; set; }
        string PacketData { get; set; }
        UInt16 IpTotalLength { get; set; }
        ulong PacketId { get; set; }
        string TimeDateCaptured { get; set; }
        IpProtocolEnum IpProtocol { get; set; }
        AppProtocolEnum IpAppProtocol { get; set; }
        string PacketDisplayedProtocol { get; set; }
        string PacketComment { get; set; }
    }
}
