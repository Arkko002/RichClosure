namespace PacketSniffer.Packets.Application.Dhcp
{
    public class DhcpPacket : IApplicationPacket
    {
        public DhcpOpcode Opcode { get; }
        public DhcpHardwareType HardType { get; }
        public byte HardAdrLength { get; }
        public byte HopCount { get; }
        public uint TransactionId { get; }
        public ushort NumOfSeconds { get; }
        public string Flags { get; }
        public string ClientIpAdr { get; }
        public string YourIpAdr { get; }
        public string ServerIpAdr { get; }
        public string GatewayIpAdr { get; }
        public string ClientHardAdr { get; }
        public string ServerName { get; }
        public string BootFilename { get; }

        public DhcpPacket(DhcpOpcode opcode, DhcpHardwareType hardType, byte hardAdrLength, byte hopCount,
            uint transactionId, ushort numOfSeconds, string flags, string clientIpAdr, string yourIpAdr,
            string serverIpAdr, string gatewayIpAdr, string clientHardAdr, string serverName, string bootFilename,
            IPacket? previousHeader, PacketProtocol nextProtocol)
        {
            Opcode = opcode;
            HardType = hardType;
            HardAdrLength = hardAdrLength;
            HopCount = hopCount;
            TransactionId = transactionId;
            NumOfSeconds = numOfSeconds;
            Flags = flags;
            ClientIpAdr = clientIpAdr;
            YourIpAdr = yourIpAdr;
            ServerIpAdr = serverIpAdr;
            GatewayIpAdr = gatewayIpAdr;
            ClientHardAdr = clientHardAdr;
            ServerName = serverName;
            BootFilename = bootFilename;
            PreviousHeader = previousHeader;
            NextProtocol = nextProtocol;
            PacketProtocol = PacketProtocol.DHCP;
        }

        public PacketProtocol PacketProtocol { get; }
        public IPacket? PreviousHeader { get; }
        public PacketProtocol NextProtocol { get; }
    }
}
