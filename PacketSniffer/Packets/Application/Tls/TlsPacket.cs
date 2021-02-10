namespace PacketSniffer.Packets.Application.Tls
{
    public class TlsPacket : IApplicationPacket
    {
        public TlsContentType Type { get; }
        public string Version { get; }
        public ushort DataLength { get; }
        public string EncryptedData { get; }

        public TlsPacket(TlsContentType type, string version, ushort dataLength, string encryptedData, IPacket? previousHeader, PacketProtocol nextProtocol)
        {
            Type = type;
            Version = version;
            DataLength = dataLength;
            EncryptedData = encryptedData;
            PacketProtocol = PacketProtocol.TLS;
            PreviousHeader = previousHeader;
            NextProtocol = nextProtocol;
        }

        public PacketProtocol PacketProtocol { get; }
        public IPacket? PreviousHeader { get; }
        public PacketProtocol NextProtocol { get; }
    }
}
