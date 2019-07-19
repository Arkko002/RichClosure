using System.Collections.Generic;
using PacketSniffer.Packets.Internet_Layer;

namespace PacketSniffer.Packets.Transport_Layer
{
    public class UdpPacket : IpPacket
    {
        public ushort UdpLength { get; private set; }
        public ushort UdpChecksum { get; private set; }
        public Dictionary<string, string> UdpPorts { get; private set; }

        public UdpPacket(Dictionary<string, object> valuesDictionary) : base(valuesDictionary)
        {
            SetUdpPacketValues(valuesDictionary);
            SetDisplayedProtocol("UDP");
        }

        private void SetUdpPacketValues(Dictionary<string, object> valuesDictionary)
        {
            UdpLength = (ushort)valuesDictionary["UdpLength"];
            UdpChecksum = (ushort)valuesDictionary["UdpChecksum"];
            UdpPorts = (Dictionary<string, string>)valuesDictionary["UdpPorts"];
        }
    }
}
