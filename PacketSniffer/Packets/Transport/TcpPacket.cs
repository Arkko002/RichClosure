using System.Collections.Generic;

namespace PacketSniffer.Packets.Transport
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

        
        //Flags
        public bool Fin { get; }
        public bool Syn { get; }
        public bool Rst { get; }
        public bool Psh { get; }
        public bool Ack { get; }
        public bool Urg { get; }
        public bool Ece { get; }
        public bool Cwr { get; }
        public bool Ns { get; }
        
        public TcpPacket(ushort destinationPort, ushort sourcePort, uint sequenceNumber, uint ackNumber, byte dataOffset,
            ushort urgentPointer, ushort windowSize, ushort checksum, IPacket previousHeader, bool fin, bool syn,
            bool rst, bool psh, bool ack, bool urg, bool ece, bool cwr, bool ns, PacketProtocol nextProtocol)
        {
            PacketProtocol = PacketProtocol.TCP;
            DestinationPort = destinationPort;
            SourcePort = sourcePort;
            SequenceNumber = sequenceNumber;
            AckNumber = ackNumber;
            DataOffset = dataOffset;
            UrgentPointer = urgentPointer;
            WindowSize = windowSize;
            Checksum = checksum;
            PreviousHeader = previousHeader;
            Fin = fin;
            Syn = syn;
            Rst = rst;
            Psh = psh;
            Ack = ack;
            Urg = urg;
            Ece = ece;
            Cwr = cwr;
            Ns = ns;
            NextProtocol = nextProtocol;
        }

        public PacketProtocol PacketProtocol { get; }
        public IPacket? PreviousHeader { get; }
        public PacketProtocol NextProtocol { get; }
    }
}
