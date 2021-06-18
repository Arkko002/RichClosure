namespace PacketSniffer.Packets.Application.Tls
{
    public enum TlsHandshake
    {
        HelloRequestReserved = 0,
        ClientHello = 1,
        ServerHello = 2,
        HelloVerifyRequestReserved = 3,
        NewSessionTicket = 4,
        EndOfEarlyData = 5,
        HelloRetryRequestReserved = 6,
        EncryptedExtensions = 8,
        Certificate = 11,
        ServerKeyExchangeReserved = 12,
        CertificateRequest = 13,
        ServerHelloDoneReserved = 14,
        CertificateVerify = 15,
        ClientKeyExchangeReserved = 16,
        Finished = 20,
        CertificateUrlReserved = 21,
        CertificateStatusReserved = 22,
        SupplementalDataReserved = 23,
        KeyUpdate = 24,
        CompressedCertificate = 25,
        MessageHash = 254
    }
}