using System;
using richClosure.Packets.TransportLayer;

namespace richClosure.Packets.SessionLayer
{
    public enum TlsContentTypeEnum
    {
        change_cipher_spec = 20,
        alert = 21,
        handshake = 22,
        application_data = 23,
        heartbeat = 24
    };

    public enum TlsAlertsEnum
    {
        close_notify = 0,
        unexpected_message = 10,
        bad_record_mac = 20,
        decryption_failed_RESERVED = 21,
        record_overflow = 22,
        decompression_failure_RESERVED = 30,
        handshake_failure = 40,
        no_certificate_RESERVED = 41,
        bad_certificate = 42,
        unsupported_certificate = 43,
        certificate_revoked = 44,
        certificate_expired = 45,
        certificate_unknown = 46,
        illegal_parameter = 47,
        unknown_ca = 48,
        access_denied = 49,
        decode_error = 50,
        decrypt_error = 51,
        export_restriction_RESERVED = 60,
        protocol_version = 70,
        insufficient_security = 71,
        internal_error = 80,
        inappropriate_fallback = 86,
        user_canceled = 90,
        no_renegotiation_RESERVED = 100,
        missing_extension = 109,
        unsupported_extension = 110,
        certificate_unobtainable_RESERVED = 111,
        unrecognized_name = 112,
        bad_certificate_status_response = 113,
        bad_certificate_hash_value_RESERVED = 114,
        unknown_psk_identity = 115,
        certificate_required = 116,
        no_application_protocol = 120
    };

    public enum TlsHandshakeEnum
    {
        hello_request_RESERVED = 0,
        client_hello = 1,
        server_hello = 2,
        hello_verify_request_RESERVED = 3,
        new_session_ticket = 4,
        end_of_early_data = 5,
        hello_retry_request_RESERVED = 6,
        encrypted_extensions = 8,
        certificate = 11,
        server_key_exchange_RESERVED = 12,
        certificate_request = 13,
        server_hello_done_RESERVED = 14,
        certificate_verify = 15,
        client_key_exchange_RESERVED = 16,
        finished = 20,
        certificate_url_RESERVED = 21,
        certificate_status_RESERVED = 22,
        supplemental_data_RESERVED = 23,
        key_update = 24,
        compressed_certificate = 25,
        message_hash = 254
    };

    class TlsPacket : TcpPacket
    {
        public TlsContentTypeEnum TlsType { get; set; }
        public string TlsVersion { get; set; }
        public ushort TlsDataLength { get; set; }
        public string TlsEncryptedData { get; set; }


        public TlsPacket(TcpPacket packet) : base (packet)
        {
            TcpAckNumber = packet.TcpAckNumber;
            TcpChecksum = packet.TcpChecksum;
            TcpDataOffset = packet.TcpDataOffset;
            TcpPorts = packet.TcpPorts;
            TcpFlags = packet.TcpFlags;
            TcpSequenceNumber = packet.TcpSequenceNumber;
            TcpUrgentPointer = packet.TcpUrgentPointer;
            TcpWindowSize = packet.TcpWindowSize;
            IpAppProtocol = AppProtocolEnum.TLS;
        }
    }
}
