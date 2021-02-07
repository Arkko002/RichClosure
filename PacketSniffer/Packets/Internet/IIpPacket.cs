using System.Net;

namespace PacketSniffer.Packets.Internet_Layer
{
    public interface IIpPacket
    {
        byte Version { get; }
        IPAddress SourceAddress { get; }
        IPAddress DestinationAddress { get; }
    }
}