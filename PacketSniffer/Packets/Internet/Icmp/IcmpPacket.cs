namespace PacketSniffer.Packets.Internet.Icmp
{
    public class IcmpPacket : IInternetPacket
    {      
        public IcmpType Type { get;  }
        public int Code { get; }
        public uint Checksum { get; }
        public uint Rest { get; }

        //TODO Proper code storage as enum
        public IcmpPacket(IcmpType type, int code, uint checksum, uint rest, IPacket? previousHeader, PacketProtocol nextProtocol)
        {
            PacketProtocol = PacketProtocol.ICMP;
            Type = type;
            Code = code;
            Checksum = checksum;
            Rest = rest;
            PreviousHeader = previousHeader;
            NextProtocol = nextProtocol;
        }

        public PacketProtocol PacketProtocol { get; }
        public IPacket? PreviousHeader { get; }
        public PacketProtocol NextProtocol { get; }
    }
}
