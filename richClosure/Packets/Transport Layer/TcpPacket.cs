using System.Collections.Generic;
using richClosure.Packets.Internet_Layer;

namespace richClosure.Packets.Transport_Layer
{
    class TcpFlags : IFlags
    {
        public CustomBool Ack = new CustomBool();
        public CustomBool Fin = new CustomBool();
        public CustomBool Urg = new CustomBool();
        public CustomBool Psh = new CustomBool();
        public CustomBool Rst = new CustomBool();
        public CustomBool Ece = new CustomBool();
        public CustomBool Cwr = new CustomBool();
        public CustomBool Syn = new CustomBool();
        public CustomBool Ns = new CustomBool();
    }

    class TcpPacket : IpPacket
    {

        public uint TcpSequenceNumber { get; private set; }
        public uint TcpAckNumber { get; private set; }
        public byte TcpDataOffset { get; private set; }
        public ushort TcpUrgentPointer { get; private set; }
        public ushort TcpWindowSize { get; private set; }
        public ushort TcpChecksum { get; private set; }
        public Dictionary<string, string> TcpPorts { get; private set; }
        public TcpFlags TcpFlags { get; private set; }

        public TcpPacket(Dictionary<string, object> valuesDictionary) : base(valuesDictionary)
        {
            SetTcpPacketValues(valuesDictionary);
            SetDisplayedProtocol("TCP");
        }

        private void SetTcpPacketValues(Dictionary<string, object> valuesDictionary)
        {
            TcpSequenceNumber = (uint)valuesDictionary["TcpSequenceNumber"];
            TcpAckNumber = (uint)valuesDictionary["TcpAckNumber"];
            TcpDataOffset = (byte)valuesDictionary["TcpDataOffset"];
            TcpUrgentPointer = (ushort)valuesDictionary["TcpUrgentPointer"];
            TcpWindowSize = (ushort)valuesDictionary["TcpWindowSize"];
            TcpChecksum = (ushort)valuesDictionary["TcpChecksum"];
            TcpPorts = (Dictionary<string, string>)valuesDictionary["TcpPorts"];
            TcpFlags = (TcpFlags)valuesDictionary["TcpFlags"];
        }
    }
}
