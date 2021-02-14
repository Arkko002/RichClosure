using System;

namespace PacketSniffer.Packets
{
    public class PacketFrame : IPacketFrame
    {
        public ulong PacketId { get; }
        public DateTime DateTimeCaptured { get; }
        public string PacketComment { get; set; }
        public string DestinationAddress { get; set; }
        public string SourceAddress { get; set; }
        public ushort DestinationPort { get; set; }
        public ushort SourcePort { get; set; }
        public IPacket Packet { get; set; }
        
        public PacketFrame(ulong packetId, DateTime dateTimeCaptured)
        {
            PacketId = packetId;
            DateTimeCaptured = dateTimeCaptured;
        }
    }
}