using System;
using PacketSniffer.Packets;

namespace richClosure.Avalonia.ViewModels
{
    public class PacketGridViewModel
    {
        public ulong Id { get; }
        public string DateTimeCaptured { get; }
        
        public string Protocol { get; }
        public string DestAddr { get; }
        public string SrcAddr { get; }
        
        public string Comment { get; }
        
        public PacketGridViewModel(IPacket packet)
        {
            Id = packet.PacketId;
            DateTimeCaptured = packet.TimeDateCaptured;
            Protocol = packet.IpProtocol;
            
        }
    }
}