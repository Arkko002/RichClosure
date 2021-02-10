using PacketSniffer.Services;

namespace PacketSniffer.Socket
{
    public interface ISnifferSocket
    {
        public void ReceivePacket();
    }
}