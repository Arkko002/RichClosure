using System.Collections.Generic;

namespace PacketSniffer.Packets.Application
{
    public class HttpPacket : IApplicationPacket, IPacket
    {
        public Dictionary<string, string> HttpFieldsDict { get; }

        public HttpPacket(Dictionary<string, string> httpFieldsDict, IPacket? previousHeader, PacketProtocol nextProtocol)
        {
            PacketProtocol = PacketProtocol.HTTP;
            HttpFieldsDict = httpFieldsDict;
            PreviousHeader = previousHeader;
            NextProtocol = nextProtocol;
        }

        public PacketProtocol PacketProtocol { get; }
        public IPacket? PreviousHeader { get; }
        public PacketProtocol NextProtocol { get; }
    }
}
