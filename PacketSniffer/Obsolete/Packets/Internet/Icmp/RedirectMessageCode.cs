namespace PacketSniffer.Packets.Internet.Icmp
{
    public enum RedirectMessageCode
    {
        RedirectDatagramForTheNetwork = 0,
        RedirectDatagramForTheHost = 1,
        RedirectDatagramForTheToSAndNetwork = 2,
        RedirectDatagramForTheToSAndHost = 3
    }
}