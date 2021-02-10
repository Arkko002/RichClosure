namespace PacketSniffer.Packets
{
    public interface IPacketFrame
    {
        ulong PacketId { get; }
        string TimeDateCaptured { get; }
        string PacketComment { get; }
        
        //TODO what type for universal address storing?
        string DestinationAddress { get; }
        string SourceAddress { get; }
        
        IPacket Packet { get; }
    }
}