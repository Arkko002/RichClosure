using System.Collections.Generic;
using richClosure.Packets.TransportLayer;

namespace richClosure.Packets.ApplicationLayer
{
    class HttpPacket : TcpPacket
    {
        public Dictionary<string, string> HttpFieldsDict { get; set; }

        public HttpPacket(TcpPacket packet) : base (packet)
        {
            TcpAckNumber = packet.TcpAckNumber;
            TcpChecksum = packet.TcpChecksum;
            TcpDataOffset = packet.TcpDataOffset;
            TcpPorts = packet.TcpPorts;
            TcpFlags = packet.TcpFlags;
            TcpSequenceNumber = packet.TcpSequenceNumber;
            TcpUrgentPointer = packet.TcpUrgentPointer;
            TcpWindowSize = packet.TcpWindowSize;
            IpAppProtocol = AppProtocolEnum.HTTP;
            PacketDisplayedProtocol = "HTTP";

        }
    }
}
