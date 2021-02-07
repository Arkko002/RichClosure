using System;

namespace PacketSniffer.Packets
{
    //TODO
    public interface IPacket
    {
        
        ulong PacketId { get; }
        string TimeDateCaptured { get; }
        string PacketComment { get; }
        
        //TODO what type for universal address storing?
        Tuple<,object> DestinationAddress { get; }
        Tuple<,object> SourceAddress { get; }
        
        PacketProtocol PacketProtocol { get; }
    }
}