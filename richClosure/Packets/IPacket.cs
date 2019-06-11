using System.Collections.Generic;

namespace richClosure.Packets
{
    public enum IpProtocolEnum
    {
        Icmp = 1,
        Tcp = 6,
        Udp = 17
    };

    public enum AppProtocolEnum
    {
        NoAppProtocol = 0,
        Dns,
        Dhcp,
        Http,
        Tls
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