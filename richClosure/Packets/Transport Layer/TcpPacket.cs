using System;
using System.Collections.Generic;
using richClosure.Packets.InternetLayer;

namespace richClosure.Packets.TransportLayer
{
    class TcpFlags : IFlags
    {
        public CustomBool ACK = new CustomBool();
        public CustomBool FIN = new CustomBool();
        public CustomBool URG = new CustomBool();
        public CustomBool PSH = new CustomBool();
        public CustomBool RST = new CustomBool();
        public CustomBool ECE = new CustomBool();
        public CustomBool CWR = new CustomBool();
        public CustomBool SYN = new CustomBool();
        public CustomBool NS = new CustomBool();
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
