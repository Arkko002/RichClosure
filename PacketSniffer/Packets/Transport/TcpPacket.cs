using System.Collections.Generic;
using PacketSniffer.Packets.Internet_Layer;

namespace PacketSniffer.Packets.Transport_Layer
{
    public class TcpPacket : ITransportPacket
    {
        public ushort DestinationPort { get; }
        public ushort SourcePort { get; }
        public uint SequenceNumber { get; }
        public uint AckNumber { get; }
        public byte DataOffset { get; }
        public ushort UrgentPointer { get; }
        public ushort WindowSize { get; }
        public ushort Checksum { get; }

        public ushort Flags { get; }
        
        public TcpPacket(Dictionary<string, object> valuesDictionary) : base(valuesDictionary)
        {
            SequenceNumber = (uint)valuesDictionary["TcpSequenceNumber"];
            AckNumber = (uint)valuesDictionary["TcpAckNumber"];
            DataOffset = (byte)valuesDictionary["TcpDataOffset"];
            UrgentPointer = (ushort)valuesDictionary["TcpUrgentPointer"];
            WindowSize = (ushort)valuesDictionary["TcpWindowSize"];
            Checksum = (ushort)valuesDictionary["TcpChecksum"];
            TcpPorts = (Dictionary<string, string>)valuesDictionary["TcpPorts"];
            TcpFlags = (TcpFlags)valuesDictionary["TcpFlags"];
        }
    }
}
