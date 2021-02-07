using System.Collections.Generic;
using PacketSniffer.Packets.Internet_Layer;

namespace PacketSniffer.Packets.Transport_Layer
{
    public class UdpPacket : ITransportPacket
    {
        public ushort DestinationPort { get; }
        public ushort SourcePort { get; }
        public ushort Length { get; }
        public ushort Checksum { get; }

        public UdpPacket(Dictionary<string, object> valuesDictionary) : base(valuesDictionary)
        {
            Length = (ushort)valuesDictionary["UdpLength"];
            Checksum = (ushort)valuesDictionary["UdpChecksum"];
            UdpPorts = (Dictionary<string, string>)valuesDictionary["UdpPorts"];
        }
    }
}
