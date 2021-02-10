namespace PacketSniffer.Packets.Application.Dns
{
    public enum DnsRcode
    {
        NoError = 0,
        FormatError = 1,
        ServerFailure = 2,
        NameError = 3,
        NotImplemented = 4,
        Refused = 5,
        YxDomain = 6,
        YxrrSet = 7,
        NxrrSet = 8,
        NotAuth = 9,
        NotZone = 10,
        Badvers = 16,
        Badkey = 17,
        Badtime = 18,
        Badmode = 19,
        Badname = 20,
        Badalg = 21,
        Badtrunc = 22
    };
}