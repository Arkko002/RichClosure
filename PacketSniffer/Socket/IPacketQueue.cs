namespace PacketSniffer.Socket
{
    public interface IPacketQueue
    {
        void EnqueuePacket(byte[] buffer);
        byte[] DequeuePacket();
        void Clear();
    }
}