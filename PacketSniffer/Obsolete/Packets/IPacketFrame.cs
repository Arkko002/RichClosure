using System;

namespace PacketSniffer.Packets
{
    public interface IPacketFrame
    {
        ulong PacketId { get; }
        DateTime DateTimeCaptured { get; }
        
        //TODO Set comment in factories
        string PacketComment { get; set; }
        
        //TODO what type for universal address storing?
        //TODO Gather the highest layers addresses
        string DestinationAddress { get; set; }
        string SourceAddress { get; set; }
        
        ushort DestinationPort { get; set; }
        ushort SourcePort { get; set; }
        
        IPacket Packet { get; set; }
    }
}