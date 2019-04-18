using System.Collections.Generic;
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

    public enum AppProtocolEnum
    {
        NoAppProtocol = 0,
        DNS,
        DHCP,
        HTTP,
        TLS
    };

    public interface IPacket
    {
        byte IpVersion { get; }
        string PacketData { get; }
        ushort IpTotalLength { get; }
        ulong PacketId { get; }
        string TimeDateCaptured { get; }
        IpProtocolEnum IpProtocol { get; }
        AppProtocolEnum IpAppProtocol { get; }
        string PacketDisplayedProtocol { get; }
        string PacketComment { get; }

        void SetPacketValues(Dictionary<string, object> valuesDictionary);
    }
}