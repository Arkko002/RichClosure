using System.Net;

namespace PacketSniffer.Packets.Internet.Ip
{
    public interface IIpPacket : IPacket
    {
        byte Version { get; }
        IpProtocol NextHeaderProtocol { get; }
        IPAddress SourceAddress { get; }
        IPAddress DestinationAddress { get; }
    }
}