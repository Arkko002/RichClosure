using System.Net.NetworkInformation;

namespace PacketSniffer.Services
{
    //TODO
    public interface IPacketSniffer
    {
        void SniffPackets(NetworkInterface networkInterface);
        void StopSniffing();
    }
}