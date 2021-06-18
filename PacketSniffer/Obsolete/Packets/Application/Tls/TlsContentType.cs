namespace PacketSniffer.Packets.Application.Tls
{
    public enum TlsContentType
    {
        ChangeCipherSpec = 20,
        Alert = 21,
        Handshake = 22,
        ApplicationData = 23,
        Heartbeat = 24
    }
}