using System.Collections.Generic;
using PacketSniffer.Packets.Transport_Layer;

namespace PacketSniffer.Packets.Session_Layer
{
    public enum TlsContentTypeEnum
    {
        ChangeCipherSpec = 20,
        Alert = 21,
        Handshake = 22,
        ApplicationData = 23,
        Heartbeat = 24
    };

    public enum TlsAlertsEnum
    {
        CloseNotify = 0,
        UnexpectedMessage = 10,
        BadRecordMac = 20,
        DecryptionFailedReserved = 21,
        RecordOverflow = 22,
        DecompressionFailureReserved = 30,
        HandshakeFailure = 40,
        NoCertificateReserved = 41,
        BadCertificate = 42,
        UnsupportedCertificate = 43,
        CertificateRevoked = 44,
        CertificateExpired = 45,
        CertificateUnknown = 46,
        IllegalParameter = 47,
        UnknownCa = 48,
        AccessDenied = 49,
        DecodeError = 50,
        DecryptError = 51,
        ExportRestrictionReserved = 60,
        ProtocolVersion = 70,
        InsufficientSecurity = 71,
        InternalError = 80,
        InappropriateFallback = 86,
        UserCanceled = 90,
        NoRenegotiationReserved = 100,
        MissingExtension = 109,
        UnsupportedExtension = 110,
        CertificateUnobtainableReserved = 111,
        UnrecognizedName = 112,
        BadCertificateStatusResponse = 113,
        BadCertificateHashValueReserved = 114,
        UnknownPskIdentity = 115,
        CertificateRequired = 116,
        NoApplicationProtocol = 120
    };

    public enum TlsHandshakeEnum
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
    };

    public class TlsPacket : TcpPacket
    {
        public TlsContentTypeEnum TlsType { get; private set; }
        public string TlsVersion { get; private set; }
        public ushort TlsDataLength { get; private set; }
        public string TlsEncryptedData { get; private set; }

        public TlsPacket(Dictionary<string, object> valuesDictionary) : base(valuesDictionary)
        {
            SetTlsPacketValues(valuesDictionary);
            SetDisplayedProtocol("TLS " + TlsVersion);
        }

        private void SetTlsPacketValues(Dictionary<string, object> valuesDictionary)
        {
            TlsType = (TlsContentTypeEnum)valuesDictionary["TlsType"];
            TlsVersion = (string)valuesDictionary["TlsVersion"];
            TlsDataLength = (ushort)valuesDictionary["TlsDataLength"];
            TlsEncryptedData = (string)valuesDictionary["TlsEncryptedData"];
        }
    }
}
