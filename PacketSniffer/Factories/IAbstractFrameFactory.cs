using PacketSniffer.Packets;

namespace PacketSniffer.Factories
{
    public interface IAbstractFrameFactory
    {
        IPacketFrame CreatePacketFrame();
    }
}