namespace PacketSniffer.Packets.Application.Dns
{
    public class DnsRecord
    {
        public string RecordName { get; set; }
        public DnsType RecordType { get; set; }
        public DnsClass RecordClass { get; set; }
        public uint TimeToLive { get; set; }
        public ushort RdataLength { get; set; }
        public string Rdata { get; set; }
    }
}