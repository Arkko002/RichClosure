namespace PacketSniffer.Packets.Application.Dns
{
    public class DnsQuery
    {
        public string QueryName { get; set; }
        public DnsType QueryType { get; set; }
        public DnsClass QueryClass { get; set; }
    }
}