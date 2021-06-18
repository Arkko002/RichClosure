namespace PacketSniffer
{
    public interface ISniffedInterface
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
        string NetworkInterfaceType { get; }
    }
}